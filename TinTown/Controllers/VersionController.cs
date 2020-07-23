using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly VersionLogic _versionLogic;
        public VersionController()
        {
            _versionLogic = new VersionLogic();
        }
        [HttpGet]
        public async Task<JsonResult> ChangeLog(string type)
        {
            try
            {
                return await _versionLogic.ChangeLog(type).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _versionLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckForUpdate(string type)
        {
            try
            {
                return await _versionLogic.CheckForUpdate(type).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _versionLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}