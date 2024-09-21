using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using src.Application.DTOs;
using src.Domain.Enums;
using Tests.Configuration;

namespace Tests.Utilities
{
    public static class RandomDataGenerator
    {
        public static UserDTO GenerateUserDTO()
        {
            UserDTO userDTO = new AutoFaker<UserDTO>(AutoBogusConfiguration.LOCATE)
                .Ignore(u => u.UserId)
                .RuleFor(u => u.Name, faker => faker.Person.FullName)
                .RuleFor(u => u.Login, faker => faker.Person.UserName)
                .RuleFor(u => u.Password, faker => faker.Person.Random.AlphaNumeric(10))
                .RuleFor(u => u.Email, faker => faker.Person.Email);
            LocationDTO locationDTO = GenerateLocationDTO();
            userDTO.Location = locationDTO;


            return userDTO;
        }
        public static LocationDTO GenerateLocationDTO()
        {
            LocationDTO locationDTO = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                .RuleFor(l => l.City, faker => faker.Address.City())
                .RuleFor(l => l.State, faker => faker.Address.StateAbbr())
                .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                .RuleFor(l => l.Lng, faker => faker.Address.Longitude());

            return locationDTO;
        }

        public static DealDTO GenerateDealDTO(int userId)
        {
            // Create Deal for the User
            DealDTO dealDTO = new AutoFaker<DealDTO>(AutoBogusConfiguration.LOCATE)
                .Ignore(d => d.DealId)
                .RuleFor(d => d.Value, faker => faker.Finance.Amount(5, 5000, 2))
                .RuleFor(d => d.Description, faker => faker.Lorem.Sentence(5));
            List<DealImageDTO> listImages = new List<DealImageDTO>();
            for(int i = 0; i < 3; i++)
            {
                DealImageDTO imageDTO = new AutoFaker<DealImageDTO>(AutoBogusConfiguration.LOCATE)
                    .Ignore(i => i.DealId)
                    .Ignore(i => i.DealImageId)
                    .RuleFor(i => i.ImageUrl, faker => faker.Internet.Url());
                listImages.Add(imageDTO);
            }
            dealDTO.UserId = userId;
            dealDTO.Type = DealType.Venda;
            dealDTO.UrgencyType = DealUrgencyType.Baixa;
            dealDTO.Location = GenerateLocationDTO();
            dealDTO.DealImages = listImages;

            return dealDTO;
        }

        public static BidDTO GenerateBidDTO(int userId, int dealId)
        {
            BidDTO bidDTO = new AutoFaker<BidDTO>(AutoBogusConfiguration.LOCATE)
                .Ignore(b => b.BidId)
                .RuleFor(b => b.Value, faker => faker.Finance.Amount(5, 5000, 2))
                .RuleFor(b => b.Description, faker => faker.Lorem.Sentence(5));
            bidDTO.UserId = userId;
            bidDTO.DealId = dealId;

            return bidDTO;
        }

        public static MessageDTO GenerateMessageDTO(int userId, int dealId)
        {
            MessageDTO messageDTO = new AutoFaker<MessageDTO>(AutoBogusConfiguration.LOCATE)
                .Ignore(m => m.MessageId)
                .RuleFor(m => m.Title, faker => faker.Lorem.Lines(1))
                .RuleFor(m => m.Message, faker => faker.Lorem.Lines(4));
            messageDTO.UserId = userId;
            messageDTO.DealId = dealId;

            return messageDTO;
        }

        public static InviteDTO GenerateInviteDTO(int userInviteId, int userInvitedId)
        {
            InviteDTO inviteDTO = new AutoFaker<InviteDTO>(AutoBogusConfiguration.LOCATE)
                .Ignore(i => i.InviteId)
                .Ignore(i => i.User)
                .Ignore(i => i.UserInvited)
                .RuleFor(i => i.Name, faker => faker.Person.FullName)
                .RuleFor(i => i.Email, faker => faker.Person.Email);
            inviteDTO.UserId = userInviteId;
            inviteDTO.UserInvitedId = userInvitedId;

            return inviteDTO;
        }
    }
}