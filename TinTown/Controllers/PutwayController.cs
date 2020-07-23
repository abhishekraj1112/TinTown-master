using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PutwayController : ControllerBase
    {
        private readonly PutwayLogic _putwayLogic;

        public PutwayController()
        {
            _putwayLogic = new PutwayLogic();
        }

        [HttpGet]
        public async Task<JsonResult> PutwayList([FromQuery] int locationid)
        {
            try
            {
                return await _putwayLogic.PutwayList(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayGRNList([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayGRNList(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayHeaderCreate([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayHeaderCreate(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayData([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayData(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayLineinfo([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayLineinfo(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayBarcodeinfo([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayBarcodeinfo(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayWithoutScanBarcode([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayWithoutScanBarcode(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayScanBarcode([FromBody] Putway putway)
        {
            try
            {
                return await _putwayLogic.PutwayScanBarcode(putway).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayDeleteLine([FromBody] List<Delete> delete)
        {
            try
            {
                return await _putwayLogic.PutwayDeleteLine(delete).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PutwayComplete([FromBody] Putway complete)
        {
            try
            {
                return await _putwayLogic.PutwayComplete(complete).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _putwayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}