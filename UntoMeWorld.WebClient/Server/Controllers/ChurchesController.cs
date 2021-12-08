using System;
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
        public override async Task<ActionResult<ResponseDto<Church>>> Add(Church item)
        {
            try
            {
                var result = await _service.AddChurch(item);
                if (result == null) 
                    throw new Exception("Something went wrong");
                return ResponseDto<Church>.Successful(item);
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }

        public override async Task<ActionResult<ResponseDto<bool>>> Delete(string item)
        {
            try
            {
                await _service.DeleteChurch(new Church {Id = item});
                return ResponseDto<bool>.Successful(true);
            }
            catch (Exception e)
            {
                return ResponseDto<bool>.Error(e.Message);
            }
        }

        public override async Task<ActionResult<ResponseDto<Church>>> Update(Church item)
        {
            try
            {
                var result = await _service.UpdateChurch(item);
                if (result == null)
                    throw new Exception("Something went wrong");
                return ResponseDto<Church>.Successful(item);
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }
        
        public override async Task<ActionResult<ResponseDto<IEnumerable<Church>>>> All(string query = null)
        {
            try
            {
                var result = string.IsNullOrEmpty(query) ? await _service.GetAllChurches() : await _service.GetChurchesByQuery(query);
                if (result == null)
                    throw new Exception("Something went wrong");
                return new JsonResult(ResponseDto<IEnumerable<Church>>.Successful(result));
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }

        public override async Task<ActionResult<ResponseDto<IEnumerable<Church>>>> BulkInsert(List<Church> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be added was not provided");
            
            try
            {
                var result = await _service.AddChurch(items);
                if (result == null)
                    throw new Exception("Something went wrong");
                return new JsonResult(ResponseDto<IEnumerable<Church>>.Successful(result));
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }

        public override async Task<ActionResult<ResponseDto<IEnumerable<Church>>>> BulkUpdate(List<Church> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be updated was not provided");
            
            try
            {
                var result = await _service.UpdateChurch(items);
                if (result == null)
                    throw new Exception("Something went wrong");
                return new JsonResult(ResponseDto<IEnumerable<Church>>.Successful(result));
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }

        public override async Task<ActionResult<ResponseDto<bool>>> BulkDelete(List<string> items)
        {
            if (items == null || !items.Any())
                return BadRequest("The list of ids to be deleted was not provided");

            try
            {
                await _service.DeleteChurch(items.Select(id => new Church {Id = id}));
                return new JsonResult(ResponseDto<IEnumerable<Church>>.Successful(true));
            }
            catch (Exception e)
            {
                return new JsonResult(ResponseDto<Church>.Error(e.Message));
            }
        }
    }
}