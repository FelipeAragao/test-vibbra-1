using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Domain.Entities
{
    [Table("DealLocation")]
    public class DealLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealLocationId { get; set; }

        [Required]
        [ForeignKey("Deal")]
        public required int DealId { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lng { get; set; }

        [Required]
        [StringLength(150)]
        public required string Address { get; set; }

        [Required]
        [StringLength(100)]
        public required string City { get; set; }

        [Required]
        [StringLength(2)]
        public required string State { get; set; }

        [Required]
        public int ZipCode { get; set; }
    }
}