using EShop.Domain.Exceptions;

namespace EShop.Application.Rules;

// TODO: Rules actually could be guards.
// and business rules and logics could be in handle method directly
public static class CommonRules
{
    public static T IfNullThrowNotFound<T>(this T? item, string? name = null)
    {
        if (item is null)
            throw new NotFoundException($"{name ?? typeof(T).Name} was not found.");
        return item;
    }
}
