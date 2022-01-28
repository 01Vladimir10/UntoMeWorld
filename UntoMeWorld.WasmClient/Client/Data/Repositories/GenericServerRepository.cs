﻿using System.Data;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Client.Utils;

namespace UntoMeWorld.WasmClient.Client.Data.Repositories;

/// <summary>
/// Allows to create a basic CRUD, it uses and endpoint to execute the CRUD tasks,
/// The endpoint must comply with these requisites
///     - Return responses wrapped in a ResponseDto
///     - Return a list of items when sending a GET request.
///     - Delete elements on DELETE request.
///     - Update elements on a PUT request.
/// </summary>
/// <typeparam name="TModel">T</typeparam>
public abstract class GenericServerRepository<TModel> : IRepository<TModel> where TModel : IModel
{
    private readonly string _endPoint;
    private readonly HttpClient _client;

    protected GenericServerRepository(HttpClient client, string endPoint)
    {
        _client = client;
        _endPoint = endPoint;
    }

    public async Task<TModel> Add(TModel item)
    {
        if (item == null)
            throw new NoNullAllowedException();
        var response = await _client.PostJsonAsync<TModel>(_endPoint, item);
        return response!.IsSuccessful ? response.Data : default;
    }

    public async Task<TModel> Update(TModel item)
    {
        if (item == null)
            throw new NoNullAllowedException();
        var response = await _client.PutJsonAsync<TModel>(_endPoint, item);
        return response!.IsSuccessful ? response.Data : default;
    }

    public async Task Delete(TModel item)
    {
        if (item == null)
            throw new NoNullAllowedException();
        await _client.DeleteJsonAsync<TModel>(HttpUtils.BuildQuery(_endPoint, "id", item.Id));
    }

    public async Task<PaginationResult<TModel>> Query(
        string query = null, string sortBy = null,
        bool sortDesc = false,
        int page = 1, int pageSize = 50)
    {
        var url = HttpUtils.BuildUrl(
            _endPoint,
            "page", page,
            "pageSize", pageSize,
            "query", query,
            "sortBy", sortBy,
            "sortDesc", sortDesc);
        
        var response = await _client.GetJsonAsync<PaginationResult<TModel>>(url);
        return response is { IsSuccessful: true, Data: not null } 
            ? response.Data 
            : new PaginationResult<TModel>();
    }

    public Task<IEnumerable<TModel>> Find(Predicate<TModel> query)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<TModel>> All()
    {
        var result = await Query(pageSize: 1000);
        return result.Result;
    }
}