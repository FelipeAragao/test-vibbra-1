using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Application.DTOs;

namespace src.Application.Interfaces
{
    public interface ITokenService
    {
        Task<CorreiosTokenResponseDTO> GetNewTokenAsync();
    }
}