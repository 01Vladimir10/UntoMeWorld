using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Shared.Errors;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Server.Controllers;

public abstract class GenericController<TModel, TKey> : BaseController<TModel, TKey>
{
    protected readonly IDatabaseService<TModel, TKey> DatabaseService;

    protected GenericController(IDatabaseService<TModel, TKey> databaseService)
    {
        DatabaseService = databaseService;
    }

    public override Task<ActionResult<ResponseDto<TModel>>> Add(TModel item)
    {
        return ServiceCallResult(() => DatabaseService.Add(item));
    }

    public override Task<ActionResult<ResponseDto<bool>>> Delete(TKey itemId)
    {
        
        return ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(itemId);
            return true;
        });
    }

    public override Task<ActionResult<ResponseDto<TModel>>> Update(TModel item)
    {
        Console.WriteLine("Updating  element => " + item);
        return ServiceCallResult(() => DatabaseService.Update(item));
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> All(string query = null, string sortBy = "", bool sortDesc = false)
    {
        return ServiceCallResult(async () =>
        {
            var data = await DatabaseService.GetAll(query);
            // For now, we are going to sort this on the server, but we'll need
            // to delegate this task to the database in the future
            if (string.IsNullOrEmpty(sortBy))
                return data;
            var properties = typeof(TModel).GetProperties();
            var property = properties.FirstOrDefault(p => string.Equals(p.Name, sortBy, StringComparison.CurrentCultureIgnoreCase));
            if (property == null)
                throw new InvalidSortByProperty();
            
            return sortDesc ?
                data.OrderByDescending(item => property.GetValue(item, null)) : 
                data.OrderBy(item => property.GetValue(item, null));
        });
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkInsert(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Add(items));
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkUpdate(List<TModel> items)
    {
        return ServiceCallResult(() => DatabaseService.Update(items));
    }

    public override Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<TKey> itemId)
    {
        return ServiceCallResult(async () =>
        {
            await DatabaseService.Delete(itemId);
            return true;
        });
    }
}