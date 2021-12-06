using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UntoMeWorld.WebClient.Server.Services;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ChurchesController : BaseController<Church>
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
                return new JsonResult(ResponseDto<Domain.Model.IChurch>.Error(e.Message));
            }
        }

        public override async Task<ActionResult<ResponseDto<bool>>> Delete(Church item)
        {
            try
            {
                await _service.DeleteChurch(item);
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
                return new JsonResult(ResponseDto<Domain.Model.IChurch>.Error(e.Message));
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
                return new JsonResult(ResponseDto<Domain.Model.IChurch>.Error(e.Message));
            }
        }
    }
}