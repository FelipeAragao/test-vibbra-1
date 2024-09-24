
namespace src.Application.DTOs
{
    public class CorreiosPrecoRequestDTO
    {
        public string? CepDestino { get; set; }
        public string? CepOrigem { get; set; }
        public int PsObjeto { get; set; }
        public int TpObjeto { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public List<string>? ServicosAdicionais { get; set; }
        public decimal VlDeclarado { get; set; }
    }
}