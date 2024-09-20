using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Infrastructure.Db;

namespace src.Application.Services
{
    public class BidService : IBidService
    {
        private readonly MyDbContext _context;

        public BidService(MyDbContext context)
        {
            this._context = context;
        }

        public Task<BidDTO> Add(BidDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<BidDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetAllByDeal(int dealId)
        {
            throw new NotImplementedException();
        }

        public Task<BidDTO> Update(BidDTO user)
        {
            throw new NotImplementedException();
        }
    }
}