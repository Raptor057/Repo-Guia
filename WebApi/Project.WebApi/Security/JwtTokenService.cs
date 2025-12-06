using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Project.WebApi.Security
{
    public sealed class JwtTokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public JwtTokenService(IConfiguration configuration)
        {
            _issuer = configuration["Jwt:Issuer"] ?? string.Empty;
            _audience = configuration["Jwt:Audience"] ?? string.Empty;
            _key = configuration["Jwt:Key"] ?? string.Empty;
        }

        public string GenerateToken(long userId, string userName, string roleName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
