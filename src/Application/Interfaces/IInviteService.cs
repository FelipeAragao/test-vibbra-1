using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface IInviteService
    {
        Task<InviteDTO> Add(InviteDTO inviteDTO);

        Task<InviteDTO> Update(InviteDTO inviteDTO);

        Task<InviteDTO> Get(int id);

        Task<List<InviteDTO>> GetAllByUser(int userId);
    }
}