using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Common;

public class ApiCallErrorException : Exception
{
    public ErrorDto ErrorDto { get; }

    public ApiCallErrorException(ErrorDto errorDto)
    {
        ErrorDto = errorDto;
    }
}