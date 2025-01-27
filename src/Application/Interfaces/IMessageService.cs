
using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> Add(MessageDTO msg);

        Task<MessageDTO> Update(MessageDTO msg);

        Task<MessageDTO> Get(int dealId, int messageId);

        Task<List<MessageDTO>> GetAllByDeal(int dealId);
    }
}