using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface ILoginService
    {
        Task<UserDTO?> Login(LoginDTO loginDTO);
    }
}