namespace SmartCookbook.Exceptions.ExceptionsBase;

public class InvalidLoginException : SmartCookbookException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {

    }
}
