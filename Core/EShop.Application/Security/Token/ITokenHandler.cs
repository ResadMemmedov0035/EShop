using EShop.Domain.Entities.Identity;

namespace EShop.Application.Security.Token;

public interface ITokenHandler
{
    AccessToken CreateAccessToken(AppUser user, TimeSpan? lifetime = null);
    RefreshToken CreateRefreshToken();
}
