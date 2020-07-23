using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReturnManifestController : ControllerBase
    {
        private readonly ReturnManifestLogic _returnManifestLogic;

        public ReturnManifestController()
        {
            _returnManifestLogic = new ReturnManifestLogic();
        }

        [HttpGet]
        public async Task<JsonResult> ReturnManifestList([FromQuery] int locationid)
        {
            try
            {
                return await _returnManifestLogic.ReturnManifestList(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returnManifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateReturn([FromBody] ReturnManifest returnManifest)
        {
            try
            {
                return await _returnManifestLogic.CreateReturn(returnManifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returnManifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ReturnManifestInfo([FromQuery] string return_manifest_no)
        {
            try
            {
                return await _returnManifestLogic.ReturnManifestInfo(return_manifest_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returnManifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AWBScan([FromBody] ReturnManifest returnManifest)
        {
            try
            {
                return await _returnManifestLogic.AWBScan(returnManifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returnManifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Complete([FromBody] ReturnManifest returnManifest)
        {
            try
            {
                return await _returnManifestLogic.Complete(returnManifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returnManifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}
