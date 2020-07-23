using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CageController : ControllerBase
    {
        private readonly CageLogic _cageLogic;
        public CageController()
        {
            _cageLogic = new CageLogic();
        }

        [HttpGet]
        public async Task<JsonResult> CageList([FromQuery] int location_id)
        {
            try
            {
                return await _cageLogic.CageList(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _cageLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddCage([FromBody] Cage cage)
        {
            try
            {
                return await _cageLogic.AddCage(cage).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _cageLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CageListByZone([FromQuery] string zone, int location_id)
        {
            try
            {
                return await _cageLogic.CageListByZone(zone, location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _cageLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}