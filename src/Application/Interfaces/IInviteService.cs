using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IInviteService
    {
        Task<InviteDTO> Add(InviteDTO user);

        Task<InviteDTO> Update(InviteDTO user);

        Task<InviteDTO> Get(int id);

        Task<List<InviteDTO>> GetAllByUser(int dealId);
    }
}