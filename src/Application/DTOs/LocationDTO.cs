
using System.ComponentModel.DataAnnotations;

namespace src.Application.DTOs
{
    public class LocationDTO
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public required string Address { get; set; }

        public required string City { get; set; }

        [StringLength(2, ErrorMessage = "The state cannot be longer than 2 characters.")]
        public required string State { get; set; }

        public int ZipCode { get; set; }
    }
}