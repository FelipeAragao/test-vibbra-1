using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Domain.Enums;
using src.Infrastructure.Db;
using Tests.Configuration;

namespace Tests.Services
{
    public class MessageServiceTest
    {
        private readonly MyDbContext _dbContext;

        public MessageServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        /* Create a MessageDTO with a deal (created by the method or received) and a user (created by the method) */
        public async Task<MessageDTO> CreateMessageDTO(int dealId = 0)
        {
            // User's treatment
            UserService userService = new UserService(this._dbContext);
            UserDTO userDTO = new AutoFaker<UserDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(u => u.Name, faker => faker.Person.FullName)
                .RuleFor(u => u.Login, faker => faker.Person.UserName)
                .RuleFor(u => u.Email, faker => faker.Person.Email);
            LocationDTO locationDTOUser = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                .RuleFor(l => l.City, faker => faker.Address.City())
                .RuleFor(l => l.State, faker => faker.Address.State())
                .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                .RuleFor(l => l.Lng, faker => faker.Address.Longitude());
            userDTO.Location = locationDTOUser;
            UserDTO userDTOInserted = await userService.Add(userDTO);
            int userIdForMessage = userDTOInserted.UserId;

            // Deal's treatment
            int dealIdForMessage = dealId;
            if(dealId == 0)
            {
                DealService dealService = new DealService(this._dbContext);
                DealDTO dealDTO = new AutoFaker<DealDTO>(AutoBogusConfiguration.LOCATE);
                dealDTO.Type = DealType.Venda;
                dealDTO.UrgencyType = DealUrgencyType.Baixa;
                LocationDTO locationDTO = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                    .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                    .RuleFor(l => l.City, faker => faker.Address.City())
                    .RuleFor(l => l.State, faker => faker.Address.State())
                    .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                    .RuleFor(l => l.Lng, faker => faker.Address.Longitude());
                dealDTO.Location = locationDTO;
                DealDTO dealDTOInserted = await dealService.Add(dealDTO);
                dealIdForMessage = dealDTOInserted.DealId;
            }

            // Create Bid
            MessageDTO messageDTO = new AutoFaker<MessageDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(m => m.Title, faker => faker.Lorem.Lines(1))
                .RuleFor(m => m.Message, faker => faker.Lorem.Lines(4));
            messageDTO.UserId = userIdForMessage;
            messageDTO.DealId = dealIdForMessage;

            return messageDTO;
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            var messageInserted = await messageService.Add(messageDTO);

            // Assert
            Assert.NotNull(messageInserted);
            Assert.True(messageInserted.MessageId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindValidId_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            var messageInserted = await messageService.Add(messageDTO);
            var messageGet = await messageService.Get(messageInserted.MessageId);

            // Assert
            Assert.NotNull(messageGet);
            Assert.Equivalent(messageGet, messageInserted);
        }

        [Fact]
        public async void Get_FindInvalidId_ReturnNull()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);

            // Act
            var messageGet = await messageService.Get(90305);

            // Assert
            Assert.Null(messageGet);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            var messageInserted = await messageService.Add(messageDTO);
            var messageGet = await messageService.Get(messageInserted.MessageId);
            messageGet.Message = "Other message for this deal";
            var messagePut = await messageService.Update(messageGet);

            // Assert
            Assert.Equal("Other message for this deal", messagePut.Message);
        }

        [Fact]
        public async void Get_FindAllByDealAndValidDealId_ReturnListOfMessageDTO()
        {
            // Arrange
            var msgService = new MessageService(this._dbContext);
            MessageDTO msgDTO1 = await this.CreateMessageDTO();
            MessageDTO msgDTO2 = await this.CreateMessageDTO(msgDTO1.DealId);
            MessageDTO msgDTO3 = await this.CreateMessageDTO(msgDTO1.DealId);

            // Act
            await msgService.Add(msgDTO1);
            await msgService.Add(msgDTO2);
            await msgService.Add(msgDTO3);
            var msgGetAll = await msgService.GetAllByDeal(msgDTO1.DealId);

            // Assert
            Assert.NotNull(msgGetAll);
            Assert.Equal(3, msgGetAll.Count);
        }
    }
}