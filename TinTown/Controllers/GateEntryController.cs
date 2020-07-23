using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GateEntryController : ControllerBase
    {
        private readonly GateEntryLogic _gateEntryLogic;
        public GateEntryController()
        {
            _gateEntryLogic = new GateEntryLogic();
        }

        [HttpGet]
        public async Task<JsonResult> AllGateEntryList([FromQuery] int location)
        {
            try
            {
                return await _gateEntryLogic.AllGateEntryList(location).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gateEntryLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GateEntryInfoByid([FromQuery] string gateentry_no)
        {
            try
            {
                return await _gateEntryLogic.GateEntryInfoByid(gateentry_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gateEntryLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateGateEntry([FromBody] GateEntry gateEntry)
        {
            try
            {
                return await _gateEntryLogic.CreateGateEntry(gateEntry).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gateEntryLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}