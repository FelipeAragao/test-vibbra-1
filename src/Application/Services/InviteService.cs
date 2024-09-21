using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Infrastructure.Db;

namespace src.Application.Services
{
    public class InviteService : IInviteService
    {
        private readonly MyDbContext _context;

        public InviteService(MyDbContext context)
        {
            this._context = context;
        }
        
        public async Task<InviteDTO> Add(InviteDTO inviteDTO)
        {
            var inviteEntity = Mappers.InviteMapper.ToEntity(inviteDTO);
            await this._context.AddAsync(inviteEntity);
            await this._context.SaveChangesAsync();

            // Update InviteDTO
            inviteDTO.InviteId = inviteEntity.InviteId;
            return inviteDTO;
        }

        public async Task<InviteDTO> Get(int id)
        {
            var invite = await _context.Invites.FindAsync(id);
            if(invite == null)
            {
                throw new Exception("Invite not found");
            }
            return Mappers.InviteMapper.ToDTO(invite);
        }

        public async Task<List<InviteDTO>> GetAllByUser(int userId)
        {
            var listInvites = await _context.Invites.Where(b => b.UserId == userId).ToListAsync();
            if(listInvites.Count == 0)
            {
                throw new Exception("Invites for user not found");
            }
            List<InviteDTO> listInvitesDTO = new List<InviteDTO>();
            foreach(var invite in listInvites)
            {
                listInvitesDTO.Add(Mappers.InviteMapper.ToDTO(invite));
            }
            return listInvitesDTO;
        }

        public async Task<InviteDTO> Update(InviteDTO inviteDTO)
        {
            // Look for the invite
            var existingInvite = await _context.Invites.FindAsync(inviteDTO.InviteId);
            if (existingInvite == null)
            {
                throw new Exception("Invite not found");
            }

            // Update the invite's properties
            InviteMapper.UpdateEntityFromDTO(existingInvite, inviteDTO);

            // Update
            await _context.SaveChangesAsync();

            return inviteDTO;
        }
    }
}