using Authentication.Models;
using Authentication.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenStoreService _tokenStore;

        public TokenController(TokenStoreService tokenStore)
        {
            _tokenStore = tokenStore;
        }

        [HttpPost("store")]
        public IActionResult StoreToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Invalid token or userId.");
            }

            _tokenStore.StoreToken(request.UserId, request.Token);
            return Ok();
        }
    }

}
