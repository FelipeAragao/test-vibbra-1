using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AutoBogus;
using EcommerceVibbra;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using src.Application.DTOs;
using src.Domain.Entities;
using Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Controllers
{
    public class AuthenticateControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _output;
        protected readonly HttpClient _httpClient;
        protected UserDTO _signupUserDTO;
        protected string _password = "123";
        protected TokenDTO? _token;

        public async Task DisposeAsync()
        {
            this._httpClient.Dispose();
        }

        public async Task InitializeAsync()
        {
            await this.Login_AddingValidUsernameAndPassword_ReturnSuccess();
            //await this.Add_RegisterNewValidUsernameAndPassword_ReturnSuccess();
        }

        public AuthenticateControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this._factory = factory;
            this._output = output;
            this._httpClient = this._factory.CreateClient();
            this._signupUserDTO = RandomDataGenerator.GenerateUserDTO();
        }

        public async Task Add_RegisterNewValidUsernameAndPassword_ReturnSuccess()
        {
            // Arrange
            this._signupUserDTO.Password = this._password;
            StringContent content = new StringContent(JsonConvert.SerializeObject(this._signupUserDTO), Encoding.UTF8, "application/json");

            // Act
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._token == null ? null : this._token.Token);
            var httpClientRequest = await this._httpClient.PostAsync("api/v1/users", content);

            // Assert
            this._output.WriteLine($"{nameof(AuthenticateControllerTests)}_{nameof(Add_RegisterNewValidUsernameAndPassword_ReturnSuccess)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

        public async Task Login_AddingValidUsernameAndPassword_ReturnSuccess()
        {
            // Arrange
            var loginDTO = new LoginDTO() {
                Login = "teste",
                Password = "123"
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");

            // Act
            var httpClientRequest = await this._httpClient.PostAsync("api/v1/authenticate", content);
            this._token = JsonConvert.DeserializeObject<TokenDTO>(await httpClientRequest.Content.ReadAsStringAsync());

            // Assert
            _output.WriteLine($"{nameof(AuthenticateControllerTests)}_{nameof(Login_AddingValidUsernameAndPassword_ReturnSuccess)} = {await httpClientRequest.Content.ReadAsStringAsync()} : {this._token}");
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(this._token);
        }
    }
}