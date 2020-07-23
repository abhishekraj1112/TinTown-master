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
    public class ReturnGatePassController : ControllerBase
    {
        private readonly ReturnGatePassLogic _returngatepassLogic;

        public ReturnGatePassController()
        {
            _returngatepassLogic = new ReturnGatePassLogic();
        }

        [HttpPost]
        public async Task<JsonResult> RGPList([FromBody] ReturnGatePass returnGatePass)
        {
            try
            {
                return await _returngatepassLogic.RGPList(returnGatePass).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PartyList([FromBody] ReturnGatePass returnGatePass)
        {
            try
            {
                return await _returngatepassLogic.PartyList(returnGatePass).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }


        [HttpPost]
        public async Task<JsonResult> NewRGPNo([FromBody] ReturnGatePass returnGatePass)
        {
            try
            {
                return await _returngatepassLogic.NewRGPNo(returnGatePass).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddItem([FromBody] ReturnGatePass returnGatePass)
        {
            try
            {
                try
                {
                    if (string.IsNullOrEmpty(returnGatePass.ItemNo))
                    {
                        return await _returngatepassLogic.SendRespose("False", "Blank Item No").ConfigureAwait(false);
                    }
                    JArray item_info = await _returngatepassLogic.AddItem(returnGatePass).ConfigureAwait(false);

                    JObject jo = item_info.Children<JObject>().FirstOrDefault(o => o["condition"] != null);
                    if (jo.Value<string>("condition") == "True")
                    {
                        decimal cost_per_unit, gst_percentage;
                        cost_per_unit = jo.Value<decimal>("cost_per_unit");
                        gst_percentage = jo.Value<decimal>("gst_percentage");
                        jo.Add(new JProperty("quantity", returnGatePass.Quantity));
                        jo.Add(new JProperty("amount_without_tax", decimal.Round((cost_per_unit * returnGatePass.Quantity), 2)));
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
                    return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Complete([FromBody] RGPComplete returnGatePass)
        {
            try
            {
                return await _returngatepassLogic.Complete(returnGatePass).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _returngatepassLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

    }
}
