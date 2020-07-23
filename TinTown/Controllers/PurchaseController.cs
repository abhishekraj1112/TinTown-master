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
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseLogic _purchaseLogic;
        public PurchaseController()
        {
            _purchaseLogic = new PurchaseLogic();
        }

        [HttpGet]
        public async Task<JsonResult> PurchaseOrderlist([FromQuery] int location_id)
        {
            try
            {
                return await _purchaseLogic.PurchaseOrderlist(location_id).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVendorCompleteDetailWithPoNo([FromQuery] string vendor_no)
        {
            try
            {
                if (string.IsNullOrEmpty(vendor_no))
                {
                    return await _purchaseLogic.SendRespose("False", "Blank Vendor No").ConfigureAwait(false);
                }
                return await _purchaseLogic.GetVendorCompleteDetailWithPoNo(vendor_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetVendorItem([FromBody] AddPO vendor)
        {
            try
            {
                if (string.IsNullOrEmpty(vendor.ItemNo))
                {
                    return await _purchaseLogic.SendRespose("False", "Blank Item No").ConfigureAwait(false);
                }
                JArray item_info = await _purchaseLogic.GetVendorItem(vendor).ConfigureAwait(false);

                JObject jo = item_info.Children<JObject>().FirstOrDefault(o => o["condition"] != null);
                if (jo.Value<string>("condition") == "True")
                {
                    decimal cost_per_unit, gst_percentage;
                    cost_per_unit = jo.Value<decimal>("cost_per_unit");
                    gst_percentage = jo.Value<decimal>("gst_percentage");
                    jo.Add(new JProperty("quantity", (vendor.Quantity)));
                    jo.Add(new JProperty("amount", decimal.Round((cost_per_unit * vendor.Quantity), 2)));
                    jo.Add(new JProperty("discount", decimal.Round((jo.Value<decimal>("amount") * vendor.Discount) / 100, 2)));
                    jo.Add(new JProperty("total_amount", decimal.Round(jo.Value<decimal>("amount") - jo.Value<decimal>("discount"), 2)));
                    jo.Add(new JProperty("gst_amount", decimal.Round((jo.Value<decimal>("total_amount") * gst_percentage) / 100, 2)));
                    jo.Add(new JProperty("grand_total", decimal.Round(jo.Value<decimal>("total_amount") + jo.Value<decimal>("gst_amount"), 2)));
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
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PurchaseOrderCreation([FromBody] Purchase purchase)
        {
            try
            {
                return await _purchaseLogic.PurchaseOrderCreation(purchase).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ActivePurchaseOrderByVendor([FromQuery] string vendor_no, int locationid)
        {
            try
            {
                return await _purchaseLogic.ActivePurchaseOrderByVendor(vendor_no, locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> POForApporoval([FromQuery] int locationid)
        {
            try
            {
                return await _purchaseLogic.POForApporoval(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> POApporoved([FromBody] Purchase approval)
        {
            try
            {
                return await _purchaseLogic.POApporoved(approval).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> POInfoForUpdate([FromBody] Purchase infoup)
        {
            try
            {
                return await _purchaseLogic.POInfoForUpdate(infoup).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> POInfo([FromBody] Purchase info)
        {
            try
            {
                return await _purchaseLogic.POInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> POGRNInfo([FromBody] Purchase info)
        {
            try
            {
                return await _purchaseLogic.POGRNInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _purchaseLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}