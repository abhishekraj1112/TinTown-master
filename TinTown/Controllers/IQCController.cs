using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IQCController : ControllerBase
    {
        private readonly IQCLogic _iQCLogic;
        public IQCController()
        {
            _iQCLogic = new IQCLogic();
        }

        [HttpGet]
        public async Task<JsonResult> GRNListForIQC([FromQuery] int locationid)
        {
            try
            {
                return await _iQCLogic.GRNListForIQC(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IQCData([FromBody] IQC grn_no)
        {
            try
            {
                return await _iQCLogic.IQCData(grn_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GRNScannedBarcodeInfo([FromBody] IQC grn_no)
        {
            try
            {
                return await _iQCLogic.GRNScannedBarcodeInfo(grn_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IQCBinScan([FromBody] IQC binscan)
        {
            try
            {
                int count = await _iQCLogic.IQCBinScan(binscan).ConfigureAwait(false);
                if (count > 0)
                {
                    return await _iQCLogic.SendRespose("True", "Bin found in reject").ConfigureAwait(false);
                }
                else
                {
                    return await _iQCLogic.SendRespose("False", binscan.Bincode + " Not found in reject bintype").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IQCScanBarcode([FromBody] IQC iQC)
        {
            try
            {
                return await _iQCLogic.IQCScanBarcode(iQC).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IQCDeleteLine([FromBody] IQC delete)
        {
            try
            {
                return await _iQCLogic.IQCDeleteLine(delete).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> IQCComplete([FromBody] IQC iQC)
        {
            try
            {
                return await _iQCLogic.IQCComplete(iQC).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _iQCLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}