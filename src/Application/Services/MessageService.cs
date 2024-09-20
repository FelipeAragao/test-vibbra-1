using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Application.DTOs;
using src.Application.Interfaces;
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
        public Task<MessageDTO> Add(MessageDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MessageDTO>> GetAllByDeal(int dealId)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDTO> Update(MessageDTO user)
        {
            throw new NotImplementedException();
        }
    }
}