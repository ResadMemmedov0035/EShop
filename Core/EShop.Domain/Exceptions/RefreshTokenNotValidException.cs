namespace EShop.Domain.Exceptions;

public class RefreshTokenNotValidException : AuthenticationException
{
    public RefreshTokenNotValidException() : base("Refresh token is not valid or expired.")
    {
    }

    public RefreshTokenNotValidException(string message) : base(message)
    {
    }
}
