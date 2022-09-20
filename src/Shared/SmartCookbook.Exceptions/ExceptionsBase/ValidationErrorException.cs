namespace SmartCookbook.Exceptions.ExceptionsBase;

public class ValidationErrorException : SmartCookbookException
{
    public List<string> ErrorMessages { get; set; }

    public ValidationErrorException(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

}
