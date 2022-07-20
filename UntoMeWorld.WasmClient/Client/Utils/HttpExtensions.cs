using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using UntoMeWorld.WasmClient.Client.Data.Common;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Utils;

public static class HttpExtensions
{
    public static Task<T> PostJsonAsync<T>(this HttpClient client, string url, object content)
        => InterpretResponse<T>(client.PostAsJsonAsync(url, content));


    public static Task<T> PutJsonAsync<T>(this HttpClient client, string url, object content)
        => InterpretResponse<T>(client.PutAsJsonAsync(url, content));

    public static Task<T> DeleteJsonAsync<T>(this HttpClient client, string url)
        => InterpretResponse<T>(client.DeleteAsync(url));

    public static Task<T> GetJsonAsync<T>(this HttpClient client, string url)
        => InterpretResponse<T>(client.GetAsync(url));
    

    private static async Task<TResult> InterpretResponse<TResult>(Task<HttpResponseMessage> responseTask)
    {
        var response = await responseTask;
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TResult>();

        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();
        throw new ApiCallErrorException(error);
    }
}