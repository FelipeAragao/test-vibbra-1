using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Domain.Enums;
using src.Infrastructure.Db;
using Tests.Configuration;
using Tests.Utilities;

namespace Tests.Services
{
    public class MessageServiceTest : IClassFixture<DbContextFixture>
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
            UserDTO messageUserDTO = RandomDataGenerator.GenerateUserDTO();
            await userService.Add(messageUserDTO);

            UserDTO dealUserDTO = RandomDataGenerator.GenerateUserDTO();
            await userService.Add(dealUserDTO);

            // Deal's treatment
            int dealIdForMessage = dealId;
            if(dealId == 0)
            {
                DealService dealService = new DealService(this._dbContext);
                DealDTO dealDTO = RandomDataGenerator.GenerateDealDTO(dealUserDTO.UserId);
                await dealService.Add(dealDTO);
                dealIdForMessage = dealDTO.DealId;
            }

            // Create Message
            return RandomDataGenerator.GenerateMessageDTO(messageUserDTO.UserId, dealIdForMessage);
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            await messageService.Add(messageDTO);

            // Assert
            Assert.True(messageDTO.MessageId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindValidId_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            await messageService.Add(messageDTO);
            var messageGet = await messageService.Get(messageDTO.DealId, messageDTO.MessageId);

            // Assert
            Assert.NotNull(messageGet);
            Assert.Equivalent(messageGet, messageDTO);
        }

        [Fact]
        public async void Get_FindInvalidId_ThrowException()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await messageService.Get(235, 56776788)
            );
            

            // Assert
            Assert.Equal("An unexpected error occurred: Message not found or Deal not correspond to message. Check the data.", exception.Message);
        }

        [Fact]
        public async void Update_EnteringAllValidData_ReturnMessageDTO()
        {
            // Arrange
            var messageService = new MessageService(this._dbContext);
            MessageDTO messageDTO = await this.CreateMessageDTO();

            // Act
            await messageService.Add(messageDTO);
            messageDTO.Message = "Other message for this deal";
            await messageService.Update(messageDTO);
            var messageGet = await messageService.Get(messageDTO.DealId, messageDTO.MessageId);

            // Assert
            Assert.Equal("Other message for this deal", messageGet.Message);
            Assert.Equivalent(messageGet, messageDTO);
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

        [Fact]
        public async void Get_FindAllByDealAndInvalidDealId_ThrowException()
        {
            // Arrange
            var MessageService = new MessageService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await MessageService.GetAllByDeal(86858586)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Messages for deal not found", exception.Message);
        }
    }
}