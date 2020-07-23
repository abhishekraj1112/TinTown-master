using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PickManualController : ControllerBase
    {
        private readonly PickManualLogic _pickManualLogic;
        public PickManualController()
        {
            _pickManualLogic = new PickManualLogic();
        }

        [HttpPost]
        public async Task<JsonResult> PickList([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.PickList(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickerListByPick([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.PickerListByPick(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickManualInfo([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.PickManualInfo(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PickStart([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.PickStart(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Scanning([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.Scanning(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpPost]
        public async Task<JsonResult> NotFound([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.NotFound(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Hold([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.Hold(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Complete([FromBody] PickManual pickManual)
        {
            try
            {
                return await _pickManualLogic.Complete(pickManual).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _pickManualLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
