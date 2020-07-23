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
    public class IQCLogic : BaseCode
    {
        internal async Task<JsonResult> GRNListForIQC(int locationid)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_grn_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", locationid);
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

        internal async Task<JsonResult> IQCData(IQC grn_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_start_info_by_grn_no", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_no", grn_no.GRNHeaderNo);
                smd.Parameters.Add("@iqcheaderOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@iqc_lines", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string iqcheader = smd.Parameters["@iqcheaderOutput"].Value.ToString();
                string iqc_lines = smd.Parameters["@iqc_lines"].Value.ToString();
                smd.Dispose();
                List<IQCResponse> response = new List<IQCResponse>();
                IQCResponse data = new IQCResponse()
                {
                    Header = JArray.Parse(iqcheader),
                    Lines = JArray.Parse(iqc_lines)
                };
                response.Add(data);
                return new JsonResult(response);
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

        internal async Task<int> IQCBinScan(IQC binscan)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand da = new SqlCommand("SELECT count(1) FROM [dbo].[bin_mst] bm WHERE [bm].[bincode] = '" + binscan.Bincode + "' AND [bm].[location] = " + binscan.LocationId + " AND [bm].[bin_type] = 'REJECT'", cn);

                //da.Parameters.Add("@bincode", SqlDbType.NVarChar,50);
                //da.Parameters["@bincode"].Value = binscan.Bincode;
                //da.Parameters.Add("@location_id", SqlDbType.Int);
                //da.Parameters["@location_id"].Value = binscan.LocationId;

                object count = await da.ExecuteScalarAsync().ConfigureAwait(false);
                return Convert.ToInt16(count.ToString());
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

        internal async Task<JsonResult> IQCDeleteLine(IQC delete)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_line_delete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_line_no", delete.GRNLineNo);
                smd.Parameters.AddWithValue("@grn_no", delete.GRNHeaderNo);
                smd.Parameters.AddWithValue("@rejection_reason", delete.RejectionReason);
                smd.Parameters.AddWithValue("@barcode", delete.Barcode);
                smd.Parameters.AddWithValue("@bincode", delete.Bincode);
                smd.Parameters.AddWithValue("@quantity", delete.Quantity);
                smd.Parameters.AddWithValue("@vendor_lot_no", delete.VendorLotNo);
                smd.Parameters.AddWithValue("@expire_date", delete.ExpireDate);
                smd.Parameters.Add("@iqcheaderOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@iqc_lines", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string iqcheader = smd.Parameters["@iqcheaderOutput"].Value.ToString();
                string iqc_lines = smd.Parameters["@iqc_lines"].Value.ToString();
                smd.Dispose();
                List<IQCResponse> response = new List<IQCResponse>();
                IQCResponse data = new IQCResponse()
                {
                    Header = JArray.Parse(iqcheader),
                    Lines = JArray.Parse(iqc_lines)
                };
                response.Add(data);
                return new JsonResult(response);
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

        internal async Task<JsonResult> GRNScannedBarcodeInfo(IQC grn_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_all_barcode_by_grn_no", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_no", grn_no.GRNHeaderNo);
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

        internal async Task<JsonResult> IQCScanBarcode(IQC iQC)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_scan", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@barcode", iQC.Barcode);
                smd.Parameters.AddWithValue("@grn_no", iQC.GRNHeaderNo);
                smd.Parameters.AddWithValue("@bincode", iQC.Bincode);
                smd.Parameters.AddWithValue("@location_id", iQC.LocationId);
                smd.Parameters.AddWithValue("@rejection_reason", iQC.RejectionReason);
                smd.Parameters.Add("@iqcheaderOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@iqc_lines", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string iqcheader = smd.Parameters["@iqcheaderOutput"].Value.ToString();
                string iqc_lines = smd.Parameters["@iqc_lines"].Value.ToString();
                smd.Dispose();
                List<IQCResponse> response = new List<IQCResponse>();
                IQCResponse data = new IQCResponse()
                {
                    Header = JArray.Parse(iqcheader),
                    Lines = JArray.Parse(iqc_lines)
                };
                response.Add(data);
                return new JsonResult(response);
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

        internal async Task<JsonResult> IQCComplete(IQC iQC)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("iqc_complete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@grn_no", iQC.GRNHeaderNo);
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
