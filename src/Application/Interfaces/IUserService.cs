using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Application.DTOs;
using src.Domain.Entities;

namespace src.Application.Interfaces
{
    public interface IUserService
    {
        User? Login(LoginDTO loginDTO);
    }
}