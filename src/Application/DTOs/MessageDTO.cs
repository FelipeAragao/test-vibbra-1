using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.DTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }

        public required int DealId { get; set; }

        public required int UserId { get; set; }

        public required string Title { get; set; }

        public required string Message { get; set; }
    }
}