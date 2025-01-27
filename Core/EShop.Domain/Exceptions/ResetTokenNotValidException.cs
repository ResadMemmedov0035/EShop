namespace EShop.Domain.Exceptions;

public class ResetTokenNotValidException : AuthenticationException
{
    public ResetTokenNotValidException() : base("Refresh token is not valid or expired.")
    {
    }

    public ResetTokenNotValidException(string message) : base(message)
    {
    }
}
