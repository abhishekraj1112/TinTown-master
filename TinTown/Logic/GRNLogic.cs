using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class GRNLogic : BaseCode
    {
        internal async Task<JsonResult> CreateGRNHeader(GRN grn)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_header_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@external_invoice_no", grn.ExternalInvoiceNo);
                smd.Parameters.AddWithValue("@external_invoice_date", grn.ExternalInvoiceDate);
                smd.Parameters.AddWithValue("@document_no", grn.DocumentNo);
                smd.Parameters.AddWithValue("@document_type", grn.DocumentType);
                smd.Parameters.AddWithValue("@created_by", grn.CreatedBy);
                smd.Parameters.AddWithValue("@gateenrty_no", grn.GateEntryNo);
                smd.Parameters.AddWithValue("@boe_no", grn.BilofEntryNo);
                smd.Parameters.AddWithValue("@boe_amount", grn.BilofEntryAmount);
                smd.Parameters.AddWithValue("@boe_date", grn.BilofEntryDate);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> GateEntryByDocumentNo(GRN document_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_gateentry_no_by_document_no", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@document_no", document_no.DocumentNo);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> GRNInfo(GRN grn_header_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_header_no", grn_header_no.GRNHeaderNo);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> DocumentInfoForGRN(SearchDocument documentinfo)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_document_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", documentinfo.LocationId);
                smd.Parameters.AddWithValue("@document_no", documentinfo.DocumentNo);
                smd.Parameters.AddWithValue("@document_type", documentinfo.DocumentType);
                smd.Parameters.Add("@document_data", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@grn_data", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string document_data = smd.Parameters["@document_data"].Value.ToString();
                string grn_data = smd.Parameters["@grn_data"].Value.ToString();
                smd.Dispose();

                List<DocumentInfoResponse> documentInfoResponse = new List<DocumentInfoResponse>()
                {
                    new DocumentInfoResponse()
                    {
                        DocumentData = JArray.Parse(document_data),
                        GRNData = JArray.Parse(grn_data)
                    }
                };

                return new JsonResult(documentInfoResponse);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> BarcodeInByPOLineInfo(GRN lineinfo)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_scanned_barcode_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_header_no", lineinfo.GRNHeaderNo);
                smd.Parameters.AddWithValue("@document_line_no", lineinfo.DocumentLineNo);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> GrnActiveDocumentNo(SearchDocument searchDocument)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_active_document_no", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", searchDocument.LocationId);
                smd.Parameters.AddWithValue("@filter_value", searchDocument.Filter);
                smd.Parameters.AddWithValue("@document_type", searchDocument.DocumentType);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> DeleteScannedBarcode(DeleteBarcode deleteline)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_delete_barcode", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_header_no", deleteline.GRNHeaderNo);
                smd.Parameters.AddWithValue("@document_line_no", deleteline.DocumentLineNo);
                smd.Parameters.AddWithValue("@barcode_for_delete", ToDataTable(deleteline.lines));
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> GRNQuantityInWithScan(GRN grn)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_with_scanning", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@barcode", grn.Barcode);
                smd.Parameters.AddWithValue("@grn_header_no", grn.GRNHeaderNo);
                smd.Parameters.AddWithValue("@item_no", grn.ItemNo);
                smd.Parameters.AddWithValue("@document_no", grn.DocumentNo);
                smd.Parameters.AddWithValue("@document_line_no", grn.DocumentLineNo);
                smd.Parameters.AddWithValue("@document_type", grn.DocumentType);
                smd.Parameters.AddWithValue("@sales_invoice_no", grn.SalesInvoiceNo);
                smd.Parameters.AddWithValue("@quantity", grn.Quantity);
                smd.Parameters.AddWithValue("@created_by", grn.CreatedBy);
                smd.Parameters.AddWithValue("@vendor_lot_no", grn.VendorLotNo);
                smd.Parameters.AddWithValue("@expire_date", grn.ExpireDate);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> CompleteGRN(GRN grn)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_complete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.AddWithValue("@grn_header_no", grn.GRNHeaderNo);
                smd.Parameters.AddWithValue("@created_by", grn.CreatedBy);
                smd.Parameters.AddWithValue("@document_no", grn.DocumentNo);
                smd.Parameters.AddWithValue("@document_type", grn.DocumentType);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<DataTable> GetBarcodeNo(GRN grn, int no_of_barcode)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlDataAdapter smd = new SqlDataAdapter("get_barcode_sequence", cn);
                smd.SelectCommand.CommandType = CommandType.StoredProcedure;
                smd.SelectCommand.Parameters.AddWithValue("@no_of_barcode", no_of_barcode);
                smd.SelectCommand.Parameters.AddWithValue("@quantity", grn.Quantity);
                smd.SelectCommand.Parameters.AddWithValue("@document_no", grn.DocumentNo);
                smd.SelectCommand.Parameters.AddWithValue("@document_type", grn.DocumentType);
                smd.SelectCommand.Parameters.AddWithValue("@sales_invoice_no", grn.SalesInvoiceNo);
                smd.SelectCommand.Parameters.AddWithValue("@document_line_no", grn.DocumentLineNo);
                smd.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> insert_new_barcode(GRN grn, DataTable newBarcodes)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("grn_without_scanning", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@newbarcode_list", newBarcodes);
                smd.Parameters.AddWithValue("@grn_header_no", grn.GRNHeaderNo);
                smd.Parameters.AddWithValue("@item_no", grn.ItemNo);
                smd.Parameters.AddWithValue("@document_no", grn.DocumentNo);
                smd.Parameters.AddWithValue("@document_line_no", grn.DocumentLineNo);
                smd.Parameters.AddWithValue("@document_type", grn.DocumentType);
                smd.Parameters.AddWithValue("@sales_invoice_no", grn.SalesInvoiceNo);
                smd.Parameters.AddWithValue("@quantity", grn.Quantity);
                smd.Parameters.AddWithValue("@created_by", grn.CreatedBy);
                smd.Parameters.AddWithValue("@vendor_lot_no", grn.VendorLotNo);
                smd.Parameters.AddWithValue("@expire_date", grn.ExpireDate);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }
    }
}
