using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BidDTO> Add(BidDTO bid)
        {
            var bidEntity = BidMapper.ToEntity(bid);
            await this._context.AddAsync(bidEntity);
            await this._context.SaveChangesAsync();

            // Update BidDTO
            bid.BidId = bidEntity.BidId;
            bid.DealId = bidEntity.DealId;
            return bid;
        }

        public async Task<BidDTO> Get(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if(bid == null)
            {
                throw new Exception("Bid not found");
            }
            return BidMapper.ToDTO(bid);
        }

        public async Task<List<BidDTO>> GetAllByDeal(int dealId)
        {
            var listBids = await _context.Bids.Where(b => b.DealId == dealId).ToListAsync();
            if(listBids.Count == 0)
            {
                throw new Exception("Bids for deal not found");
            }
            List<BidDTO> listBidDTO = new List<BidDTO>();
            foreach(var bid in listBids)
            {
                listBidDTO.Add(BidMapper.ToDTO(bid));
            }
            return listBidDTO;
        }

        public async Task<BidDTO> Update(BidDTO bid)
        {
            // Look for the bid
            var existingBid = await _context.Bids.FindAsync(bid.BidId);
            if (existingBid == null)
            {
                throw new Exception("Bid not found");
            }

            // Update the bid's properties
            BidMapper.UpdateEntityFromDTO(existingBid, bid);

            // Update
            await _context.SaveChangesAsync();

            return bid;
        }
    }
}