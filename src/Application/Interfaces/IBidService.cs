using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IBidService
    {
        Task<BidDTO> Add(BidDTO bid);

        Task<BidDTO> Update(BidDTO bid);

        Task<BidDTO> Get(int dealId, int bidId);

        Task<List<BidDTO>> GetAllByDeal(int dealId);
    }
}