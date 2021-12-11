using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Server.Controllers;

public class GenericController<TModel, TKey> : BaseController<TModel, TKey>
{
    
    public override Task<ActionResult<ResponseDto<TModel>>> Add(TModel item)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<bool>>> Delete(TKey itemId)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<TModel>>> Update(TModel item)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> All(string query = null)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkInsert(List<TModel> items)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<IEnumerable<TModel>>>> BulkUpdate(List<TModel> items)
    {
        throw new System.NotImplementedException();
    }

    public override Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<TKey> itemId)
    {
        throw new System.NotImplementedException();
    }
}