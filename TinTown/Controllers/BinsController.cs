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
    public class BinsController : ControllerBase
    {
        private readonly BinLogic _binLogic;

        public BinsController()
        {
            _binLogic = new BinLogic();
        }

        [HttpGet]
        public async Task<JsonResult> BinList([FromQuery] int locationid)
        {
            try
            {
                return await _binLogic.BinList(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _binLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BinInfo([FromBody] Bin bin)
        {
            try
            {
                return await _binLogic.BinInfo(bin).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _binLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddBin([FromBody] Bin bin)
        {
            try
            {
                return await _binLogic.AddBin(bin).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _binLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBin([FromBody] Bin bin)
        {
            try
            {
                return await _binLogic.DeleteBin(bin).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _binLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpPost]
        public async Task<JsonResult> BarcodeInBin([FromBody] Bin bin)
        {
            try
            {
                DataTable dt = _binLogic.BarcodeInBin(bin);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                return await _binLogic.SendRespose("False", "No barcode Found").ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _binLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
