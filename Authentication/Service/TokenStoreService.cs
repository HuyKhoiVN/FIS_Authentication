using Microsoft.Extensions.Caching.Memory;

namespace Authentication.Service
{
    public class TokenService
    {
        private readonly IMemoryCache _memoryCache;

        public TokenService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SaveToken(string token)
        {
            // Lưu token vào MemoryCache với thời gian hết hạn (ví dụ: 30 phút)
            _memoryCache.Set(token, true, TimeSpan.FromMinutes(30));
        }

        public bool ValidateToken(string token)
        {
            // Kiểm tra token có trong MemoryCache hay không
            return _memoryCache.TryGetValue(token, out _);
        }
    }
}
