
using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> Add(MessageDTO user);

        Task<MessageDTO> Update(MessageDTO user);

        Task<MessageDTO> Get(int id);

        Task<List<MessageDTO>> GetAllByDeal(int dealId);
    }
}