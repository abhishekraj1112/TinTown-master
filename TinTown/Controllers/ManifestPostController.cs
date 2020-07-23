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
    public class ManifestPostController : ControllerBase
    {
        private readonly ManifestLogic _manifestLogic;
        public ManifestPostController()
        {
            _manifestLogic = new ManifestLogic();
        }

        [HttpGet]
        public async Task<JsonResult> shipment_partner()
        {
            try
            {
                return await _manifestLogic.shipment_partner().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> manifest_create([FromBody] Manifest manifest)
        {
            try
            {
                return await _manifestLogic.manifest_create(manifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> manifest_mark_release(Manifest manifest)
        {
            try
            {
                return await _manifestLogic.manifest_mark_release(manifest).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> excel_ReportManifestPost([FromQuery] int location_id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await _manifestLogic.get_ReportManifestPost(location_id).ConfigureAwait(false);
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

        // manifetspost posted hit by user
        [HttpPost]
        public async Task<JsonResult> manifest_mark_Created(Manifest manifest)
        {
            try
            {
                return await _manifestLogic.manifest_mark_created(manifest);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message);
            }

        }
    }
}