using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftLogic _shiftLogic;
        public ShiftController()
        {
            _shiftLogic = new ShiftLogic();
        }

        [HttpPost]
        public async Task<JsonResult> CreateShift([FromBody] Shift newshift)
        {
            try
            {
                return await _shiftLogic.CreateShift(newshift).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _shiftLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> AllShift(int location_id)
        {
            try
            {
                return await _shiftLogic.AllShift(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _shiftLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CurrentShift([FromQuery] string superviser, int location_id)
        {
            try
            {
                return await _shiftLogic.CurrentShift(superviser, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _shiftLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateShift([FromBody] Shift updateShift)
        {
            try
            {
                return await _shiftLogic.UpdateShift(updateShift).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _shiftLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteShift([FromBody] Shift deleteshift)
        {
            try
            {
                return await _shiftLogic.DeleteShift(deleteshift).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _shiftLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}