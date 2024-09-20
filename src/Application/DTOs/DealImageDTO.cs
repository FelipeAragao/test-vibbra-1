
namespace src.Application.DTOs
{
    public class DealImageDTO
    {
        public int DealImageId { get; set; }

        public required int DealId { get; set; }

        public required string ImageUrl { get; set; }
    }
}