namespace EShop.Domain.Exceptions;

public class UserRegistrationFailedException : AuthenticationException
{
    public UserRegistrationFailedException(string message) : base(message)
    { }
}
