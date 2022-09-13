using UntoMeWorld.Domain.Errors;

namespace UntoMeWorld.Application.Errors;

public abstract class InvalidApiRequestRequestException : UserErrorException
{
    protected InvalidApiRequestRequestException()
    {
        Cause = "Your request has an invalid format or missing parameters";
        Fix = "Please check the error messages provided and try again";
    }

    protected InvalidApiRequestRequestException(string message):base(message)
    {
        Cause = "Your request has an invalid format or missing parameters";
        Fix = "Please check the error messages provided and try again";
    }
    
}

public class InvalidRequestException : InvalidApiRequestRequestException
{
    public InvalidRequestException() : base()
    {
        
    }

    public InvalidRequestException(string message): base(message)
    {
        
    }
}

public class InvalidSortByProperty : InvalidApiRequestRequestException
{
    public override string Message => "The name of the property to order by is invalid";
}
public class InvalidQueryLengthException : InvalidApiRequestRequestException
{
    public override string Message => "The query string is too short, min query length is 3";
}
public class InvalidPageNumberException : InvalidApiRequestRequestException
{
    public override string Message => "The page number cannot be smaller than 1";
}
public class MissingParametersException : InvalidApiRequestRequestException
{
    public override string Message => "The page number cannot be smaller than 0";
}

public class InvalidParameterError : InvalidApiRequestRequestException
{
    public InvalidParameterError(string parameter): base("Invalid parameter => " + parameter)
    {
        
    }
}