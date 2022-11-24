using System.Net;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using UntoMeWorld.WasmClient.Client.Data.Common;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public static class HttpExtensions
{
    public static Task<HttpResponseMessage> SendHeadAsync(this HttpClient client, string url)
        => client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Head,
            RequestUri = new Uri(url)
        });

    public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string url, object? body)
        => client.SendAsync(new HttpRequestMessage
        {
            Content = body == null
                ? null
                : new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
            Method = HttpMethod.Patch,
            RequestUri = new Uri(url)
        });

    public static Task<T?> PostJsonAsync<T>(this HttpClient client, string url, object content)
        => ExecuteRequest<T>(client, HttpMethod.Post, url, content);

    public static Task<T?> PutJsonAsync<T>(this HttpClient client, string url, object content)
        => ExecuteRequest<T>(client, HttpMethod.Put, url, content);

    public static Task<T?> DeleteJsonAsync<T>(this HttpClient client, string url)
        => ExecuteRequest<T>(client, HttpMethod.Delete, url);

    public static Task<T?> GetJsonAsync<T>(this HttpClient client, string url)
        => ExecuteRequest<T>(client, HttpMethod.Get, url);


    private static Task<T?> ExecuteRequest<T>(HttpClient client, HttpMethod method, string url, object? body = null)
    {
        var request = new HttpRequestMessage { Method = method };
        request.RequestUri = new Uri(url, UriKind.Relative);
        if (body != null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        return InterpretResponse<T>(client.SendAsync(request));
    }

    private static async Task<TResult?> InterpretResponse<TResult>(Task<HttpResponseMessage> responseTask)
    {
        var response = await responseTask;

        if (response.IsSuccessStatusCode)
            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException("You do not have sufficient permissions to execute this task");

        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();
        if (error != null)
            throw new ApiCallErrorException(error);
        throw new Exception(await response.Content.ReadAsStringAsync());
    }
}