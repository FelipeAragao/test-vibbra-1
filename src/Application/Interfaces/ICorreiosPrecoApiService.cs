using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface ICorreiosPrecoApiService
    {
        Task<CorreiosPrecoResponseDTO> GetPrecoAsync(CorreiosPrecoRequestDTO request);
    }
}