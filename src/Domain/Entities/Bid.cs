using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Domain.Entities
{
    [Table("Bids")]
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BidId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Deal")]
        public int DealId { get; set; }

        public bool Accepted { get; set; } = false;

        [Required]
        public required decimal Value { get; set; }

        [Required]
        [StringLength(150)]
        public required string Description { get; set; }
    }
}