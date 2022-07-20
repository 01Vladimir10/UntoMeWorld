using UntoMeWorld.Domain.Errors;

namespace UntoMeWorld.WasmClient.Shared.Model;

public class ErrorDto
{
    public string Error { get; set; }
    public string Cause { get; set; }
    public string Fix { get; set; }

    public ErrorDto()
    {
        
    }

    public ErrorDto(UserErrorException e)
    {
        Error = e.Message;
        Cause = e.Cause;
        Fix = e.Fix;
    }
}