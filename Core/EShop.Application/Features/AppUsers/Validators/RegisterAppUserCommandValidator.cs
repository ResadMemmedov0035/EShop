using EShop.Application.Features.AppUsers.Commands;
using FluentValidation;

namespace EShop.Application.Features.AppUsers.Validators;

public class RegisterAppUserCommandValidator : AbstractValidator<RegisterAppUserCommand>
{
    public RegisterAppUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(25);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(25);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(25);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        // Identity has validator for passwords. See: PersistenceServiceRegitration
        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty()
            .Must((command, passwordConfirm) => command.Password == passwordConfirm)
                .WithMessage("Confirm password must be match with password.");
    }
}
