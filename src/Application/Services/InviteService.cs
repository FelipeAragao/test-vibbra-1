using src.Application.DTOs;
using src.Application.Interfaces;
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
        
        public Task<InviteDTO> Add(InviteDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<InviteDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<InviteDTO>> GetAllByUser(int dealId)
        {
            throw new NotImplementedException();
        }

        public Task<InviteDTO> Update(InviteDTO user)
        {
            throw new NotImplementedException();
        }
    }
}