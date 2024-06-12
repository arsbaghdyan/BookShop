namespace BookShop.Services.Exceptions;

public class NotEnoughProductException : Exception
{
    public NotEnoughProductException()
    {
    }

    public NotEnoughProductException(string message) : base(message)
    {
    }

    public NotEnoughProductException(string message, Exception ex) : base(message, ex)
    {
    }
}
