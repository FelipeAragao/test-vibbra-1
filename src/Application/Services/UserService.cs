using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<UserDTO?> Login(LoginDTO loginDTO)
        {   
            try {
                var user = await this._context.Users.Where(x => x.Login == loginDTO.Login)
                    .Include(u => u.Locations)
                    .FirstOrDefaultAsync();
                UserDTO? userDTO = null;
                if(user != null && PasswordHasher.VerifyPassword(loginDTO.Password, user.Password))
                {
                    userDTO = UserMapper.ToDTO(user);
                }
                return userDTO;
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDTO> Add(UserDTO user)
        {
            try {
                // Validates
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
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while updating the deal. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<UserDTO> Update(UserDTO user)
        {
            try {
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
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while updating the deal. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<UserDTO> Get(int userId)
        {
            try {
                var user = await _context.Users
                    .Include(u => u.Locations)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if(user == null)
                {
                    throw new Exception("User not found");
                }
                return UserMapper.ToDTO(user);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<UserDTO?> GetByLogin(string login)
        {
            try {
                var user = await _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
                return user == null ? null : UserMapper.ToDTO(user);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}