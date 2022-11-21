using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Errors;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.Domain.Validation;

namespace UntoMeWorld.Application.Services.Abstractions;

public abstract class GenericService<TModel> : IService<TModel, string>
    where TModel : IModel, IRecyclableModel
{
    protected readonly IStore<TModel> Store;
    private readonly ILogsService _logs;
    private readonly string _storeName;

    public GenericService(IStore<TModel> store, ILogsService logs, string storeName)
    {
        Store = store;
        _logs = logs;
        _storeName = storeName;
    }

    public Task<TModel> Add(TModel item)
        => _logs.ExecuteAndLogInsertion(_storeName, () =>
        {
            {
                Validate(item);
                item.CreatedOn = DateTime.Now;
                return Store.AddOne(item);
            }
        });

    public Task<TModel?> Get(string id)
        => _logs.ExecuteAndLog(ActionLogType.Get, _storeName, id, () =>
        {
            {
                if (string.IsNullOrEmpty(id))
                    throw new MissingParametersException();
                return Store.Get(id);
            }
        });

    public Task<TModel> Update(TModel item)
        => _logs.ExecuteAndLog(ActionLogType.Update, _storeName, item.Id, () =>
        {
            Validate(item);
            item.LastUpdatedOn = DateTime.Now;
            return Store.UpdateOne(item);
        });

    public Task Restore(string item)
        => _logs.ExecuteAndLog(ActionLogType.Restore, _storeName, item, () => Store.RestoreOne(item));

    public Task Delete(string id, bool softDelete = true)
        => _logs.ExecuteAndLog(softDelete ? ActionLogType.Delete : ActionLogType.Purge, _storeName, id,
            () => softDelete ? Store.DeleteOne(id) : Store.PurgeOne(id));

    public Task<IEnumerable<TModel>> GetAll(string? query = null)
        => _logs.ExecuteAndLog(ActionLogType.Query, _storeName, new { query }, () => Store.All(query ?? string.Empty));

    public Task<IEnumerable<TModel>> Add(IEnumerable<TModel> items)
        => _logs.ExecuteAndLogInsertion(_storeName, () =>
        {
            {
                var models = items.ToList();
                Validate(models);
                return Store.AddMany(models.ToList());
            }
        });

    public Task<PaginationResult<TModel>> Query(QueryFilter? filter = null,
        string? textQuery = null,
        string? orderBy = null,
        bool orderDesc = false, int page = 1, int pageSize = 100)
        => _logs.ExecuteAndLog(ActionLogType.Query, _storeName,
            new
            {
                Filter = filter, TextQuery = textQuery, OrderBy = orderBy, OrderDesc = orderDesc, Page = page,
                PageSize = pageSize
            },
            () => Store.Query(filter, textQuery, orderBy, orderDesc, page, pageSize));

    public Task<IEnumerable<TModel>> Update(IEnumerable<TModel> item)
    {
        var items = item.ToList();
        return _logs.ExecuteAndLog(ActionLogType.BulkUpdate, _storeName, items, () =>
        {
            Validate(items);
            return Store.UpdateMany(items);
        });
    }

    public Task Restore(IEnumerable<string> item)
    {
        var items = item.ToArray();
        return _logs.ExecuteAndLog(ActionLogType.BulkRestore, _storeName, items, () => Store.RestoreMany(items));
    }

    public Task Delete(IEnumerable<string> id, bool softDelete = true)
    {
        var items = id.ToArray();
        return _logs.ExecuteAndLog(softDelete ? ActionLogType.BulkDelete : ActionLogType.BulkPurge, _storeName, items,
            () => softDelete ? Store.DeleteMany(items) : Store.PurgeMany(items));
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