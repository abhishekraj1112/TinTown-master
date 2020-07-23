using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.Logic;
using TinTown.Models;

namespace TinTown.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GRNController : ControllerBase
    {
        private readonly GRNLogic _gRNLogic;

        public GRNController()
        {
            _gRNLogic = new GRNLogic();
        }

        [HttpPost]
        public async Task<JsonResult> GrnActiveDocumentNo([FromBody] SearchDocument searchDocument)
        {
            try
            {
                return await _gRNLogic.GrnActiveDocumentNo(searchDocument).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DocumentInfoForGRN([FromBody] SearchDocument documentinfo)
        {
            try
            {
                return await _gRNLogic.DocumentInfoForGRN(documentinfo).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GateEntryByDocumentNo([FromBody] GRN document_no)
        {
            try
            {
                return await _gRNLogic.GateEntryByDocumentNo(document_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateGRNHeader([FromBody] GRN grn)
        {
            try
            {
                return await _gRNLogic.CreateGRNHeader(grn).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GRNInfo([FromBody] GRN grn_header_no)
        {
            try
            {
                return await _gRNLogic.GRNInfo(grn_header_no).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GRNQuantityInWithoutScan([FromBody] GRN grn)
        {
            try
            {
                DataTable newBarcodes = new DataTable();
                newBarcodes.Columns.Add("Barcode", typeof(string));
                newBarcodes.Columns.Add("Quantity", typeof(int));
                switch (grn.ProcessType.ToLower())
                {
                    case "serial":
                        DataTable serial_dt = await _gRNLogic.GetBarcodeNo(grn, grn.Quantity).ConfigureAwait(false);
                        if (serial_dt.Columns.Count > 0 && serial_dt.Rows[0]["condition"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(serial_dt.Rows[0]["current_value"]) > 1)
                        {
                            long serial_current_no = Convert.ToInt32(serial_dt.Rows[0]["current_value"]);
                            for (int i = 0; i < grn.Quantity; i++)
                            {
                                newBarcodes.Rows.Add(serial_current_no, 1);
                                serial_current_no += 1;
                            }
                        }
                        else
                        {
                            return new JsonResult(serial_dt);
                        }
                        break;
                    case "lot":
                        DataTable lot_dt = await _gRNLogic.GetBarcodeNo(grn, 1).ConfigureAwait(false);
                        if (lot_dt.Columns.Count > 0 && lot_dt.Rows[0]["condition"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) && Convert.ToInt32(lot_dt.Rows[0]["current_value"]) > 1)
                        {
                            long lot_current_no = Convert.ToInt32(lot_dt.Rows[0]["current_value"]);
                            newBarcodes.Rows.Add(lot_current_no, grn.Quantity);
                        }
                        else
                        {
                            return new JsonResult(lot_dt);
                        }
                        break;
                    case "item":
                        newBarcodes.Rows.Add(grn.ItemNo, grn.Quantity);
                        break;
                }
                if (newBarcodes.Rows.Count > 0)
                {
                    return await _gRNLogic.insert_new_barcode(grn, newBarcodes).ConfigureAwait(false);
                }
                else
                {
                    return await _gRNLogic.SendRespose("False", "Unable to genrate barcode").ConfigureAwait(false);
                }

            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GRNQuantityInWithScan([FromBody] GRN grn)
        {
            try
            {
                return await _gRNLogic.GRNQuantityInWithScan(grn).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BarcodeInByPOLineInfo([FromBody] GRN lineinfo)
        {
            try
            {
                return await _gRNLogic.BarcodeInByPOLineInfo(lineinfo).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteScannedBarcode([FromBody] DeleteBarcode deleteline)
        {
            try
            {
                return await _gRNLogic.DeleteScannedBarcode(deleteline).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CompleteGRN([FromBody] GRN grn)
        {
            try
            {
                return await _gRNLogic.CompleteGRN(grn).ConfigureAwait(false);
            }
            catch (Exception ee)
            {
                return await _gRNLogic.SendRespose("False", ee.Message).ConfigureAwait(false);
            }
        }
    }
}