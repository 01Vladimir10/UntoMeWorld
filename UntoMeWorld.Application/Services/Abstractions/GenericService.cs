using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Errors;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Validation;

namespace UntoMeWorld.Application.Services.Abstractions;

public abstract class GenericService<TModel> : IService<TModel, string>
    where TModel : IModel, IRecyclableModel
{
    protected readonly IStore<TModel> Store;

    public GenericService(IStore<TModel> store)
    {
        Store = store;
    }

    public Task<TModel> Add(TModel item)
    {
        Validate(item);
        item.CreatedOn = DateTime.Now;
        return Store.AddOne(item);
    }

    public Task<TModel> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new MissingParametersException();
        return Store.Get(id);
    }

    public Task<TModel> Update(TModel item)
    {
        Validate(item);
        item.LastUpdatedOn = DateTime.Now;
        return Store.UpdateOne(item);
    }

    public Task Restore(string item)
    {
        return Store.RestoreOne(item);
    }

    public Task Delete(string id, bool softDelete = true)
    {
        return softDelete ? Store.DeleteOne(id) : Store.PurgeOne(id);
    }

    public Task<IEnumerable<TModel>> GetAll(string? query = null)
    {
        return Store.All(query ?? string.Empty);
    }

    public Task<IEnumerable<TModel>> Add(IEnumerable<TModel> item)
    {
        var models = item.ToList();
        Validate(models);
        return Store.AddMany(models.ToList());
    }

    public Task<PaginationResult<TModel>> Query(QueryFilter? filter = null, 
        string? textQuery = null,
        string? orderBy = null,
        bool orderDesc = false, int page = 1, int pageSize = 100)
    {
        return Store.Query(filter, textQuery, orderBy, orderDesc, page, pageSize);
    }

    public Task<IEnumerable<TModel>> Update(IEnumerable<TModel> item)
    {
        var models = item.ToList();
        Validate(models);
        return Store.UpdateMany(models);
    }

    public Task Restore(IEnumerable<string> item)
    {
        return Store.RestoreMany(item.ToArray());
    }

    public Task Delete(IEnumerable<string> id, bool softDelete = true)
    {
        return softDelete ? Store.DeleteMany(id.ToArray()) : Store.PurgeMany(id.ToArray());
    }

    private static void Validate(TModel model)
    {
        var result = ModelValidator.Validate(model);
        if (!result.IsValid)
            throw new ModelValidationException(result);
    }
    private static void Validate(IEnumerable<TModel> models)
    {
        foreach (var model in models)
        {
            Validate(model);
        }
    }
}