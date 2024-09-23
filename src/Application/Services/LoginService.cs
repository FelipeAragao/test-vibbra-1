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
    public class LoginService : ILoginService
    {
        private readonly MyDbContext _context;

        public LoginService(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<UserDTO?> Login(LoginDTO loginDTO)
        {   
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
    }
}