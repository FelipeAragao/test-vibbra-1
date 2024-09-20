using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IDealService
    {
        Task<DealDTO> Add(DealDTO user);

        Task<DealDTO> Update(DealDTO user);

        Task<DealDTO> Get(int id);
    }
}