using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TinTown.Logic;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OutBoundDashboardController : ControllerBase
    {
        private readonly OutboundDashboardLogic outboundDashboardLogic;
        public OutBoundDashboardController()
        {
            outboundDashboardLogic = new OutboundDashboardLogic();
        }
        [HttpGet]
        public async Task<JsonResult> Dashboard_Data([FromQuery] string order_type)
        {
            try
            {
                return await outboundDashboardLogic.dashboard_Data(order_type).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await outboundDashboardLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
        [HttpGet]
        public async Task<JsonResult> WaveWiseZoneActivity([FromQuery] string emailid)
        {
            try
            {
                return await outboundDashboardLogic.WaveWiseZoneActivity(emailid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await outboundDashboardLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}