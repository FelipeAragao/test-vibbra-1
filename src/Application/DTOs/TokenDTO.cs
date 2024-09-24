
namespace src.Application.DTOs
{
    public class TokenDTO
    {
        public required string Token { get; set; }

        public required UserDTO User { get; set; }
    }
}