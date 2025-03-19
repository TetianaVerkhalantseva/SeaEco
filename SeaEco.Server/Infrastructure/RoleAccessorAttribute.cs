namespace SeaEco.Server.Infrastructure;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class RoleAccessorAttribute : Attribute
{
    public bool IsAdmin { get; private set; }

    public RoleAccessorAttribute(bool isAdmin)
    {
        IsAdmin = isAdmin;
    }
}