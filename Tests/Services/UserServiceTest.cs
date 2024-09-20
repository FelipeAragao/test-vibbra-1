using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using src.Application.DTOs;
using src.Application.Services;
using src.Domain.Entities;
using src.Infrastructure.Db;
using Tests.Configuration;
using Xunit;

namespace Tests.Services
{
    public class UserServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public UserServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        public UserDTO CreateUserDTO()
        {
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

            return userDTO;
        }

        [Fact]
        public async void Login_WhenTryToLoginWithLoginAndPasswordCorrect_ReturnTheUser()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            var loginDTO = new LoginDTO() {
                Login = "teste",
                Password = "123"
            };

            // Act
            var userDTO = await userService.Login(loginDTO);

            // Assert
            Assert.NotNull(userDTO);
        }

        [Fact]
        public async void Login_WhenTryToLoginWithLoginAndPasswordCorrect_ReturnNull()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            var loginDTO = new LoginDTO() {
                Login = "teste_erro",
                Password = "1234"
            };

            // Act
            var userDTO = await userService.Login(loginDTO);

            // Assert
            Assert.Null(userDTO);
        }

        [Fact]
        public async void Add_EnteringAllValidDataAndLoginThatNotExists_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = this.CreateUserDTO();

            // Act
            var user = await userService.Add(userDTO);

            // Assert
            Assert.Equivalent(user, userDTO);
        }

        [Fact]
        public async void Add_EnteringAllValidDataAndLoginThatExists_ReturnNull()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = this.CreateUserDTO();
            userDTO.Login = "teste";

            // Act
            var user = await userService.Add(userDTO);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async void Add_EnteringAllValidDataWithoutLocation_ReturnNull()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = this.CreateUserDTO();
            userDTO.Location = null;

            // Act
            var user = await userService.Add(userDTO);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async void Get_FindValidId_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);

            // Act
            var userDTO = await userService.Get(1);

            // Assert
            Assert.NotNull(userDTO);
        }

        [Fact]
        public async void Get_FindInvalidId_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);

            // Act
            var userDTO = await userService.Get(10000);

            // Assert
            Assert.Null(userDTO);
        }

        [Fact]
        public async void Update_EnteringAllValidDataAndSameLogin_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = this.CreateUserDTO();

            // Act
            var userUpdate = await userService.Update(userDTO);

            // Assert
            Assert.Equivalent(userUpdate, userDTO);
        }

        [Fact]
        public async void Update_EnteringAllValidDataAndReplaceLogin_ReturnNull()
        {
            // Arrange
            var userService = new UserService(this._dbContext);

            // Act
            UserDTO userDTO = await userService.Get(1);
            userDTO.Login = "outro";
            UserDTO userUpdate = await userService.Update(userDTO);

            // Assert
            Assert.Null(userUpdate);
        }
    }
}