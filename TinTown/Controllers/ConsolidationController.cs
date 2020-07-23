using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConsolidationController : ControllerBase
    {
        private readonly ConsolidationLogic _consolidationLogic;
        public ConsolidationController()
        {
            _consolidationLogic = new ConsolidationLogic();
        }

        [HttpGet]
        public async Task<JsonResult> ZoneList([FromQuery] string email = "", int location_id = 0)
        {
            try
            {
                return await _consolidationLogic.ZoneList(email, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanBarcode([FromBody] Consolidation scandata)
        {
            try
            {
                return await _consolidationLogic.ScanBarcode(scandata).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanSlot([FromBody] Consolidation slotdata)
        {
            try
            {
                return await _consolidationLogic.ScanSlot(slotdata).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BoxChange([FromBody] Consolidation boxdata)
        {
            try
            {
                return await _consolidationLogic.BoxChange(boxdata).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOrderDetail([FromQuery] string order_no)
        {
            try
            {
                return await _consolidationLogic.getOrderDetail(order_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {

                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RangeChange([FromBody] OQCRange check)
        {
            try
            {
                return await _consolidationLogic.RangeChange(check).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> HoldSlot([FromQuery] string email)
        {
            try
            {
                return await _consolidationLogic.HoldSlot(email).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _consolidationLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}
