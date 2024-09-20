using src.Application.DTOs;
using src.Application.Interfaces;
using src.Infrastructure.Db;

namespace src.Application.Services
{
    public class DealService : IDealService
    {
        private readonly MyDbContext _context;

        public DealService(MyDbContext context)
        {
            this._context = context;
        }

        public Task<DealDTO> Add(DealDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<DealDTO> Update(DealDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<DealDTO?> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}