#nullable enable
namespace UntoMeWorld.WasmClient.Shared.Model;

public class ResponseDto<T>
{
    public bool IsSuccessful { get; set; }
    public T? Data { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public static ResponseDto<TItem> Successful<TItem>(TItem data)
        => new()
        {
            IsSuccessful = true,
            Data = data,
        };

    public static ResponseDto<T> Error(string error)
        => new()
        {
            IsSuccessful = false,
            ErrorMessage = error
        };

    public Exception ToException()
        => new ApiCallException(ErrorMessage);
}

public class ApiCallException : Exception
{
    public ApiCallException(string error) : base(
        "An unexpected error occured while executing a call to the API, error message: " + error)
    {
    }
}