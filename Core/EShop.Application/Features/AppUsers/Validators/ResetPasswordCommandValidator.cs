using EShop.Application.Features.AppUsers.Commands;
using FluentValidation;

namespace EShop.Application.Features.AppUsers.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.NewPasswordConfirm)
            .Must(BeMatchWithPassword)
            .WithMessage("Confirm password must be match with the password");
    }

    private bool BeMatchWithPassword(ResetPasswordCommand command, string arg)
    {
        return command.NewPassword == arg;
    }
}
