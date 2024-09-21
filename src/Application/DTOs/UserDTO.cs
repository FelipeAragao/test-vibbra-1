
using System.ComponentModel.DataAnnotations;

namespace src.Application.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public required string Email { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public LocationDTO? Location { get; set; }
    }
}