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
    public class DSPController : ControllerBase
    {
        private readonly DSPLogic _dspLogic;
        public DSPController()
        {
            _dspLogic = new DSPLogic();
        }

        [HttpGet]
        public async Task<JsonResult> DSPPartnerList([FromQuery] int location, int code = 0)
        {
            try
            {
                return await _dspLogic.DSPPartnerList(location, code).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdateDSP([FromBody] DSP dsp)
        {
            try
            {
                return await _dspLogic.CreateUpdateDSP(dsp).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> DSPServiceList([FromQuery] int location, string dsp_code)
        {
            try
            {
                return await _dspLogic.DSPServiceList(location, dsp_code).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUpdateDSPService([FromBody] DSPService dspservice)
        {
            try
            {
                return await _dspLogic.CreateUpdateDSPService(dspservice).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DspAwb([FromBody] DSP bin)
        {
            try
            {
                DataTable dt = _dspLogic.DspAwb(bin);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                return await _dspLogic.SendRespose("False", "No barcode Found").ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadAWB([FromBody] DSP dspawbno)
        {
            try
            {
                return await _dspLogic.UploadAWB(dspawbno).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _dspLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
