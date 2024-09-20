
using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Configuration;

namespace Tests.Services
{
    public class InviteServiceTest
    {
        private readonly MyDbContext _dbContext;

        public InviteServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        /* Create a InviteDTO 2 new users */
        public async Task<InviteDTO> CreateInviteDTO(int userInvite = 0)
        {
            // Users' treatment
            UserService userService = new UserService(this._dbContext);
            int userIdInvite = userInvite;
            if(userInvite == 0)
            {
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
                userIdInvite = userDTOInserted.UserId;
            }

            UserDTO userInvitedDTO = new AutoFaker<UserDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(u => u.Name, faker => faker.Person.FullName)
                .RuleFor(u => u.Login, faker => faker.Person.UserName)
                .RuleFor(u => u.Email, faker => faker.Person.Email);
            LocationDTO locationDTOUserInvited = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                .RuleFor(l => l.City, faker => faker.Address.City())
                .RuleFor(l => l.State, faker => faker.Address.State())
                .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                .RuleFor(l => l.Lng, faker => faker.Address.Longitude());
            userInvitedDTO.Location = locationDTOUserInvited;
            UserDTO userInvitedDTOInserted = await userService.Add(userInvitedDTO);

            // Create Invite
            InviteDTO inviteDTO = new AutoFaker<InviteDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(m => m.Name, faker => faker.Person.FullName)
                .RuleFor(m => m.Email, faker => faker.Person.Email);
            inviteDTO.UserId = userIdInvite;
            inviteDTO.UserInvitedId = userInvitedDTOInserted.UserId;

            return inviteDTO;
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnInviteDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            var inviteInserted = await inviteService.Add(inviteDTO);

            // Assert
            Assert.NotNull(inviteInserted);
            Assert.True(inviteInserted.InviteId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindInviteWithValidId_ReturnInviteDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            var inviteInserted = await inviteService.Add(inviteDTO);
            var inviteGet = await inviteService.Get(inviteInserted.InviteId);

            // Assert
            Assert.NotNull(inviteGet);
            Assert.Equivalent(inviteGet, inviteInserted);
        }

        [Fact]
        public async void Get_FindInviteWithInvalidId_ReturnNull()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);

            // Act
            var inviteGet = await inviteService.Get(40234);

            // Assert
            Assert.Null(inviteGet);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnBidDTO()
        {
            // Arrange
            var inviteService = new InviteService(this._dbContext);
            InviteDTO inviteDTO = await this.CreateInviteDTO();

            // Act
            var inviteInserted = await inviteService.Add(inviteDTO);
            var inviteGet = await inviteService.Get(inviteInserted.InviteId);
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
    }
}