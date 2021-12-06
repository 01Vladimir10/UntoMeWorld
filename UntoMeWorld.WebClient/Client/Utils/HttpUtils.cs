using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace UntoMeWorld.WebClient.Client.Utils
{
    public static class HttpUtils
    {
        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string url, object content)
        {
            var body = JsonContent.Create(content);
            var response = await client.PostAsync(url, body);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public static async Task<T> PutJsonAsync<T>(this HttpClient client, string url, object content)
        {
            var body = JsonContent.Create(content);
            var response = await client.PutAsync(url, body);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public static async Task<T> DeleteJsonAsync<T>(this HttpClient client, string url, object content)
        {
            var body = JsonContent.Create(content);
            var response = await client.PutAsync(url, body);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}