using EShop.Domain.Entities.Identity;
using Identity = Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EShop.Application.Security.Token;

namespace EShop.Infrastructure.Security
{
    public class JsonWebTokenHandler : ITokenHandler
    {
        private readonly TokenOptions _tokenOptions;
        private readonly Identity.UserManager<AppUser> _userManager;

        public JsonWebTokenHandler(IOptions<TokenOptions> tokenOptions, Identity.UserManager<AppUser> userManager)
        {
            _tokenOptions = tokenOptions.Value;
            _userManager = userManager;
        }

        public AccessToken CreateAccessToken(AppUser user, TimeSpan? lifetime = null)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromMinutes(_tokenOptions.Lifetime)),
                signingCredentials: signingCredentials,
                claims: GetUserClaims(user));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new(token, jwt.ValidTo);
        }

        public RefreshToken CreateRefreshToken()
        {
            byte[] data = new byte[64];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            return new(Convert.ToBase64String(data), DateTime.UtcNow.AddMinutes(_tokenOptions.Lifetime * 5));
        }

        private IEnumerable<Claim> GetUserClaims(AppUser user)
        {
            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = _userManager.GetRolesAsync(user).Result.Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roleClaims);

            return claims;
        }
    }
}
