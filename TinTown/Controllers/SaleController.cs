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
    public class SaleController : ControllerBase
    {
        private readonly SaleLogic _saleLogic;

        public SaleController()
        {
            _saleLogic = new SaleLogic();
        }

        [HttpPost]
        public async Task<JsonResult> SaleOrderlist([FromBody] Sale sale)
        {
            try
            {
                return await _saleLogic.SaleOrderlist(sale).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSaleNo()
        {
            try
            {
                return await _saleLogic.GetSaleNo().ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddItem([FromBody] AddSO so)
        {
            try
            {
                if (string.IsNullOrEmpty(so.ItemNo))
                {
                    return await _saleLogic.SendRespose("False", "Blank Item No").ConfigureAwait(false);
                }
                JArray item_info = await _saleLogic.AddItem(so).ConfigureAwait(false);

                JObject jo = item_info.Children<JObject>().FirstOrDefault(o => o["condition"] != null);
                if (jo.Value<string>("condition") == "True")
                {
                    decimal  gst_percentage;
                    gst_percentage = jo.Value<decimal>("gst_percentage");
                    jo.Add(new JProperty("cost_per_unit", (so.CostPerUnit)));
                    jo.Add(new JProperty("quantity", (so.Quantity)));
                    jo.Add(new JProperty("discount_percentage", so.Discount));
                    jo.Add(new JProperty("amount", decimal.Round((jo.Value<decimal>("cost_per_unit") * so.Quantity), 2)));
                    jo.Add(new JProperty("discount", decimal.Round((jo.Value<decimal>("amount") * so.Discount) / 100, 2)));
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
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaleOrderCreation([FromBody] Sale sale)
        {
            try
            {
                return await _saleLogic.SaleOrderCreation(sale).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> SOForApporoval([FromQuery] int locationid)
        {
            try
            {
                return await _saleLogic.SOForApporoval(locationid).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SOApporoved([FromBody] Sale approval)
        {
            try
            {
                return await _saleLogic.SOApporoved(approval).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaleInfo([FromBody] Sale info)
        {
            try
            {
                return await _saleLogic.SaleInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaleInvoiceList([FromBody] Sale info)
        {
            try
            {
                return await _saleLogic.SaleInvoiceList(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaleInvoiceInfo([FromBody] Sale info)
        {
            try
            {
                return await _saleLogic.SaleInvoiceInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _saleLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}
