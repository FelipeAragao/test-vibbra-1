using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IDealService
    {
        Task<DealDTO> Add(DealDTO deal);

        Task<DealDTO> Update(DealDTO deal);

        Task<DealDTO> Get(int id);
    }
}