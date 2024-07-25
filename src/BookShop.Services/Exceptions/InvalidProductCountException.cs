namespace BookShop.Services.Exceptions;

public class InvalidProductCountException : Exception
{
    public InvalidProductCountException()
    {
    }

    public InvalidProductCountException(string message) : base(message)
    {
    }

    public InvalidProductCountException(string message, Exception ex) : base(message, ex)
    {
    }
}
