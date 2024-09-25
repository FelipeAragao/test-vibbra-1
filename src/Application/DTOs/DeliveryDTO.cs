using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.DTOs
{
    public class DeliveryDTO
    {
        public int DeliveryId { get; set; }

        public int DealId { get; set; }

        public int UserId { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        public List<DeliveryStepsDTO>? Steps { get; set; }
    }
}