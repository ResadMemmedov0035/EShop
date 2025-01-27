namespace EShop.Application.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SecuredOperationAttribute : Attribute
{
    public SecuredOperationAttribute(params string[] roles)
    {
        Roles = roles;
    }

    public string[] Roles { get; }
}
