using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Domain.Entities;
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

        public async Task<DealDTO> Add(DealDTO deal)
        {
            // Validates
            // Check if user has location
            if (deal.Location == null)
            {
                throw new Exception("The location is incomplete or blank");
            }

            var dealEntity = DealMapper.ToEntity(deal);
            await this._context.AddAsync(dealEntity);
            await this._context.SaveChangesAsync();

            // Update DealDTO
            deal.DealId = dealEntity.DealId;
            for(int i = 0; i < dealEntity.DealImages.Count; i++)
            {
                deal.DealImages[i].DealImageId = dealEntity.DealImages[i].DealImageId;
                deal.DealImages[i].DealId = dealEntity.DealImages[i].DealId;
            }
            return deal;
        }

        public async Task<DealDTO> Update(DealDTO deal)
        {
            // Look for the deal
            var existingDeal = await _context.Deals.FindAsync(deal.DealId);
            if (existingDeal == null)
            {
                throw new Exception("Deal not found");
            }

            // Update the deal's properties
            DealMapper.UpdateEntityFromDTO(existingDeal, deal);

            // Update
            await _context.SaveChangesAsync();

            return deal;
        }

        public async Task<DealDTO> Get(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if(deal == null)
            {
                throw new Exception("Deal not found");
            }
            return DealMapper.ToDTO(deal);
        }
    }
}