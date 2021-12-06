using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Server.Controllers
{
    public abstract class BaseController<T> : ControllerBase
    {
        [HttpPost]
        public abstract Task<ActionResult<ResponseDto<T>>> Add(T item);
        [HttpDelete]
        public abstract Task<ActionResult<ResponseDto<bool>>> Delete(T item);
        [HttpPut]
        public abstract Task<ActionResult<ResponseDto<T>>> Update(T item);
        [HttpGet]
        public abstract Task<ActionResult<ResponseDto<IEnumerable<T>>>> All(string query = null);
    }
}