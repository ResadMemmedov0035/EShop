using System.Security.Claims;

namespace EShop.Infrastructure.Security
{
    public static class ClaimsExtensions
    {
        public static string GetEmail(this IEnumerable<Claim> claims)
            => claims.First(c => c.Type == ClaimTypes.Email).Value;

        public static string GetId(this IEnumerable<Claim> claims)
            => claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }
}
