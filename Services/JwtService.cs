using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TestApps.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _expDate;

        public JwtService(IConfiguration config)
        {
            _secret = config["Jwt:Secret"];
            _expDate = config["Jwt:ExpirationInMinutes"];
        }

        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var notBefore = DateTime.UtcNow.AddSeconds(-5);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_expDate)); // still uses UtcNow + X minutes

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                NotBefore = notBefore,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
