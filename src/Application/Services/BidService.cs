using System.Data;
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
            try {
                var bidEntity = BidMapper.ToEntity(bid);
                await this._context.AddAsync(bidEntity);
                await this._context.SaveChangesAsync();

                // Update BidDTO
                bid.BidId = bidEntity.BidId;
                bid.DealId = bidEntity.DealId;
                return bid;
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

        public async Task<BidDTO> Get(int dealId, int bidId)
        {
            try {
                var bid = await _context.Bids.Where(b => b.DealId == dealId && b.BidId == bidId).FirstOrDefaultAsync();
                if(bid == null)
                {
                    throw new Exception("Bid not found or Deal not correspond to bid. Check the data.");
                }
                return BidMapper.ToDTO(bid);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }

        }

        public async Task<List<BidDTO>> GetAllByDeal(int dealId)
        {
            try {
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
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<List<BidDTO>> GetAllByDealAccepted(int dealId)
        {
            try {
                var listBids = await _context.Bids.Where(b => b.DealId == dealId && b.Accepted).ToListAsync();
                if(listBids.Count == 0)
                {
                    throw new Exception("Accepted bids for deal not found");
                }
                List<BidDTO> listBidDTO = new List<BidDTO>();
                foreach(var bid in listBids)
                {
                    listBidDTO.Add(BidMapper.ToDTO(bid));
                }
                return listBidDTO;
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<BidDTO> Update(BidDTO bid)
        {
            try {
                // Look for the bid
                var existingBid = await _context.Bids.Where(b => b.DealId == bid.DealId && b.BidId == bid.BidId).FirstOrDefaultAsync();
                if (existingBid == null)
                {
                    throw new Exception("Bid not found or Deal not correspond to bid. Check the data.");
                }

                // Update the bid's properties
                BidMapper.UpdateEntityFromDTO(existingBid, bid);

                // Update
                await _context.SaveChangesAsync();

                return bid;
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