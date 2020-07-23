//using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PartialController : ControllerBase
    {
        private readonly PartialLogic _partialLogic;
        public PartialController()
        {
            _partialLogic = new PartialLogic();
        }

        [HttpPost]
        public async Task<JsonResult> ScanOrderNo([FromBody] Partial partial)
        {
            try
            {
                return await _partialLogic.ScanOrderNo(partial).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ScanSlot([FromBody] Partial partial)
        {
            try
            {
                return await _partialLogic.ScanSlot(partial).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFocefullyFlushOutScanOrderNo([FromBody] Partial check)
        {
            try
            {
                List<MultiOQCLines> mlines = new List<MultiOQCLines>();

                DataSet ds = await _partialLogic.Get_focefully_flush_out_scan_order_no(check).ConfigureAwait(false);

                if (ds.Tables.Count > 1)
                {
                    mlines = _partialLogic.DataTableToList<MultiOQCLines>(ds.Tables[1]);
                    for (int i = 0; i < mlines.Count; i++)
                    {
                        mlines[i].images = await _partialLogic.GetImage(mlines[i].barcode).ConfigureAwait(false);
                    }
                }

                MultiOQCResponse multiOQCResponse = new MultiOQCResponse()
                {
                    header = ds.Tables[0],
                    lines = mlines
                };

                List<MultiOQCResponse> returnresponse = new List<MultiOQCResponse>() { multiOQCResponse };

                return new JsonResult(returnresponse);
            }
            catch (Exception ee)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("condition");
                dt.Columns.Add("message");
                DataRow excpe = dt.NewRow();
                excpe["condition"] = "False";
                excpe["message"] = ee.Message;
                dt.Rows.Add(excpe);

                MultiOQCResponse multiOQCResponse = new MultiOQCResponse() { header = dt };

                List<MultiOQCResponse> returnresponse = new List<MultiOQCResponse>() { multiOQCResponse };

                await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);

                return new JsonResult(returnresponse);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ReleaseHold([FromBody] Partial rhold)
        {
            try
            {
                return await _partialLogic.ReleaseHold(rhold).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForcefullyFlushOutPostOQC([FromBody] MultiOQCPost check)
        {
            try
            {
                JArray jArray = await _partialLogic.ForcefullyFlushOutPostOQCPostOQC(check).ConfigureAwait(false);
                if (jArray.Count > 0 && jArray.First["condition"].ToString() == "True" && jArray.First["report"].ToString() == "CompleteInvoice")
                {
                    return await InvoiceReport(jArray[0]["order"].ToString()).ConfigureAwait(false);
                }
                else if (jArray.Count > 0 && jArray.First["condition"].ToString() == "True" && jArray.First["report"].ToString() == "Marketplace")
                {
                    return await OrderReportGet(jArray[0]["order"].ToString()).ConfigureAwait(false);
                }

                return new JsonResult(jArray);
            }
            catch (Exception ee)
            {
                return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        public async Task<IActionResult> InvoiceReport(string sale_no, string action = "1")
        {
            //string fileUrl = "";
            //try
            //{
            //    DataTable dt = new OutboundQualityCheckLogic().ReportSection(sale_no, action);
            //    if (dt.Rows.Count > 0 && dt.Rows[0]["condition"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
            //    {
            //        XtraReport report = XtraReport.FromFile("Reports/InvoiceReport.repx");
            //        report.ExportOptions.Pdf.Compressed = true;
            //        report.DataSource = dt;
            //        report.DataMember = dt.TableName;

            //        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports")))
            //        {
            //            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports"));
            //        }

            //        fileUrl = Directory.GetCurrentDirectory() + "/wwwroot/Reports/" + sale_no.Replace("/", "_") + ".pdf";
            //        if (System.IO.File.Exists(fileUrl))
            //        {
            //            System.IO.File.Delete(fileUrl);
            //        }
            //        new PdfStreamingExporter(report, true).Export(fileUrl);
            //        Response.Headers.Add("filename", sale_no.Replace("/", "_") + ".pdf");
            //        return File(System.IO.File.ReadAllBytes(fileUrl), "application/pdf", sale_no.Replace("/", "_") + ".pdf");
            //    }
            //    else if (dt.Rows.Count > 0)
            //    {
            //        return new JsonResult(dt);
            //    }

            //    return await _partialLogic.SendRespose("True", "Fail get Get Data.").ConfigureAwait(false);

            //}
            //catch (Exception ee)
            //{
            //    return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}

            return await _partialLogic.SendRespose("False", "").ConfigureAwait(false);
        }

        public async Task<IActionResult> OrderReportGet(string order_no)
        {
            //string fileUrl = "";
            //try
            //{
            //    DataTable dt = new OutboundQualityCheckLogic().PartialOrderReportSection(order_no);
            //    if (dt.Rows.Count > 0)
            //    {
            //        XtraReport report = XtraReport.FromFile("Reports/PartialOrderReport.repx");
            //        report.ExportOptions.Pdf.Compressed = true;
            //        report.DataSource = dt;
            //        report.DataMember = dt.TableName;
            //        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports")))
            //        {
            //            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports"));
            //        }
            //        fileUrl = Directory.GetCurrentDirectory() + "/wwwroot/Reports/" + order_no.Replace("/", "_") + ".pdf";
            //        if (System.IO.File.Exists(fileUrl))
            //        {
            //            System.IO.File.Delete(fileUrl);
            //        }
            //        new PdfStreamingExporter(report, true).Export(fileUrl);
            //        Response.Headers.Add("filename", order_no.Replace("/", "_") + ".pdf");
            //        return File(System.IO.File.ReadAllBytes(fileUrl), "application/pdf", order_no.Replace("/", "_") + ".pdf");
            //    }
            //    return await _partialLogic.SendRespose("True", "Fail get Get Data.").ConfigureAwait(false);
            //}
            //catch (Exception ee)
            //{
            //    return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}

            return await _partialLogic.SendRespose("False", "").ConfigureAwait(false);

        }

        [HttpGet]
        public async Task<JsonResult> PartialOrderInfo([FromQuery] string order_no)
        {
            try
            {
                return await _partialLogic.PartialOrderInfo(order_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _partialLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}