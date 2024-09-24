using System.Data;
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
            try {
                var inviteEntity = Mappers.InviteMapper.ToEntity(inviteDTO);
                await this._context.AddAsync(inviteEntity);
                await this._context.SaveChangesAsync();

                // Update InviteDTO
                inviteDTO.InviteId = inviteEntity.InviteId;
                return inviteDTO;
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while updating the deal. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<InviteDTO> Get(int userId, int inviteId)
        {
            try {
                var invite = await _context.Invites.Where(i => i.UserId == userId && i.InviteId == inviteId).FirstOrDefaultAsync();
                if(invite == null)
                {
                    throw new Exception("Invite not found or user not correspond to invite. Check the data.");
                }
                return Mappers.InviteMapper.ToDTO(invite);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<List<InviteDTO>> GetAllByUser(int userId)
        {
            try {
                var listInvites = await _context.Invites.Where(i => i.UserId == userId).ToListAsync();
                if(listInvites.Count == 0)
                {
                    throw new Exception("Invite not found or user not correspond to invite. Check the data.");
                }
                List<InviteDTO> listInvitesDTO = new List<InviteDTO>();
                foreach(var invite in listInvites)
                {
                    listInvitesDTO.Add(Mappers.InviteMapper.ToDTO(invite));
                }
                return listInvitesDTO;
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<InviteDTO> Update(InviteDTO inviteDTO)
        {
            try {
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
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while updating the deal. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}