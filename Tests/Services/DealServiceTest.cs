using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Configuration;
using src.Domain.Enums;
using Xunit;

namespace Tests.Services
{
    public class DealServiceTest
    {
        private readonly MyDbContext _dbContext;

        public DealServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        /* Create a DealDTO */
        public async Task<DealDTO> CreateDealDTO()
        {
            // Create user on Database
            UserService userService = new UserService(this._dbContext);
            UserDTO userDTO = new AutoFaker<UserDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(u => u.Name, faker => faker.Person.FullName)
                .RuleFor(u => u.Login, faker => faker.Person.UserName)
                .RuleFor(u => u.Email, faker => faker.Person.Email);
            LocationDTO locationDTO = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                .RuleFor(l => l.City, faker => faker.Address.City())
                .RuleFor(l => l.State, faker => faker.Address.State())
                .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                .RuleFor(l => l.Lng, faker => faker.Address.Longitude());
            userDTO.Location = locationDTO;
            UserDTO userDTOInserted = await userService.Add(userDTO);

            // Create Deal for the User
            DealDTO dealDTO = new AutoFaker<DealDTO>(AutoBogusConfiguration.LOCATE);
            dealDTO.UserId = userDTOInserted.UserId;
            dealDTO.Type = DealType.Venda;
            dealDTO.UrgencyType = DealUrgencyType.Baixa;
            dealDTO.Location = locationDTO;

            return dealDTO;
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnDealDTO()
        {
            // Arrange
            var dealService = new DealService(this._dbContext);
            DealDTO dealDTO = await this.CreateDealDTO();

            // Act
            var dealInserted = await dealService.Add(dealDTO);

            // Assert
            Assert.True(dealInserted.DealId > 0 ? true : false);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnDealDTO()
        {
            // Arrange
            var dealService = new DealService(this._dbContext);
            DealDTO dealDTO = await this.CreateDealDTO();

            // Act
            var dealInserted = await dealService.Add(dealDTO);
            dealInserted.UrgencyType = DealUrgencyType.Alta;
            var dealUpdated = await dealService.Update(dealInserted);

            // Assert
            Assert.Equal(DealUrgencyType.Alta, dealUpdated.UrgencyType);
        }

        [Fact]
        public async void Get_FindValidId_ReturnDealDTO()
        {
            // Arrange
            var dealService = new DealService(this._dbContext);
            DealDTO dealDTO = await this.CreateDealDTO();

            // Act
            var dealInserted = await dealService.Add(dealDTO);
            var dealGet = await dealService.Get(dealInserted.DealId);

            // Assert
            Assert.NotNull(dealGet);
            Assert.Equivalent(dealInserted, dealGet);
        }
    }
}