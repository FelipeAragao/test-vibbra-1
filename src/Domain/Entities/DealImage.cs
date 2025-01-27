using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Domain.Entities
{
    [Table("DealImages")]
    public class DealImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealImageId { get; set; }

        [Required]
        [ForeignKey("Deal")]
        public required int DealId { get; set; }

        [Required]
        [StringLength(255)]
        public required string ImageUrl { get; set; }
    }
}