using appData.Utils.Enitties;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appData.Utils.Enitty
{
    public interface ITokenService
    {
        string GenerateToken(AccountToken account);
        bool ValidateToken(string authToken);
        JwtSecurityToken ParseToken(string tokenString);
        string GenerateToken(AccountToken account, DateTime expiryTime);
    }
}
