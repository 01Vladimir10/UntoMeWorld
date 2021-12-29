using UntoMeWorld.Domain.Common;
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
        item.CreatedOn = DateTime.Now;
        return Store.Add(item);
    }
    public Task<TModel> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new MissingParametersException();
        return Store.Get(id);
    }
    public Task<TModel> Update(TModel item)
    {
        item.LastUpdatedOn = DateTime.Now;
        return Store.Update(item);
    }

    public Task Restore(string item)
    {
        return Store.Restore(item);
    }

    public Task Delete(string id, bool softDelete = true)
    {
        return softDelete ? Store.SoftDelete(id) : Store.Delete(id);
    }
    public Task<IEnumerable<TModel>> GetAll(string query = null)
    {
        return Store.All(query);
    }

    public Task<IEnumerable<TModel>> Add(IEnumerable<TModel> item)
    {
        return Store.Add(item.ToList());
    }

    public Task<PaginationResult<TModel>> Query(string query = null, string orderBy = null, bool orderDesc = false, bool deleted = false, int page = 1, int pageSize = 100)
    {
        // Validate if the property exists
        if (!string.IsNullOrEmpty(orderBy) &&
            typeof(TModel).GetProperties().ToList().FindIndex(p => p.Name.ToLower().Equals(orderBy.ToLower())) < 0)
            throw new InvalidSortByProperty();
        
        // validate if the query has more than 3 characters
        if (!string.IsNullOrEmpty(query) && query.Trim().Length < 3)
            throw new InvalidQueryLengthException();

        var parameters = new List<DatabaseQueryParameter>
        {
            new()
            {
                PropertyName = nameof(IModel.IsDeleted),
                Operator = DatabaseQueryOperator.Equal,
                Value = deleted
            }
        };
        if (!string.IsNullOrEmpty(query))
            parameters.Add(new DatabaseQueryParameter
            {
                Operator = DatabaseQueryOperator.TextQuery,
                Value = query
            });
        
        return Store.Query(parameters, orderBy, orderDesc, page, pageSize);
    }

    public Task<IEnumerable<TModel>> Update(IEnumerable<TModel> item)
    {
        return Store.Update(item.ToList());
    }

    public Task Restore(IEnumerable<string> item)
    {
        return Store.Restore(item.ToArray());
    }

    public Task Delete(IEnumerable<string> id, bool softDelete = true)
    {
        return softDelete ? Store.SoftDelete(id.ToArray()) : Store.Delete(id.ToArray());
    }
}