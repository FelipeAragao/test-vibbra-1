using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.DTOs
{
    public class DeliveryDTO
    {
        public required DealDTO DealDTO { get; set; }
        public required BidDTO BidDTO { get; set; }
    }
}