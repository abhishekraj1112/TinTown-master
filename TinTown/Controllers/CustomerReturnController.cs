using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerReturnController : ControllerBase
    {
        private readonly CustomerReturnLogic _customerReturnLogic;
        public CustomerReturnController()
        {
            _customerReturnLogic = new CustomerReturnLogic();
        }

        [HttpGet]
        public async Task<JsonResult> CRList([FromQuery] int location_id)
        {
            try
            {
                return await _customerReturnLogic.CRList(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _customerReturnLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateCR([FromBody] CustomerReturn newcr)
        {
            try
            {
                return await _customerReturnLogic.CreateCR(newcr).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _customerReturnLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}
