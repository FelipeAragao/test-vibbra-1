using System;
using System.Collections.Generic;
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
            var msgEntity = MessageMapper.ToEntity(msg);
            await this._context.AddAsync(msgEntity);
            await this._context.SaveChangesAsync();

            // Update MessageDTO
            msg.MessageId = msgEntity.MessageId;
            return msg;
        }

        public async Task<MessageDTO> Get(int id)
        {
            var msg = await _context.Messages.FindAsync(id);
            if(msg == null)
            {
                throw new Exception("Message not found");
            }
            return MessageMapper.ToDTO(msg);
        }

        public async Task<List<MessageDTO>> GetAllByDeal(int dealId)
        {
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

        public async Task<MessageDTO> Update(MessageDTO msg)
        {
            // Look for the message
            var existingMsg = await _context.Messages.FindAsync(msg.MessageId);
            if (existingMsg == null)
            {
                throw new Exception("Message not found");
            }

            // Update the message's properties
            MessageMapper.UpdateEntityFromDTO(existingMsg, msg);

            // Update
            await _context.SaveChangesAsync();

            return msg;
        }
    }
}