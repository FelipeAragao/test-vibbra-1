using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using src.Application.Interfaces;
using src.Configurations;

namespace src.Application.Services
{
    public class CorreiosTokenManager
    {
        private readonly ITokenService _tokenService;
        private readonly CorreiosSettings _correiosSettings;
        private readonly IMemoryCache _memoryCache;

        public CorreiosTokenManager(ITokenService tokenService, IMemoryCache memoryCache, IOptions<CorreiosSettings> correiosSettings)
        {
            this._tokenService = tokenService;
            this._memoryCache = memoryCache;
            this._correiosSettings = correiosSettings.Value;
        }

        public async Task<string> GetTokenAsync()
        {
            // Check if exists any token in cache
            if (_memoryCache.TryGetValue("correios_access_token", out TokenCacheItem? tokenCacheItem))
            {
                // 5 minutos to expiry
                if (tokenCacheItem != null && tokenCacheItem.Token != null && DateTime.UtcNow < tokenCacheItem.Expiry.AddMinutes(-5))
                {
                    return tokenCacheItem.Token;
                }
            }

            // Get new token
            var newToken = await _tokenService.GetNewTokenAsync();
            var expiry = newToken.expiraEm;

            // Armazena o novo token no cache
            var cacheItem = new TokenCacheItem { Token = newToken.token, Expiry = expiry };
            this._memoryCache.Set("correios_access_token", cacheItem, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return newToken.token;
        }
    }

    public class TokenCacheItem
    {
        public string? Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}