
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Domain.Enums;

namespace src.Domain.Entities
{
    [Table("Deals")]
    public class Deal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public DealType Type { get; set; }

        [Required]
        public required decimal Value { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? TradeFor { get; set; }

        [Required]
        public DealUrgencyType UrgencyType { get; set; }

        public DealLocation? Location { get; set; }

        public List<DealImage>? DealImages { get; set; }

        public List<Bid>? Bids { get; set; }

        public List<Message>? Messages { get; set; }

        public Delivery? Delivery { get; set; }
    }
}