using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using src.Domain.Enums;

namespace src.Domain.Entities
{
    public class DeliverySteps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryStepsId { get; set; }

        [Required]
        [ForeignKey("Delivery")]
        public int DeliveryId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required DeliveryStatus DeliveryStatus { get; set; }

        public bool Active { get; set; } = false;

        public Delivery? Delivery { get; set; }
    }
}