using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WebClient.Server.Services;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Server.Controllers
{
    public class ChurchesController : BaseController<Church, string>
    {
        private readonly ChurchesService _service;

        public ChurchesController(ChurchesService service)
        {
            _service = service;
        }

        public override Task<ActionResult<ResponseDto<Church>>> Add(Church item)
        {
            return ServiceCallResult(() => _service.AddChurch(item));
        }

        public override Task<ActionResult<ResponseDto<bool>>> Delete(string item)
        {
            return ServiceCallResult(async () =>
            {
                await _service.DeleteChurch(item);
                return true;
            });
        }

        public override Task<ActionResult<ResponseDto<Church>>> Update(Church item)
        {
            return ServiceCallResult(() => _service.UpdateChurch(item));
        }

        public override Task<ActionResult<ResponseDto<IEnumerable<Church>>>> All(string query = null)
        {
            return ServiceCallResult(() =>
                string.IsNullOrEmpty(query) ? _service.GetAllChurches() : _service.GetChurchesByQuery(query));
        }

        public override async Task<ActionResult<ResponseDto<IEnumerable<Church>>>> BulkInsert(List<Church> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be added was not provided");
            return await ServiceCallResult(() => _service.AddChurch(items));
        }

        public override async Task<ActionResult<ResponseDto<IEnumerable<Church>>>> BulkUpdate(List<Church> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be updated was not provided");
            return await ServiceCallResult(() => _service.UpdateChurch(items));
        }

        public override async Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<string> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be deleted was not provided");

            return await ServiceCallResult(async () =>
            {
                await _service.DeleteChurch(items);
                return true;
            });
        }
    }
}