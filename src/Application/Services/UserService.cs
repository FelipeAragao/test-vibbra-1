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

        public User? Login(LoginDTO loginDTO)
        {
            return this._context.Users.Where(x => x.Login == loginDTO.Login && x.Password == loginDTO.Password).FirstOrDefault();
        }
    }
}