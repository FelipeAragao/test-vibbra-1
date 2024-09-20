
namespace src.Application.DTOs
{
    public class LocationDTO
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public required string Address { get; set; }

        public required string City { get; set; }

        public required string State { get; set; }

        public int ZipCode { get; set; }
    }
}