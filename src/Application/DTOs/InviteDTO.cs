
namespace src.Application.DTOs
{
    public class InviteDTO
    {
        public int InviteId { get; set; }

        public required int UserId { get; set; }

        public required int UserInvitedId { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public UserDTO? User { get; set; }

        public UserDTO? UserInvited { get; set; }


    }
}