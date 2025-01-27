using EShop.Application.Features.AppUsers.Commands;
using EShop.Domain.Entities.Identity;

namespace EShop.Application.Mapping.Manual;

public static class AppUserMapper
{
    public static AppUser MapToAppUser(this RegisterAppUserCommand command)
    {
        return new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            UserName = command.UserName
        };
    }
}
