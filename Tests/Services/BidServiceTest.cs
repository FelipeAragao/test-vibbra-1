using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Domain.Entities;
using src.Domain.Enums;
using src.Infrastructure.Db;
using Tests.Configuration;
using Tests.Utilities;
using Xunit;

namespace Tests.Services
{
    public class BidServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public BidServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        // A method to centralize and standardize the bid's creation on test
        public async Task<BidDTO> CreateBidDTO(int userIdForDeal = 0, int dealId = 0)
        {
            UserService userService = new UserService(this._dbContext);

            // Deal's user treatment
            if(userIdForDeal == 0)
            {
                UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();
                await userService.Add(userDTO);
                userIdForDeal = userDTO.UserId;
            }

            // Deal's treatment
            int dealIdForBid = dealId;
            if(dealId == 0)
            {
                DealService dealService = new DealService(this._dbContext);
                DealDTO dealDTO = RandomDataGenerator.GenerateDealDTO(userIdForDeal);
                DealDTO dealDTOInserted = await dealService.Add(dealDTO);
                dealIdForBid = dealDTOInserted.DealId;
            }

            // Bid's user treatment
            UserDTO bidUserDTO = RandomDataGenerator.GenerateUserDTO();
            await userService.Add(bidUserDTO);
            int userIdForBid = bidUserDTO.UserId;

            // Create Bid
            return RandomDataGenerator.GenerateBidDTO(userIdForBid, dealIdForBid);
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnDealDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            await bidService.Add(bidDTO);

            // Assert
            Assert.NotNull(bidDTO);
            Assert.True(bidDTO.DealId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindValidId_ReturnBidDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            var dealInserted = await bidService.Add(bidDTO);
            var dealGet = await bidService.Get(dealInserted.DealId, dealInserted.BidId);

            // Assert
            Assert.NotNull(dealGet);
            Assert.Equivalent(dealGet, dealInserted);
        }

        [Fact]
        public async void Get_FindInvalidId_ThrowException()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await bidService.Get(20, 80000)
            );
            
            // Assert
            Assert.Equal("An unexpected error occurred: Bid not found or Deal not correspond to bid. Check the data.", exception.Message);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnBidDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            await bidService.Add(bidDTO);
            bidDTO.Value = 150.15M;
            await bidService.Update(bidDTO);
            var dealUpdated = await bidService.Get(bidDTO.DealId, bidDTO.BidId);

            // Assert
            Assert.NotNull(dealUpdated);
            Assert.Equal(150.15M, dealUpdated.Value);
        }

        [Fact]
        public async void Get_FindAllByDealAndValidDealId_ReturnListOfBidDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO1 = await this.CreateBidDTO();
            BidDTO bidDTO2 = await this.CreateBidDTO(bidDTO1.UserId, bidDTO1.DealId);
            BidDTO bidDTO3 = await this.CreateBidDTO(bidDTO1.UserId, bidDTO1.DealId);

            // Act
            await bidService.Add(bidDTO1);
            await bidService.Add(bidDTO2);
            await bidService.Add(bidDTO3);
            var dealGetAll = await bidService.GetAllByDeal(bidDTO1.DealId);

            // Assert
            Assert.NotNull(dealGetAll);
            Assert.Equal(3, dealGetAll.Count);
        }

        [Fact]
        public async void Get_FindAllByDealAndInvalidDealId_ThrowException()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await bidService.GetAllByDeal(2343634)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Bids for deal not found", exception.Message);
        }
    }
}