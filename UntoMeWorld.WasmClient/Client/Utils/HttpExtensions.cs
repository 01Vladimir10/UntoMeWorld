using System.Net.Http.Json;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Utils;

public static class HttpExtensions
{
    public static async Task<ResponseDto<T>> PostJsonAsync<T>(this HttpClient client, string url, object content)
    {
        Console.WriteLine($"sending post to {url}...");
        var body = JsonContent.Create(content);
        var response = await client.PostAsync(url, body);
        return await response.Content.ReadFromJsonAsync<ResponseDto<T>>();
    }

    public static async Task<ResponseDto<T>> PutJsonAsync<T>(this HttpClient client, string url, object content)
    {
        var body = JsonContent.Create(content);
        var response = await client.PutAsync(url, body);
        return await response.Content.ReadFromJsonAsync<ResponseDto<T>>();
    }

    public static async Task<ResponseDto<T>> DeleteJsonAsync<T>(this HttpClient client, string url)
    {
        var response = await client.DeleteAsync(url);
        return await response.Content.ReadFromJsonAsync<ResponseDto<T>>();
    }

    public static async Task<ResponseDto<T>> GetJsonAsync<T>(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ResponseDto<T>>() : default;
    }
}