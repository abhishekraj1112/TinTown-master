//using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManifestHandoverController : ControllerBase
    {
        private readonly ManifestLogic _manifestLogic;
        public ManifestHandoverController()
        {
            _manifestLogic = new ManifestLogic();
        }

        [HttpGet]
        public async Task<JsonResult> get_handover_ship_agent_code([FromQuery] string ship_agent_code, int location_id)
        {
            try
            {
                DataTable dt = await _manifestLogic.get_handover_ship_agent_code(ship_agent_code, location_id).ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _manifestLogic.SendRespose("False", "No Record Found").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> get_lines_with_ship_agent_code([FromBody] HandoverLineNew handover_Line_New)
        {
            try
            {
                DataTable dt = await _manifestLogic.get_lines_with_ship_agent_code(handover_Line_New).ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    return new JsonResult(dt);
                }
                else
                {
                    return await _manifestLogic.SendRespose("False", "No Record Found").ConfigureAwait(false);
                }
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> handover_create(handover_create handover_Create)
        {
            try
            {
                JArray array = await _manifestLogic.handover_create(handover_Create).ConfigureAwait(false);
                return await print_handover_report(array).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> reprint_handover([FromQuery] string handoverno)
        {
            try
            {
                JArray array = await _manifestLogic.get_reprint_handover(handoverno).ConfigureAwait(false);
                return await print_handover_report(array).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        public async Task<IActionResult> print_handover_report(JArray array)
        {
            //string fileUrl = "";
            //try
            //{
            //    if (array.Count > 0)
            //    {
            //        if (array[0]["condition"].ToString() == "TRUE")
            //        {
            //            XtraReport report = XtraReport.FromFile("Reports/Manifest_handover_Report.repx");
            //            report.ExportOptions.Pdf.Compressed = true;

            //            report.Parameters["Document_No"].Value = array[0]["handover_no"].ToString();
            //            report.Parameters["No_of_shipment"].Value = array.Count().ToString();
            //            report.Parameters["Shipping_partner"].Value = array[0]["ship_agent_code"].ToString();
            //            report.Parameters["Vehical_No"].Value = array[0]["vehicle_no"].ToString();

            //            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports")))
            //            {
            //                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports"));
            //            }

            //            fileUrl = Directory.GetCurrentDirectory() + "/wwwroot/Reports/" + array[0]["handover_no"].ToString().Replace("/", "_") + "_r.pdf";
            //            if (System.IO.File.Exists(fileUrl))
            //            {
            //                System.IO.File.Delete(fileUrl);
            //            }
            //            new PdfStreamingExporter(report, true).Export(fileUrl);
            //            Response.Headers.Add("filename", array[0]["handover_no"].ToString().Replace("/", "_") + "_r.pdf");
            //            return File(System.IO.File.ReadAllBytes(fileUrl), "application/pdf", array[0]["handover_no"].ToString().Replace("/", "_") + "_r.pdf");
            //        }
            //        return new JsonResult(array);
            //    }
            //    else
            //    {
            //        return await _manifestLogic.SendRespose("False", "do not get Data Api Error.").ConfigureAwait(false);
            //    }
            //}
            //catch (Exception ee)
            //{
            //    return await _manifestLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}

            return await _manifestLogic.SendRespose("False", "").ConfigureAwait(false);
        }
    }
}