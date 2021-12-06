#nullable enable
namespace UntoMeWorld.WebClient.Shared.Model
{
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

    }
}