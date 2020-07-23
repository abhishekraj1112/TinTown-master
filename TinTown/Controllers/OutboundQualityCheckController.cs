//using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OutboundQualityCheckController : ControllerBase
    {
        private readonly OutboundQualityCheckLogic _outboundQualityCheckLogic;

        public OutboundQualityCheckController()
        {
            _outboundQualityCheckLogic = new OutboundQualityCheckLogic();
        }

        [HttpGet]
        public async Task<JsonResult> MarketPlace()
        {
            try
            {
                ShipmentAgent imageResponse = new ShipmentAgent();
                DataTable dt = await _outboundQualityCheckLogic.MarketPlace().ConfigureAwait(false);
                if (dt.Rows.Count > 0)
                {
                    string[] domain_username = dt.Rows[0]["user_name"].ToString().Split("\\");

                    HttpClientHandler handler = new HttpClientHandler();

                    NetworkCredential credential = new NetworkCredential(domain_username[1], dt.Rows[0]["password"].ToString(), domain_username[0]);
                    handler.AllowAutoRedirect = true;

                    CredentialCache ccache = new CredentialCache
                    {
                        {
                            new Uri(dt.Rows[0]["url"].ToString())
                            , "NTLM"
                            , credential
                        }
                    };

                    handler.Credentials = ccache;

                    HttpClient client = new HttpClient(handler)
                    {
                        BaseAddress = new Uri(dt.Rows[0]["url"].ToString())
                    };

                    Uri uri = new Uri(dt.Rows[0]["url"].ToString(), UriKind.Absolute);

                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        imageResponse = JsonConvert.DeserializeObject<ShipmentAgent>(apiResponse);
                    }
                }
                if (imageResponse.value.Count > 0)
                {
                    return new JsonResult(imageResponse.value);
                }
                return await _outboundQualityCheckLogic.SendRespose("False", "No Marketplace found").ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SingleOrderOQC([FromBody] OutboundQualityCheck shipData)
        {
            try
            {
                OQCResponse qCResponse = await _outboundQualityCheckLogic.SingleOrderOQC(shipData).ConfigureAwait(false);

                if (qCResponse.barcode != null)
                {
                    qCResponse.images = await _outboundQualityCheckLogic.GetImage(qCResponse.barcode).ConfigureAwait(false);
                }
                List<OQCResponse> loQCResponses = new List<OQCResponse>()
                {
                      qCResponse
                };
                return new JsonResult(loQCResponses);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SingleOrderOQCPost([FromBody] OQCPost oqcpost)
        {
            try
            {
                DataTable dt = await _outboundQualityCheckLogic.SingleOrderOQCPost(oqcpost).ConfigureAwait(false);
                if (dt.Rows.Count > 0 && dt.Columns.Contains("report"))
                {
                    if (dt.Rows[0]["report"].ToString() == "CompleteInvoice")
                    {
                        return await InvoiceReport(dt.Rows[0]["order"].ToString()).ConfigureAwait(false);
                    }
                    else if (dt.Rows[0]["report"].ToString() == "Marketplace")
                    {
                        return await OrderReport(dt.Rows[0]["order"].ToString()).ConfigureAwait(false);
                    }
                }
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ReleaseHold([FromBody] OutboundQualityCheck rhold)
        {
            try
            {
                return await _outboundQualityCheckLogic.ReleaseHold(rhold).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetSlotForOQC([FromBody] OutboundQualityCheck check)
        {
            try
            {
                List<MultiOQCLines> mlines = new List<MultiOQCLines>();

                DataSet ds = await _outboundQualityCheckLogic.GetSlotForOQC(check).ConfigureAwait(false);

                if (ds.Tables.Count > 1)
                {
                    mlines = _outboundQualityCheckLogic.DataTableToList<MultiOQCLines>(ds.Tables[1]);
                    for (int i = 0; i < mlines.Count; i++)
                    {
                        mlines[i].images = await _outboundQualityCheckLogic.GetImage(mlines[i].barcode).ConfigureAwait(false);
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

                await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);

                return new JsonResult(returnresponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostOQC([FromBody] MultiOQCPost check)
        {
            try
            {
                DataTable dt = await _outboundQualityCheckLogic.PostOQC(check).ConfigureAwait(false);
                if (dt.Rows.Count > 0 && dt.Columns.Contains("report"))
                {
                    if (dt.Rows[0]["report"].ToString() == "CompleteInvoice")
                    {
                        return await InvoiceReport(dt.Rows[0]["order"].ToString()).ConfigureAwait(false);
                    }
                    else if (dt.Rows[0]["report"].ToString() == "OrderNo")
                    {
                        return await OrderReport(check.OrderNo).ConfigureAwait(false);
                    }
                    else if (dt.Rows[0]["report"].ToString() == "Marketplace")
                    {
                        return await OrderReport(dt.Rows[0]["order"].ToString()).ConfigureAwait(false);
                    }
                }
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Report([FromQuery] string order_no, string action = "")
        {
            return await InvoiceReport(order_no, action).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<IActionResult> OrderReport([FromQuery] string order_no)
        {
            return await OrderReportGet(order_no).ConfigureAwait(false);
        }

        public async Task<IActionResult> InvoiceReport(string sale_no, string action = "")
        {
            //string fileUrl = "";
            //try
            //{
            //    DataTable dt = _outboundQualityCheckLogic.ReportSection(sale_no, action);
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

            //    return await _outboundQualityCheckLogic.SendRespose("True", "Fail get Get Data.").ConfigureAwait(false);

            //}
            //catch (Exception ee)
            //{
            //    return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}

            return await _outboundQualityCheckLogic.SendRespose("False", "").ConfigureAwait(false);
        }

        public async Task<IActionResult> OrderReportGet(string order_no)
        {
            //string fileUrl = "";
            //try
            //{
            //    DataTable dt = _outboundQualityCheckLogic.PartialOrderReportSection(order_no);
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
            //    return await _outboundQualityCheckLogic.SendRespose("True", "No Report Needed").ConfigureAwait(false);
            //}
            //catch (Exception ee)
            //{
            //    return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}

            return await _outboundQualityCheckLogic.SendRespose("False", "").ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> RangeChange([FromBody] OQCRange check)
        {
            try
            {
                return await _outboundQualityCheckLogic.RangeChange(check).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> OQCCancelOrder([FromBody] MultiOQCPost cancel)
        {
            try
            {
                DataTable dt = _outboundQualityCheckLogic.OQCCancelOrder(cancel);
                if (dt.Rows.Count > 0 && dt.Columns.Contains("report"))
                {
                    return await OrderReport(cancel.OrderNo).ConfigureAwait(false);
                }
                return new JsonResult(dt);
            }
            catch (Exception ee)
            {
                return await _outboundQualityCheckLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}