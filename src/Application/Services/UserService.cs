using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Infrastructure.Db;
using src.Infrastructure.Security;

namespace src.Application.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<UserDTO> Add(UserDTO user)
        {
            // Validates
            // Check if user has location
            if (user.Location == null)
            {
                throw new Exception("The location is incomplete or blank");
            }
            // Check if login already exists
            bool loginExists = await _context.Users.AnyAsync(u => u.Login == user.Login);
            if (loginExists)
            {
                throw new Exception("Login already exists");
            }

            // Hash password
            byte[] salt = PasswordHasher.GenerateSalt();
            user.Password = PasswordHasher.HashPassword(user.Password, salt);

            // Add user
            var userEntity = UserMapper.ToEntity(user);
            await this._context.Users.AddAsync(userEntity);
            await this._context.SaveChangesAsync();
            user.UserId = userEntity.UserId;
            return user;
        }

        public async Task<UserDTO> Update(UserDTO user)
        {
            // Look for the user
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if(existingUser == null)
            {
                throw new Exception("User not found");
            }
            // Validates
            if(existingUser.Login != user.Login)
            {
                throw new Exception("Login can't be changed");
            }

            if(existingUser.Password != user.Password)
            {
                byte[] salt = PasswordHasher.GenerateSalt();
                user.Password = PasswordHasher.HashPassword(user.Password, salt);
            }

            // Update the user's properties
            UserMapper.UpdateEntityFromDTO(existingUser, user);

            // Update
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDTO> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return UserMapper.ToDTO(user);
        }
    }
}