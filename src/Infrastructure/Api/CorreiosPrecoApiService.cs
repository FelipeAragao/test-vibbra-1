
using Microsoft.Extensions.Options;
using src.Application.DTOs;
using src.Application.Interfaces;
using src.Configurations;

namespace src.Infrastructure.Api
{
    public class CorreiosPrecoApiService : ICorreiosPrecoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly CorreiosSettings _correiosSettings;

        public CorreiosPrecoApiService(HttpClient httpClient, IOptions<CorreiosSettings> correiosSettings)
        {
            this._httpClient = httpClient;
            this._correiosSettings = correiosSettings.Value;
        }

        public async Task<CorreiosPrecoResponseDTO> GetPrecoAsync(CorreiosPrecoRequestDTO request)
        {
            try {
                var url = $"{ this._correiosSettings.ApiUrl + this._correiosSettings.RoutesPrecoNacional }?" +
                        $"cepDestino={ request.CepDestino }" +
                        $"&cepOrigem={ request.CepOrigem }" +
                        $"&psObjeto={ request.PsObjeto }" +
                        $"&tpObjeto={ request.TpObjeto }" +
                        $"&comprimento={ request.Comprimento }" +
                        $"&largura={ request.Largura }" +
                        $"&altura={ request.Altura }";
                        
                if (request.ServicosAdicionais != null)
                {
                    foreach (var servico in request.ServicosAdicionais)
                    {
                        url += $"&servicosAdicionais={ servico }";
                    }
                }

                url += $"&vlDeclarado={ request.VlDeclarado }";

                var response = await this._httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CorreiosPrecoResponseDTO>() ??
                        throw new Exception("No results on response of Correios Preços");
                }
                else
                    throw new Exception("Error on Correios Api request. Status Code: " + response.StatusCode);
            }
            catch (Exception ex) {
                throw new Exception("Error on Correios Api request. " + ex.Message);
            }
        }
    }
}