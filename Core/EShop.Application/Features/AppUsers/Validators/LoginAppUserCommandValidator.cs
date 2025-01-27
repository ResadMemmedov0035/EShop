using EShop.Application.Features.AppUsers.Commands;
using FluentValidation;

namespace EShop.Application.Features.AppUsers.Validators;

public class LoginAppUserCommandValidator : AbstractValidator<LoginAppUserCommand>
{
    public LoginAppUserCommandValidator()
    {
        RuleFor(x => x.EmailOrUserName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
