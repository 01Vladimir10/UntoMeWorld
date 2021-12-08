using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WebClient.Server.Services;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Server.Controllers
{
    public class ChildrenController : BaseController<Child, string>
    {
        private readonly ChildrenService _service;

        public ChildrenController(ChildrenService service)
        {
            _service = service;
        }

        public override Task<ActionResult<ResponseDto<Child>>> Add(Child item)
        {
            return ServiceCallResult(() => _service.AddChild(item));
        }

        public override Task<ActionResult<ResponseDto<bool>>> Delete(string itemId)
        {
            return ServiceCallResult(async () =>
            {
                await _service.DeleteChild(itemId);
                return true;
            });
        }

        public override Task<ActionResult<ResponseDto<Child>>> Update(Child item)
        {
            return ServiceCallResult(() => _service.UpdateChild(item));
        }

        public override Task<ActionResult<ResponseDto<IEnumerable<Child>>>> All(string query = null)
        {
            return ServiceCallResult(() => _service.GetAllChildren(query));
        }

        public override Task<ActionResult<ResponseDto<IEnumerable<Child>>>> BulkInsert(List<Child> items)
        {
            return ServiceCallResult(() => _service.AddChild(items));
        }

        public override Task<ActionResult<ResponseDto<IEnumerable<Child>>>> BulkUpdate(List<Child> items)
        {
            return ServiceCallResult(() => _service.UpdateChild(items));
        }

        public override Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<string> items)
        {
            return ServiceCallResult(async () =>
            {
                await _service.DeleteChild(items);
                return true;
            });
        }
    }
}