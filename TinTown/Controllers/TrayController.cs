using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TrayController : ControllerBase
    {
        private readonly TrayLogic _trayLogic;
        public TrayController()
        {
            _trayLogic = new TrayLogic();
        }

        [HttpGet]
        public async Task<JsonResult> AllTray([FromQuery] int location_id)
        {
            try
            {
                return await _trayLogic.AllTray(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _trayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewTray([FromBody] Tray tray)
        {
            try
            {
                return await _trayLogic.NewTray(tray).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _trayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTray([FromBody] Tray tray)
        {
            try
            {
                return await _trayLogic.DeleteTray(tray).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _trayLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}