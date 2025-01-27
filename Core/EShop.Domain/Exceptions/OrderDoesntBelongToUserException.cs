namespace EShop.Domain.Exceptions;

public class OrderDoesntBelongToUserException : AuthorizationException
{
    public OrderDoesntBelongToUserException() : base("The order doesn't belong to the specified user.")
    { }
}
