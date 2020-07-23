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
    public class TransferOrderController : ControllerBase
    {
        private readonly TransferOrderLogic _transferLogic;

        public TransferOrderController()
        {
            _transferLogic = new TransferOrderLogic();
        }

        [HttpPost]
        public async Task<JsonResult> InboundList([FromBody] TransferOrder inboundTransfer)
        {
            try
            {
                return await _transferLogic.InboundList(inboundTransfer).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> NewTransferNo([FromBody] TransferOrder inboundTransfer)
        {
            try
            {
                return await _transferLogic.NewTransferNo(inboundTransfer).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddItem([FromBody] TransferOrder inboundTransfer)
        {
            try
            {
                try
                {
                    if (string.IsNullOrEmpty(inboundTransfer.ItemNo))
                    {
                        return await _transferLogic.SendRespose("False", "Blank Item No").ConfigureAwait(false);
                    }
                    JArray item_info = await _transferLogic.AddItem(inboundTransfer).ConfigureAwait(false);

                    JObject jo = item_info.Children<JObject>().FirstOrDefault(o => o["condition"] != null);
                    if (jo.Value<string>("condition") == "True")
                    {
                        decimal cost_per_unit, gst_percentage;
                        cost_per_unit = jo.Value<decimal>("cost_per_unit");
                        gst_percentage = jo.Value<decimal>("gst_percentage");
                        jo.Add(new JProperty("quantity", inboundTransfer.Quantity));
                        jo.Add(new JProperty("amount_without_tax", decimal.Round((cost_per_unit * inboundTransfer.Quantity), 2)));
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
                    return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Complete([FromBody] TransferComplete inboundTransfer)
        {
            try
            {
                return await _transferLogic.Complete(inboundTransfer).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> TransferOrderInfo([FromBody] TransferComplete info)
        {
            try
            {
                return await _transferLogic.TransferOrderInfo(info).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _transferLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}