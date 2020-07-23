using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RejectionController : ControllerBase
    {
        private readonly RejectionLogic _rejectionLogic;
        public RejectionController()
        {
            _rejectionLogic = new RejectionLogic();
        }

        [HttpPost]
        public async Task<JsonResult> RejectionWork([FromBody] Rejection rejection)
        {
            try
            {
                return await _rejectionLogic.RejectionWork(rejection).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _rejectionLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
