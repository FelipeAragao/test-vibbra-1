using src.Application.DTOs;
using src.Application.Services;
using src.Infrastructure.Db;
using src.Domain.Enums;
using Tests.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Services
{
    public class DealServiceTest : IClassFixture<DbContextFixture>
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
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();
            UserDTO userDTOInserted = await userService.Add(userDTO);

            // Return DealDTO
            return RandomDataGenerator.GenerateDealDTO(userDTOInserted.UserId);
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
            await dealService.Update(dealInserted);
            var dealUpdated = await dealService.Get(dealInserted.DealId);

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

        [Fact]
        public async void Get_FindInvalidId_ThrowException()
        {
            // Arrange
            var dealService = new DealService(this._dbContext);
            DealDTO dealDTO = await this.CreateDealDTO();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await dealService.Get(76585604)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Deal not found", exception.Message);
        }
    }
}