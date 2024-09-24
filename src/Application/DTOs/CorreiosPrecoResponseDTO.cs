using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.DTOs
{
    public class CorreiosPrecoServicoAdicionalDTO
    {
        public string? CoServAdicional { get; set; }
        public string? TpServAdicional { get; set; }
        public decimal PcServicoAdicional { get; set; }
    }

    public class CorreiosPrecoResponseDTO
    {
        public string? CoProduto { get; set; }
        public decimal PcBase { get; set; }
        public decimal PcBaseGeral { get; set; }
        public decimal PeVariacao { get; set; }
        public decimal PcReferencia { get; set; }
        public decimal VlBaseCalculoImposto { get; set; }
        public string? InPesoCubico { get; set; }
        public int PsCobrado { get; set; }
        public List<CorreiosPrecoServicoAdicionalDTO>? ServicoAdicional { get; set; }
        public decimal PeAdValorem { get; set; }
        public decimal VlSeguroAutomatico { get; set; }
        public int QtAdicional { get; set; }
        public decimal PcFaixa { get; set; }
        public decimal PcFaixaVariacao { get; set; }
        public decimal PcProduto { get; set; }
        public decimal PcTotalServicosAdicionais { get; set; }
        public decimal PcFinal { get; set; }
    }
}