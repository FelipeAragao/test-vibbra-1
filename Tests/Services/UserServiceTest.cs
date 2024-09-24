using src.Application.DTOs;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Utilities;

namespace Tests.Services
{
    public class UserServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public UserServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }

        [Fact]
        public async void Add_EnteringAllValidDataAndLoginThatNotExists_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();

            // Act
            var user = await userService.Add(userDTO);

            // Assert
            Assert.True(user.UserId > 0);
        }

        [Fact]
        public async void Add_EnteringAllValidDataAndLoginThatExists_ThrowException()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();
            userDTO.Login = "teste";

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () => 
            {
                await userService.Add(userDTO);
            });

            // Assert
            Assert.Equal("An unexpected error occurred: Login already exists", exception.Message);
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
        public async void Get_FindInvalidId_ThrowException()
        {
            // Arrange
            var userService = new UserService(this._dbContext);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () => 
            {
                await userService.Get(85045);
            });

            // Assert
            Assert.Equal("An unexpected error occurred: User not found", exception.Message);
        }

        [Fact]
        public async void Update_EnteringAllValidDataAndSameLogin_ReturnUserDTO()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();

            // Act
            await userService.Add(userDTO);
            userDTO.Email = "another@email.com";
            userDTO.Name = "New Name";
            await userService.Update(userDTO);
            var updatedUser = await userService.Get(userDTO.UserId);

            // Assert
            Assert.Equal("another@email.com", updatedUser.Email);
            Assert.Equal("New Name", updatedUser.Name);
        }

        [Fact]
        public async void Update_EnteringAllValidDataAndReplacingLogin_ThrowException()
        {
            // Arrange
            var userService = new UserService(this._dbContext);
            UserDTO userDTO = RandomDataGenerator.GenerateUserDTO();

            // Act
            await userService.Add(userDTO);
            userDTO.Login = "another.login";
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await userService.Update(userDTO)
            );

            // Assert
            Assert.Equal("An unexpected error occurred: Login can't be changed", exception.Message);
        }
    }
}