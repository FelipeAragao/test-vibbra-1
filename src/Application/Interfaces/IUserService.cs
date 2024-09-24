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
        Task<UserDTO?> Login(LoginDTO loginDTO);

        Task<UserDTO> Add(UserDTO user);

        Task<UserDTO> Update(UserDTO user);

        Task<UserDTO> Get(int userId);

        Task<UserDTO?> GetByLogin(string login);
    }
}