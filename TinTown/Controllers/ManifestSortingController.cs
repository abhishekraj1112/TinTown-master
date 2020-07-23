using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManifestSortingController : ControllerBase
    {
        private readonly ManifestLogic _manifestLogic;
        public ManifestSortingController()
        {
            _manifestLogic = new ManifestLogic();
        }

        [HttpGet]
        public async Task<JsonResult> GetLastScanAwbNo([FromQuery] string created_by)
        {
            try
            {
                DataTable dt = await _manifestLogic.GetLastScanAwbNo(created_by).ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _manifestLogic.SendRespose("False", "No Record Found").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanAwbNoManifestCreate([FromBody] Post_Manifest post_Manifest)
        {
            try
            {
                return await _manifestLogic.ScanAwbNoManifestCreate(post_Manifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}