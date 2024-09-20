using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Domain.Entities;
using src.Infrastructure.Db;

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
            var user = await this._context.Users.Where(x => x.Login == loginDTO.Login && x.Password == loginDTO.Password)
                .Include(u => u.Locations)
                .FirstOrDefaultAsync();
            UserDTO? userDTO = null;
            if(user != null)
            {
                LocationDTO? locationDTO = null;
                if(user.Locations != null)
                    locationDTO = new LocationDTO() {
                    Lat = user.Locations[0].Lat,
                    Lng = user.Locations[0].Lat,
                    Address = user.Locations[0].Address,
                    City = user.Locations[0].City,
                    State = user.Locations[0].State,
                    ZipCode = user.Locations[0].ZipCode
                };

                userDTO = new UserDTO() {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Login = user.Login,
                    Password = user.Password,
                    Location = locationDTO
                };
            }
            return userDTO;
        }

        public Task<UserDTO> Add(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> Update(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}