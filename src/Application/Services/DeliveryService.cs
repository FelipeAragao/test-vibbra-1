using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Domain.Entities;
using src.Domain.Enums;
using src.Infrastructure.Db;

namespace src.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly MyDbContext _context;

        public DeliveryService(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<DeliveryDTO> Add(int dealId, int userId)
        {
            try {
                var deal = await _context.Deals.FindAsync(dealId);
                if (deal == null)
                    throw new Exception("Deal not found.");

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found.");

                // Implementation not tested, without data for authorization on Correios
                /*
                CorreiosPrecoRequestDTO precoRequestDTO = new CorreiosPrecoRequestDTO {
                    CepOrigem = deal.Location?.ZipCode.ToString(),
                    CepDestino = user.Locations?[0].ZipCode.ToString(),
                    ServicosAdicionais = null,
                    VlDeclarado = deal.Value
                };
                var correiosResponse = await this._correiosPrecoApiService.GetPrecoAsync(precoRequestDTO);*/
                CorreiosPrecoResponseDTO correiosResponse = new CorreiosPrecoResponseDTO {
                    PcFinal = 10
                };

                Delivery delivery = new Delivery {
                    DealId = dealId,
                    UserId = userId,
                    DeliveryPrice = correiosResponse.PcFinal,
                    Steps = [ new DeliverySteps() {
                        Date = DateTime.Now,
                        DeliveryStatus = DeliveryStatus.OrderReceived,
                        Active = true
                    } ]
                };

                await this._context.Deliveries.AddAsync(delivery);
                await this._context.SaveChangesAsync();
                return DeliveryMapper.ToDTO(delivery);
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message;
                throw new Exception($"An error occurred while adding the delivery. Details: {innerException}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<DeliveryDTO> Get(int dealId)
        {
            try {
                var delivery = await _context.Deliveries.Where(d => d.DealId == dealId)
                    .Include(d => d.Steps)
                    .FirstOrDefaultAsync();
                if (delivery == null)
                {
                    throw new Exception("Delivery not found. Check the data.");
                }
                return DeliveryMapper.ToDTO(delivery);
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