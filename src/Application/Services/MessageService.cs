using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Mappers;
using src.Infrastructure.Db;

namespace src.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly MyDbContext _context;

        public MessageService(MyDbContext context)
        {
            this._context = context;
        }
        public async Task<MessageDTO> Add(MessageDTO msg)
        {
            try {
                var msgEntity = MessageMapper.ToEntity(msg);
                await this._context.AddAsync(msgEntity);
                await this._context.SaveChangesAsync();

                // Update MessageDTO
                msg.MessageId = msgEntity.MessageId;
                return msg;
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

        public async Task<MessageDTO> Get(int dealId, int messageId)
        {
            try {
                var msg = await _context.Messages.Where(m => m.DealId == dealId && m.MessageId == messageId).FirstOrDefaultAsync();
                if(msg == null)
                {
                    throw new Exception("Message not found or Deal not correspond to message. Check the data.");
                }
                return MessageMapper.ToDTO(msg);
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<List<MessageDTO>> GetAllByDeal(int dealId)
        {
            try {
                var listMessages = await _context.Messages.Where(b => b.DealId == dealId).ToListAsync();
                if(listMessages.Count == 0)
                {
                    throw new Exception("Messages for deal not found");
                }
                List<MessageDTO> listMessageDTO = new List<MessageDTO>();
                foreach(var msg in listMessages)
                {
                    listMessageDTO.Add(MessageMapper.ToDTO(msg));
                }
                return listMessageDTO;
            }
            catch (DBConcurrencyException ex) {
                throw new Exception("The database is busy. Try again later. " + ex.Message);
            }
            catch (Exception ex) {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<MessageDTO> Update(MessageDTO msg)
        {
            try {
                // Look for the message
                var existingMsg = await _context.Messages.Where(m => m.DealId == msg.DealId && m.MessageId == msg.MessageId).FirstOrDefaultAsync();
                if (existingMsg == null)
                {
                    throw new Exception("Message not found or Deal not correspond to message. Check the data.");
                }

                // Update the message's properties
                MessageMapper.UpdateEntityFromDTO(existingMsg, msg);

                // Update
                await _context.SaveChangesAsync();

                return msg;
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