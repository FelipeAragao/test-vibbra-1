using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Domain.Entities;
using src.Domain.Enums;
using src.Infrastructure.Db;
using Tests.Configuration;
using Xunit;

namespace Tests.Services
{
    public class BidServiceTest
    {
        private readonly MyDbContext _dbContext;

        public BidServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        /* Create a BidDTO with a user (created by the method or received) and a deal (created by the method or received) */
        public async Task<BidDTO> CreateBidDTO(int userIdForDeal = 0, int dealId = 0)
        {
            // Create location for user and deal (if necessary)
            LocationDTO locationDTO = new AutoFaker<LocationDTO>(AutoBogusConfiguration.LOCATE)
                .RuleFor(l => l.Address, faker => faker.Address.StreetAddress())
                .RuleFor(l => l.City, faker => faker.Address.City())
                .RuleFor(l => l.State, faker => faker.Address.State())
                .RuleFor(l => l.Lat, faker => faker.Address.Latitude())
                .RuleFor(l => l.Lng, faker => faker.Address.Longitude());

            // User's treatment
            int userIdForBid = userIdForDeal;
            if(userIdForDeal == 0)
            {
                UserService userService = new UserService(this._dbContext);
                UserDTO userDTO = new AutoFaker<UserDTO>(AutoBogusConfiguration.LOCATE)
                    .RuleFor(u => u.Name, faker => faker.Person.FullName)
                    .RuleFor(u => u.Login, faker => faker.Person.UserName)
                    .RuleFor(u => u.Email, faker => faker.Person.Email);
                userDTO.Location = locationDTO;
                UserDTO userDTOInserted = await userService.Add(userDTO);
                userIdForBid = userDTOInserted.UserId;
            }

            // Deal's treatment
            int dealIdForBid = dealId;
            if(dealId == 0)
            {
                DealService dealService = new DealService(this._dbContext);
                DealDTO dealDTO = new AutoFaker<DealDTO>(AutoBogusConfiguration.LOCATE);
                dealDTO.UserId = userIdForBid;
                dealDTO.Type = DealType.Venda;
                dealDTO.UrgencyType = DealUrgencyType.Baixa;
                dealDTO.Location = locationDTO;
                DealDTO dealDTOInserted = await dealService.Add(dealDTO);
                dealIdForBid = dealDTOInserted.DealId;
            }

            // Create Bid
            BidDTO bidDTO = new AutoFaker<BidDTO>(AutoBogusConfiguration.LOCATE);
            bidDTO.UserId = userIdForBid;
            bidDTO.DealId = dealIdForBid;

            return bidDTO;
        }

        [Fact]
        public async void Add_EnteringAllValidData_ReturnDealDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            var dealInserted = await bidService.Add(bidDTO);

            // Assert
            Assert.NotNull(dealInserted);
            Assert.True(dealInserted.DealId > 0 ? true : false);
        }

        [Fact]
        public async void Get_FindValidId_ReturnBidDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            var dealInserted = await bidService.Add(bidDTO);
            var dealGet = await bidService.Get(dealInserted.BidId);

            // Assert
            Assert.NotNull(dealGet);
            Assert.Equivalent(dealGet, dealInserted);
        }

        [Fact]
        public async void Get_FindInvalidId_ReturnNull()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);

            // Act
            var dealGet = await bidService.Get(80000);

            // Assert
            Assert.Null(dealGet);
        }

        [Fact]
        public async void Put_EnteringAllValidData_ReturnBidDTO()
        {
            // Arrange
            var bidService = new BidService(this._dbContext);
            BidDTO bidDTO = await this.CreateBidDTO();

            // Act
            var dealInserted = await bidService.Add(bidDTO);
            var dealGet = await bidService.Get(dealInserted.BidId);
            BidDTO dealPut;
            dealGet.Value = 150.15M;
            dealPut = await bidService.Update(dealGet);

            // Assert
            Assert.Equal(150.15M, dealPut.Value);
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
    }
}