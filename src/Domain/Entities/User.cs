
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(50)]
        public required string Login { get; set; }

        [Required]
        [StringLength(255)]
        public required string Password { get; set; }

        public virtual List<UserLocation>? Locations { get; set; }

        public virtual List<Deal>? Deals { get; set; }

        public virtual List<Bid>? Bids { get; set; }

        public virtual List<Message>? Messages { get; set; }

        public virtual List<Invite>? InvitesSent { get; set; }

        public virtual List<Invite>? InvitesReceived { get; set; }
    }
}