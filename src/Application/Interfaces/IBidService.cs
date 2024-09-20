using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IBidService
    {
        Task<BidDTO> Add(BidDTO user);

        Task<BidDTO> Update(BidDTO user);

        Task<BidDTO> Get(int id);

        Task<List<BidDTO>> GetAllByDeal(int dealId);
    }
}