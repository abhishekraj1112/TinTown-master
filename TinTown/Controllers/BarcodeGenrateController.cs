//using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BarcodeGenrateController : ControllerBase
    {
        private readonly BarcodeGenrateLogic _barcodeGenrateLogic;

        public BarcodeGenrateController()
        {
            _barcodeGenrateLogic = new BarcodeGenrateLogic();
        }

        [HttpGet]
        public async Task<JsonResult> SeachByStyle([FromQuery] string stylecode)
        {
            try
            {
                string apiResponse = "";
                using (HttpClient httpClient = new HttpClient())
                {
                    string zivame_style_url = Startup._inappuse.GetValue<string>("Modules:ZivameSearchbyStyleUrl:url"); // read url 
                    //httpClient.Timeout = TimeSpan.FromMilliseconds(300);
                    Uri uri = new Uri(zivame_style_url + stylecode, UriKind.Absolute);
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
                JObject arr = JObject.Parse(apiResponse);

                return new JsonResult(arr);
            }
            catch (Exception ee)
            {
                return await _barcodeGenrateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> SeachByStyleAndColor([FromQuery] string stylecode, string colorcode)
        {
            try
            {
                return await _barcodeGenrateLogic.SeachByStyleAndColor(stylecode, colorcode).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _barcodeGenrateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BarcodePrintInfoNAV([FromBody] BarcodeGenrate barcodeGenrate)
        {
            try
            {
                return await _barcodeGenrateLogic.BarcodePrintInfoNAV(barcodeGenrate).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _barcodeGenrateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrintBarcodeReport([FromBody] BarcodeGenrate barcodeGenrate)
        {
            //string fileUrl = "";
            //try
            //{
            //    DataSet ds = await _barcodeGenrateLogic.PrintBarcodeReport(barcodeGenrate).ConfigureAwait(false);
            //    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0]["condition"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
            //    {
            //        XtraReport report = XtraReport.FromFile("Reports/BarcodeReport.repx");
            //        report.ExportOptions.Pdf.Compressed = true;
            //        report.DataSource = ds.Tables[1];
            //        report.DataMember = ds.Tables[1].TableName;

            //        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports")))
            //        {
            //            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports"));
            //        }

            //        fileUrl = Directory.GetCurrentDirectory() + "/wwwroot/Reports/" + barcodeGenrate.Barcode.Replace("/", "_") + ".pdf";
            //        if (System.IO.File.Exists(fileUrl))
            //        {
            //            System.IO.File.Delete(fileUrl);
            //        }
            //        new PdfStreamingExporter(report, true).Export(fileUrl);
            //        Response.Headers.Add("filename", barcodeGenrate.Barcode.Replace("/", "_") + ".pdf");
            //        return File(System.IO.File.ReadAllBytes(fileUrl), "application/pdf", barcodeGenrate.Barcode.Replace("/", "_") + ".pdf");
            //    }
            //    else if (ds.Tables.Count > 1)
            //    {
            //        return new JsonResult(ds.Tables[1]);
            //    }

            //    return await _barcodeGenrateLogic.SendRespose("True", "Fail get Get Data.").ConfigureAwait(false);
            //}
            //catch (Exception ee)
            //{
            //    return await _barcodeGenrateLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            //}
            //finally
            //{
            //    if (System.IO.File.Exists(fileUrl))
            //    {
            //        System.IO.File.Delete(fileUrl);
            //    }
            //}\

            return await _barcodeGenrateLogic.SendRespose("False", "").ConfigureAwait(false);
        }
    }
}