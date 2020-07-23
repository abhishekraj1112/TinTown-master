using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PurchaseReturnOrderController : ControllerBase
    {
        private readonly PurchaseReturnOrderLogic _purchaseReturnOrderLogic;

        public PurchaseReturnOrderController()
        {
            _purchaseReturnOrderLogic = new PurchaseReturnOrderLogic();
        }

        [HttpPost]
        public async Task<JsonResult> PROList([FromBody] PurchaseReturnOrder pro)
        {
            try
            {
                return await _purchaseReturnOrderLogic.PROList(pro).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewPRONo([FromBody] PurchaseReturnOrder pro)
        {
            try
            {
                return await _purchaseReturnOrderLogic.NewPRONo(pro).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddItem([FromBody] PurchaseReturnOrder pro)
        {
            try
            {
                try
                {
                    if (string.IsNullOrEmpty(pro.ItemNo))
                    {
                        return await _purchaseReturnOrderLogic.SendRespose("False", "Blank Item No").ConfigureAwait(false);
                    }
                    JArray item_info = await _purchaseReturnOrderLogic.AddItem(pro).ConfigureAwait(false);

                    JObject jo = item_info.Children<JObject>().FirstOrDefault(o => o["condition"] != null);
                    if (jo.Value<string>("condition") == "True")
                    {
                        decimal cost_per_unit, gst_percentage;
                        cost_per_unit = jo.Value<decimal>("cost_per_unit");
                        gst_percentage = jo.Value<decimal>("gst_percentage");
                        jo.Add(new JProperty("quantity", pro.Quantity));
                        jo.Add(new JProperty("amount_without_tax", decimal.Round((cost_per_unit * pro.Quantity), 2)));
                        jo.Add(new JProperty("gst_amount", decimal.Round((jo.Value<decimal>("amount_without_tax") * gst_percentage) / 100, 2)));
                        jo.Add(new JProperty("amount_with_tax", decimal.Round(jo.Value<decimal>("amount_without_tax") + jo.Value<decimal>("gst_amount"), 2)));
                        JArray response = new JArray(jo);
                        return new JsonResult(response);
                    }
                    else
                    {
                        return new JsonResult(item_info);
                    }
                }
                catch (Exception ee)
                {
                    return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Complete([FromBody] PurchaseReturnOrderComplete pro)
        {
            try
            {
                return await _purchaseReturnOrderLogic.Complete(pro).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PurchaseReturnOrderInfo([FromBody] PurchaseReturnOrderComplete info)
        {
            try
            {
                return await _purchaseReturnOrderLogic.PurchaseReturnOrderInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseReturnOrderLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
