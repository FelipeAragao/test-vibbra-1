using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Domain.Entities;
using src.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            try {
                // Validates
                // Check if user has location
                if (deal.Location == null)
                {
                    throw new Exception("The location is incomplete or blank");
                }

                var dealEntity = DealMapper.ToEntity(deal);
                await this._context.Deals.AddAsync(dealEntity);
                await this._context.SaveChangesAsync();

                // Update DealDTO
                deal.DealId = dealEntity.DealId;
                if (deal.DealImages != null && dealEntity.DealImages != null && deal.DealImages.Count > 0)
                {
                    for(int i = 0; i < dealEntity.DealImages.Count; i++)
                    {
                        deal.DealImages[i].DealImageId = dealEntity.DealImages[i].DealImageId;
                        deal.DealImages[i].DealId = dealEntity.DealImages[i].DealId;
                    }
                }
                return deal;
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while adding the deal. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<DealDTO> Update(DealDTO deal)
        {
            try {
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

        public async Task<DealDTO> Get(int id)
        {
            try {
                var deal = await _context.Deals
                    .Include(d => d.Location)
                    .Include(d => d.DealImages)
                    .FirstOrDefaultAsync(d => d.DealId == id);
                if(deal == null)
                {
                    throw new Exception("Deal not found");
                }
                return DealMapper.ToDTO(deal);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}