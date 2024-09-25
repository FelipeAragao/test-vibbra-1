
using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task<DeliveryDTO> Add(int dealId, int userId);

        Task<DeliveryDTO> Get(int dealId);
    }
}