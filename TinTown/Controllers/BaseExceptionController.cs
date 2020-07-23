using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseExceptionController : ControllerBase
    {
        [HttpPost]
        public async Task<JsonResult> UIException([FromBody] BaseException baseExec)
        {
            BaseExceptionLogic bel = new BaseExceptionLogic();
            try
            {
                return await bel.ExceptionLog(baseExec).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await bel.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}