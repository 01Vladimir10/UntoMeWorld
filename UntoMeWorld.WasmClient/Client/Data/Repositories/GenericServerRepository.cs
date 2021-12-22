using System.Data;
using UntoMeWorld.Domain.Model;
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
        await _client.DeleteJsonAsync<TModel>(_endPoint + "?itemId=" + item.Id);
    }

    public async Task<IEnumerable<TModel>> All(string query = null, string sortBy = null, bool sortDesc = false)
    {
        var endpoint = _endPoint;
        var parameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
        
        if (!string.IsNullOrEmpty(query))
            parameters.Add("query", query);
        
        if (!string.IsNullOrEmpty(sortBy))
            parameters.Add("sortBy", sortBy);
        
        if (sortDesc)
            parameters.Add("sortDesc", "true");

        if (parameters.Count > 0)
            endpoint += $"?{parameters}";
        
        var response = await _client.GetJsonAsync<IEnumerable<TModel>>(endpoint);
        return response is { IsSuccessful: true, Data: { } } ? response.Data : new List<TModel>();
    }

    public Task<IEnumerable<TModel>> All(Predicate<TModel> query)
    {
        throw new NotImplementedException();
    }
} 