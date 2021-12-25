using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Shared.Errors;

namespace UntoMeWorld.WasmClient.Server.Services;

public abstract class GenericDatabaseService<TModel> : IDatabaseService<TModel, string> where TModel : IModel
{
    protected readonly IStore<TModel> Store;

    public GenericDatabaseService(IStore<TModel> store)
    {
        Store = store;
    }

    public Task<TModel> Add(TModel item)
    {
        return Store.Add(item);
    }

    public Task<TModel> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<TModel> Update(TModel item)
    {
        return Store.Update(item);
    }

    public Task Delete(string id)
    {
        return Store.Delete(id);
    }

    public Task<IEnumerable<TModel>> GetAll(string query = null)
    {
        return Store.All(query);
    }

    public Task<IEnumerable<TModel>> Add(IEnumerable<TModel> item)
    {
        return Store.Add(item.ToList());
    }

    public Task<PaginationResult<TModel>> Query(string query = null, string orderBy = null, bool orderDesc = false, int page = 1, int pageSize = 100)
    {
        // Validate if the property exists
        if (!string.IsNullOrEmpty(orderBy) &&
            typeof(TModel).GetProperties().ToList().FindIndex(p => p.Name.ToLower().Equals(orderBy.ToLower())) < 0)
            throw new InvalidSortByProperty();
        
        // validate if the query has more than 3 characters
        if (!string.IsNullOrEmpty(query) && query.Trim().Length < 3)
            throw new InvalidQueryLengthException();
        
        return Store.Query(query, orderBy, orderDesc, page, pageSize);
    }

    public Task<IEnumerable<TModel>> Update(IEnumerable<TModel> item)
    {
        return Store.Update(item.ToList());
    }

    public Task Delete(IEnumerable<string> id)
    {
        return Store.Delete(id.ToArray());
    }
}