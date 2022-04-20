using System.Net.Http.Json;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Client.Utils;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public abstract class GenericRemoteStore<T> : IStore<T> where T : IRecyclableModel, IModel
{
    protected readonly string EndPoint;
    private readonly HttpClient _client;
    protected readonly ServerActionsPaths Paths;

    protected GenericRemoteStore(HttpClient client, string endPoint)
    {
        _client = client;
        EndPoint = endPoint;
        Paths = new ServerActionsPaths(EndPoint);
    }


    public Task<IEnumerable<T>> All()
        => _client.GetFromJsonAsync<IEnumerable<T>>(EndPoint);

    public Task<IEnumerable<T>> All(string query)
        => InterpretServerResponse(_client.PostJsonAsync<IEnumerable<T>>(EndPoint, new object()));

    public Task<IEnumerable<T>> All(Predicate<T> query)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<T>> Query(QueryFilter filter = null, string orderBy = null,
        bool orderByDesc = false,
        int page = 1,
        int pageSize = 100)
        => InterpretServerResponse(_client.PostJsonAsync<PaginationResult<T>>(Paths.Query, new QueryRequestDto
        {
            Filter = filter,
            OrderBy = orderBy ?? string.Empty,
            OrderDesc = orderByDesc,
            Page = page,
            PageSize = pageSize
        }));

    public Task<T> AddOne(T data)
        => InterpretServerResponse(_client.PostJsonAsync<T>(Paths.Add, data));

    public Task<T> UpdateOne(T data)
        => InterpretServerResponse(_client.PutJsonAsync<T>(Paths.Update, data));
    
    public Task DeleteOne(string key)
        => InterpretServerResponse(_client.DeleteJsonAsync<bool>($"{Paths.Delete}/{key}"));
    public Task PurgeOne(string key)
        => InterpretServerResponse(_client.DeleteJsonAsync<bool>($"{Paths.Purge}/{key}"));

    public Task RestoreOne(string key)
        => InterpretServerResponse(_client.PutJsonAsync<bool>($"{Paths.Restore}/{key}", new object()));

    public Task DeleteMany(IEnumerable<string> keys)
        => InterpretServerResponse(_client.PostJsonAsync<bool>(Paths.DeleteMany, keys));

    public Task PurgeMany(IEnumerable<string> keys)
        => InterpretServerResponse(_client.PostJsonAsync<bool>(Paths.PurgeMany, keys));

    public Task RestoreMany(IEnumerable<string> keys)
        => InterpretServerResponse(_client.PostJsonAsync<bool>(Paths.RestoreMany, keys));

    public Task<IEnumerable<T>> AddMany(List<T> data)
        => InterpretServerResponse(_client.PostJsonAsync<IEnumerable<T>>(Paths.AddMany, data));

    public Task<IEnumerable<T>> UpdateMany(List<T> data)
        => InterpretServerResponse(_client.PostJsonAsync<IEnumerable<T>>(Paths.UpdateMany, data));
    
    public Task<T> Get(string id)
        => InterpretServerResponse(_client.GetJsonAsync<T>($"{Paths.GetOne}/{id}"));

    private static async Task<TResult> InterpretServerResponse<TResult>(Task<ResponseDto<TResult>> callback)
    {
        try
        {
            var result = await callback;
            if (result.IsSuccessful)
                return result.Data;
            throw new Exception(result.ErrorMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }
}

public class ServerActionsPaths
{
    public string Add { get; }
    public string GetOne { get; }
    public string Delete { get; }
    public string Update { get; }
    public string Query { get; }
    public string Restore { get; }
    public string Purge { get; }
    public string AddMany { get; }
    public string DeleteMany { get; }
    public string UpdateMany { get; }
    public string RestoreMany { get; }
    public string PurgeMany{ get; }

    public ServerActionsPaths(string endpoint)
    {
        Add = endpoint;
        GetOne = endpoint;
        Delete = endpoint;
        Update = endpoint;
        Query = endpoint + "/query";
        Restore = endpoint + "/bin";
        Purge = endpoint + "/bin";
        AddMany = endpoint + "/bulk/insert";
        DeleteMany = endpoint + "/bulk/delete";
        UpdateMany = endpoint + "/bulk/update";
        RestoreMany = endpoint + "/bin/bulk/restore";
        PurgeMany = endpoint + "/bin/bulk/delete";
    }
} 