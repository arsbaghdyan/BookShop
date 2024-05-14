namespace BookShop.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ExcludeFromClientContextMiddleware : Attribute
{

}
