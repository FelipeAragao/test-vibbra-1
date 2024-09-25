using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using src.Domain.Enums;

namespace src.Application.DTOs
{
    public class DeliveryStepsDTO
    {
        public int DeliveryStepsId { get; set; }

        public int DeliveryId { get; set; }

        public required DateTime Date { get; set; }

        public required string DeliveryStatus { get; set; }

        public bool Active { get; set; } = false;
    }
}