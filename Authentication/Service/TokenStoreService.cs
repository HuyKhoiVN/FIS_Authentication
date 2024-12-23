using Microsoft.Extensions.Caching.Memory;

namespace Authentication.Service
{
    public class TokenStoreService
    {
        private readonly IMemoryCache _memoryCache;

        public TokenStoreService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void StoreToken(string token)
        {
            var cacheKey = $"Token_";
            _memoryCache.Set(cacheKey, token, TimeSpan.FromHours(1)); // Token hết hạn sau 1 giờ
        }

        public string? GetToken()
        {
            var cacheKey = $"Token_";
            _memoryCache.TryGetValue(cacheKey, out string? token);
            return token;
        }
    }

}
