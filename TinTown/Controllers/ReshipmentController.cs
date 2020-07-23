using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReshipmentController : ControllerBase
    {
        private readonly ReshipmentLogic _reshipmentLogic;
        public ReshipmentController()
        {
            _reshipmentLogic = new ReshipmentLogic();
        }

        [HttpGet]
        public async Task<JsonResult> AwbList([FromQuery] int location)
        {
            try
            {
                return await _reshipmentLogic.AwbList(location).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reshipmentLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewDspandAwb([FromBody] Reshipment reshipment)
        {
            try
            {
                return await _reshipmentLogic.NewDspandAwb(reshipment).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _reshipmentLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}
