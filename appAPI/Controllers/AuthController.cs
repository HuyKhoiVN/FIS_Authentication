using appData.Models;
using appData.ModelsDTO;
using appData.Repository;
using appAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using appData.Utils.Enitties;

namespace appAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<Role> _roleRepository;

        public AuthController(IAccountService userService, IConfiguration configuration, IBaseRepository<Role> roleRepository)
        {
            _accountService = userService;
            _configuration = configuration;
            _roleRepository = roleRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Xác minh người dùng
            var user = await _accountService.ValidateUser(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Tạo JWT
            var token = await GenerateJwtToken(user);
            return Ok(new LoginResponse
            {
                Status = "200",
                Token = token
            }
                );
        }

        [HttpPost("loginService")]
        public async Task<IActionResult> LoginService([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _accountService.LoginService(request);
                if (response.status == "200")
                {
                    HttpContext.Response.Cookies.Append("Authorization", ((SignInResponse)response.resources).AccessToken);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                string errror = e.Message;
                return BadRequest();
            }
        }

        private async Task<string> GenerateJwtToken(Account user)
        {
            var positionName = (await _roleRepository.GetByIdAsync(user.RoleId)).RoleName;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, positionName)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
