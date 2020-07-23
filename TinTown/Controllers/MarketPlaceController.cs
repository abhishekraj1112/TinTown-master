using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.Logic;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarketPlaceController : ControllerBase
    {
        private readonly MarketPlaceLogic _marketplaceLogic;
        public MarketPlaceController()
        {
            _marketplaceLogic = new MarketPlaceLogic();
        }

        [HttpGet]
        public async Task<JsonResult> MarketPlace_invoice([FromQuery] string order_no)
        {
            try
            {
                order_no = "'" + order_no.Replace("|", "','") + "'";
                DataTable dt = await _marketplaceLogic.MarketPlace_invoice(order_no).ConfigureAwait(false);

                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _marketplaceLogic.SendRespose("False", "Record Not Found.").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _marketplaceLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> MarketPlace_invoice_sync([FromQuery] string order_no)
        {
            try
            {
                return await _marketplaceLogic.MarketPlace_invoice_sync(order_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _marketplaceLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}