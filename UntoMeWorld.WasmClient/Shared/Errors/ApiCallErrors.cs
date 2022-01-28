namespace UntoMeWorld.WasmClient.Shared.Errors;

public abstract class InvalidApiRequestRequestException : Exception
{
    
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