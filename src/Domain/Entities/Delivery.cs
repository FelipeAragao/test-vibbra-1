using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Domain.Entities
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryId { get; set; }

        [Required]
        [ForeignKey("Deal")]
        public int DealId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public decimal DeliveryPrice { get; set; }

        public List<DeliverySteps>? Steps { get; set; }
    }
}