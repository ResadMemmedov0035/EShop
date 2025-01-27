namespace EShop.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    { }

    public static T ThrowIfNull<T>(T? arg, string? name = null)
    {
        if (arg is null)
            throw new NotFoundException($"{name ?? typeof(T).Name} was not found.");
        return arg;
    }
}
