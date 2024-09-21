using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.Mappers
{
using src.Domain.Entities;
using src.Application.DTOs;

public static class MessageMapper
{
    public static MessageDTO ToDTO(Message message)
    {
        return new MessageDTO
        {
            MessageId = message.MessageId,
            DealId = message.DealId,
            UserId = message.UserId,
            Title = message.Title,
            Message = message.TextMessage
        };
    }

    public static Message ToEntity(MessageDTO messageDTO)
    {
        return new Message
        {
            MessageId = messageDTO.MessageId,
            DealId = messageDTO.DealId,
            UserId = messageDTO.UserId,
            Title = messageDTO.Title,
            TextMessage = messageDTO.Message
        };
    }

    public static void UpdateEntityFromDTO(Message message, MessageDTO messageDTO)
    {
        message.DealId = messageDTO.DealId;
        message.UserId = messageDTO.UserId;
        message.Title = messageDTO.Title;
        message.TextMessage = messageDTO.Message;
    }
}

}