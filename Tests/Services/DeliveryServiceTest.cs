using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Utilities;
using Xunit;

namespace Tests.Services
{
    public class DeliveryServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public DeliveryServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        // A method to centralize and standardize the delivery's creation on test
        public async Task<DeliveryDTO> CreateDeliveryDTO()
        {
            // Create User for Delivery
            UserService userService = new UserService(this._dbContext);
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();
            await userService.Add(userDTO);
            // Create User for the Deal
            UserDTO userForDealDTO = RandomDataGenerator.GenerateUserDTO();
            await userService.Add(userForDealDTO);

            // Create the Deal
            DealService dealService = new DealService(this._dbContext);
            DealDTO dealDTO = RandomDataGenerator.GenerateDealDTO(userForDealDTO.UserId);
            DealDTO dealDTOInserted = await dealService.Add(dealDTO);

            // Return DealDTO
            return RandomDataGenerator.GenerateDeliveryDTO(dealDTOInserted.DealId, userDTO.UserId);
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnDeliveryDTO()
        {
            // Arrange
            var deliveryService = new DeliveryService(this._dbContext);
            DeliveryDTO deliveryDTO = await CreateDeliveryDTO();

            // Act
            var deliveryAdded = await deliveryService.Add(deliveryDTO.DealId, deliveryDTO.UserId);

            // Assert
            Assert.NotNull(deliveryAdded);
            Assert.True(deliveryAdded.DeliveryId > 0 ? true : false);
        }

        [Fact]
        public async void Add_EnteringInvalidDeal_ThrowException()
        {
            // Arrange
            var deliveryService = new DeliveryService(this._dbContext);
            DeliveryDTO deliveryDTO = await CreateDeliveryDTO();
            deliveryDTO.DealId = 237903593;

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await deliveryService.Add(deliveryDTO.DealId, deliveryDTO.UserId)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Deal not found.", exception.Message);
        }

        [Fact]
        public async void Add_EnteringInvalidUser_ThrowException()
        {
            // Arrange
            var deliveryService = new DeliveryService(this._dbContext);
            DeliveryDTO deliveryDTO = await CreateDeliveryDTO();
            deliveryDTO.UserId = 237903593;

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await deliveryService.Add(deliveryDTO.DealId, deliveryDTO.UserId)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: User not found.", exception.Message);
        }

        [Fact]
        public async void Get_EnteringValidDealId_ReturnDeliveryDTO()
        {
            // Arrange
            var deliveryService = new DeliveryService(this._dbContext);
            DeliveryDTO deliveryDTO = await CreateDeliveryDTO();
            var deliveryAdded = await deliveryService.Add(deliveryDTO.DealId, deliveryDTO.UserId);

            // Act
            var deliveryGet = await deliveryService.Get(deliveryAdded.DealId);

            // Assert
            Assert.NotNull(deliveryGet);
            Assert.Equal(deliveryGet.DealId, deliveryAdded.DealId);
        }

        [Fact]
        public async void Get_EnteringInvalidDealId_ThrowException()
        {
            // Arrange
            var deliveryService = new DeliveryService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await deliveryService.Get(340956734)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Delivery not found. Check the data.", exception.Message);
        }
    }
}