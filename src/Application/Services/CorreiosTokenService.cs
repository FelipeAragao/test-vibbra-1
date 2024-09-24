using Microsoft.Extensions.Options;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Configurations;

namespace src.Application.Services
{
    public class CorreiosTokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CorreiosSettings _correiosSettings;

        public CorreiosTokenService(IHttpClientFactory httpClientFactory, IOptions<CorreiosSettings> correiosSettings)
        {
            this._httpClientFactory = httpClientFactory;
            this._correiosSettings = correiosSettings.Value;
        }

        public async Task<CorreiosTokenResponseDTO> GetNewTokenAsync()
        {
            // Get new token
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(this._correiosSettings.ApiUrl + this._correiosSettings.RoutesAutenticaCartaoPostagem, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve token");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<CorreiosTokenResponseDTO>();
            return tokenResponse ?? throw new Exception("Invalid token response");
        }
    }
}