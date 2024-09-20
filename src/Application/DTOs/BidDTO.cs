
namespace src.Application.DTOs
{
    public class BidDTO
    {
        public int BidId { get; set; }

        public int UserId { get; set; }

        public int DealId { get; set; }

        public bool Accepted { get; set; } = false;

        public required decimal Value { get; set; }

        public required string Description { get; set; }
    }
}