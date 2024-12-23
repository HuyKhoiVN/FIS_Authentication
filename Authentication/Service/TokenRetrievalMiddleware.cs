namespace Authentication.Service
{
    public class TokenRetrievalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenStoreService _tokenStore;

        public TokenRetrievalMiddleware(RequestDelegate next, TokenStoreService tokenStore)
        {
            _next = next;
            _tokenStore = tokenStore;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = _tokenStore.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);
        }
    }

}
