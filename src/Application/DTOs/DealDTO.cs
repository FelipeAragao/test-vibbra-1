using src.Domain.Enums;

namespace src.Application.DTOs
{
    public class DealDTO
    {
        public int DealId { get; set; }

        public int UserId { get; set; }

        public DealType Type { get; set; }

        public required decimal Value { get; set; }

        public string? Description { get; set; }

        public string? TradeFor { get; set; }

        public DealUrgencyType UrgencyType { get; set; }

        public required LocationDTO Location { get; set; }

        public required List<DealImageDTO> DealImages { get; set; }
    }
}