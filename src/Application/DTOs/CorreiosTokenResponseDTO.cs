
namespace src.Application.DTOs
{
    public class CorreiosTokenResponseDTO
    {
        public string? ambiente { get; set; }
        public int id { get; set; }
        public required string perfil {get; set; }
        public required string cnpj { get; set; }
        public DateTime emissao { get; set; }
        public DateTime expiraEm { get; set; }
        public required string token { get; set; }
    }
}