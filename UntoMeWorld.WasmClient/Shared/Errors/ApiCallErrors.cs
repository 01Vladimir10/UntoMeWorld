namespace UntoMeWorld.WasmClient.Shared.Errors;

public abstract class InvalidApiRequestRequestException : Exception
{
    
}

public class InvalidSortByProperty : Exception
{
    public override string Message { get; } = "The name of the property to order by is invalid";
}