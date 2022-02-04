namespace UntoMeWorld.WasmClient.Shared.Errors;

public abstract class ApiServerError : Exception
{
    protected ApiServerError(string message) : base(message)
    {
    }
}

public class InvalidServiceConfigurationError : ApiServerError
{
    public InvalidServiceConfigurationError(string message) : base(message)
    {
    }
}