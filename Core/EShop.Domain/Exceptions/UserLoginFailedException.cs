namespace EShop.Domain.Exceptions;

public class UserLoginFailedException : AuthenticationException
{
    public UserLoginFailedException() : base("The username/email or password is wrong.")
    { }

    public UserLoginFailedException(string message) : base(message)
    { }
}
