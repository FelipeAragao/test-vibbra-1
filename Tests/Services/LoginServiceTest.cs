using src.Application.DTOs;
using src.Application.Services;
using src.Infrastructure.Db;
using Tests.Utilities;

namespace Tests.Services
{
    public class LoginServiceTest : IClassFixture<DbContextFixture>
    {
        private readonly MyDbContext _dbContext;

        public LoginServiceTest(DbContextFixture fixture)
        {
            this._dbContext = fixture.DbContext;
        }
        
        [Fact]
        public async void Login_WhenTryToLoginWithLoginAndPasswordCorrect_ReturnTheUser()
        {
            // Arrange
            var loginService = new UserService(this._dbContext);
            var loginDTO = new LoginDTO() {
                Login = "teste",
                Password = "123"
            };

            // Act
            var userDTO = await loginService.Login(loginDTO);

            // Assert
            Assert.NotNull(userDTO);
        }

        [Fact]
        public async void Login_WhenTryToLoginWithLoginAndPasswordCorrect_ReturnNull()
        {
            // Arrange
            var loginService = new UserService(this._dbContext);
            var loginDTO = new LoginDTO() {
                Login = "teste_erro",
                Password = "1234"
            };

            // Act
            var userDTO = await loginService.Login(loginDTO);

            // Assert
            Assert.Null(userDTO);
        }
    }
}