using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Extensions;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.WasmClient.Client.Utils.Extensions;
using UntoMeWorld.WasmClient.Shared.DTOs;
using UntoMeWorld.WasmClient.Shared.Model;
using static UntoMeWorld.Domain.Query.QueryLanguage;

namespace UntoMeWorld.WasmClient.Client.Data.Stores;

public abstract class GenericRemoteStore<TModel, TAddDto, TUpdateDto> : IStore<TModel>
    where TModel : IRecyclableModel, IModel
    where TAddDto : IDto<TModel>
    where TUpdateDto : IUpdateDto<TModel>
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

    public async Task<IEnumerable<TModel>?> All()
        => await Query(Ne(nameof(IRecyclableModel.IsDeleted), true)).ContinueWith(t => t.Result.Result);

    public async Task<IEnumerable<TModel>?> All(string query)
        => string.IsNullOrEmpty(query)
            ? await All()
            : await Query(TextSearch(query)).ContinueWith(t => t.Result.Result);

    public Task<IEnumerable<TModel>> All(Predicate<TModel> query)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<TModel>> Query(QueryFilter? filter = null,
        string? textQuery = null,
        string? orderBy = null,
        bool orderByDesc = false,
        int page = 1,
        int pageSize = 100)
        => _client.PostJsonAsync<PaginationResult<TModel>>(Paths.Query, new QueryRequestDto
            {
                Filter = filter,
                TextQuery = textQuery,
                OrderBy = orderBy ?? string.Empty,
                OrderDesc = orderByDesc,
                Page = page,
                PageSize = pageSize
            })
            .OrElse(new PaginationResult<TModel>());

    public Task<TModel> AddOne(TModel data)
        => _client.PostJsonAsync<TModel>(Paths.Add, ToAddDto(data))
            .OrElse(data);

    public Task<TModel> UpdateOne(TModel data)
        => _client.PutJsonAsync<TModel>(Paths.Update, ToUpdateDto(data))
            .OrElse(data);

    public Task DeleteOne(string key)
        => _client.DeleteJsonAsync<bool>($"{Paths.Delete}/{key}");

    public Task PurgeOne(string key)
        => _client.DeleteJsonAsync<bool>($"{Paths.Purge}/{key}");

    public Task RestoreOne(string key)
        => _client.PutJsonAsync<bool>($"{Paths.Restore}/{key}", new object());

    public Task DeleteMany(IEnumerable<string> keys)
        => _client.PostJsonAsync<bool>(Paths.DeleteMany, keys);

    public Task PurgeMany(IEnumerable<string> keys)
        => _client.PostJsonAsync<bool>(Paths.PurgeMany, keys);

    public Task RestoreMany(IEnumerable<string> keys)
        => _client.PostJsonAsync<bool>(Paths.RestoreMany, keys);

    public Task<IEnumerable<TModel>> AddMany(List<TModel> data)
        => _client.PostJsonAsync<IEnumerable<TModel>>(Paths.AddMany, data.Select(ToAddDto))
            .OrElse(new List<TModel>());

    public Task<IEnumerable<TModel>> UpdateMany(List<TModel> data)
        => _client.PostJsonAsync<IEnumerable<TModel>>(Paths.UpdateMany, data.Select(ToUpdateDto))
            .OrElse(new List<TModel>());

    public Task<TModel?> Get(string id)
        => _client.GetJsonAsync<TModel>($"{Paths.GetOne}/{id}");

    protected abstract TAddDto ToAddDto(TModel model);
    protected abstract TUpdateDto ToUpdateDto(TModel model);
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
    public string PurgeMany { get; }

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