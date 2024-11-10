using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList.Core.Common.IOptions;
using TodoList.Core.Models;

namespace TodoList.BusinessLogic.Services;

public class TokenService(IOptions<JwtSettings> jwtSettings)
{
    private JwtSettings _jwtSettings = jwtSettings.Value;

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var now = DateTime.Now;

        var tokenDescriptor = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            now,
            now.AddMinutes(_jwtSettings.ExpirationInMinutes),
            credentials);

        var token = tokenHandler.WriteToken(tokenDescriptor);
        return token;
    }
}
