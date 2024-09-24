
using AutoBogus;
using Microsoft.AspNetCore.Identity;
using src.Application.DTOs;
using src.Application.Mappers;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Configuration;
using Tests.Utilities;

namespace Tests.Services
{
    public class InviteServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public InviteServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        /* Create a InviteDTO */
        public async Task<InviteDTO> CreateInviteDTO(int userInvite = 0)
        {
            // User 1: Invite
            UserService userService = new UserService(this._dbContext);
            int userIdInvite = userInvite;
            if(userInvite == 0)
            {
                UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();
                UserDTO userDTOInserted = await userService.Add(userDTO);
                userIdInvite = userDTOInserted.UserId;
            }

            // User 2: Invited
            UserDTO userInvitedDTO = RandomDataGenerator.GenerateUserDTO();
            UserDTO userInvitedDTOInserted = await userService.Add(userInvitedDTO);

            // Create Invite
            return RandomDataGenerator.GenerateInviteDTO(userIdInvite, userInvitedDTOInserted.UserId);
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnInviteDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            await inviteService.Add(inviteDTO);

            // Assert
            Assert.True(inviteDTO.InviteId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindInviteWithValidId_ReturnInviteDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            await inviteService.Add(inviteDTO);
            var inviteGet = await inviteService.Get(inviteDTO.UserId, inviteDTO.InviteId);

            // Assert
            Assert.NotNull(inviteGet);
            Assert.Equal(inviteGet.UserId, inviteDTO.UserId);
            Assert.Equal(inviteGet.UserInvitedId, inviteDTO.UserInvitedId);
            Assert.Equal(inviteGet.Name, inviteDTO.Name);
            Assert.Equal(inviteGet.Email, inviteDTO.Email);
        }

        [Fact]
        public async void Get_FindInviteWithInvalidId_ReturnNull()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await inviteService.Get(2345, 40234)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Invite not found or user not correspond to invite. Check the data.", exception.Message);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnBidDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            var inviteInserted = await inviteService.Add(inviteDTO);
            var inviteGet = await inviteService.Get(inviteInserted.UserId, inviteInserted.InviteId);
            inviteGet.Email = "outro@email.com";
            var invitePut = await inviteService.Update(inviteGet);

            // Assert
            Assert.Equal("outro@email.com", invitePut.Email);
        }

        [Fact]
        public async void Get_FindAllInvitesByUser_ReturnListOfInviteDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO1 = await this.CreateInviteDTO();
            InviteDTO inviteDTO2 = await this.CreateInviteDTO(inviteDTO1.UserId);
            InviteDTO inviteDTO3 = await this.CreateInviteDTO(inviteDTO1.UserId);

            // Act
            await inviteService.Add(inviteDTO1);
            await inviteService.Add(inviteDTO2);
            await inviteService.Add(inviteDTO3);
            var inviteGetAll = await inviteService.GetAllByUser(inviteDTO1.UserId);

            // Assert
            Assert.NotNull(inviteGetAll);
            Assert.Equal(3, inviteGetAll.Count);
        }

        [Fact]
        public async void Get_FindAllInvitesByInvalidUser_ThrowException()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await inviteService.GetAllByUser(465438)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Invite not found or user not correspond to invite. Check the data.", exception.Message);
        }
    }
}