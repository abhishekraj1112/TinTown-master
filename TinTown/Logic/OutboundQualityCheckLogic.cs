using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class OutboundQualityCheckLogic : BaseCode
    {
        internal async Task<DataTable> MarketPlace()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(am.base_url, am.[port], am.action_url) AS [url], am.[user_name], am.[password]  FROM dbo.api_mst am WHERE am.access_type = 'Shipment Agent'", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
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

        internal async Task<OQCResponse> SingleOrderOQC(OutboundQualityCheck shipData)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("oqc_get_data_for_single_quantity", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@barcode", shipData.Barcode);
                smd.Parameters.AddWithValue("@sorting_zone", shipData.SortingZone);
                smd.Parameters.AddWithValue("@marketplace", shipData.Marketplace);
                smd.Parameters.AddWithValue("@location_id", shipData.LocationId);
                smd.Parameters.AddWithValue("@email_id", shipData.EmailID);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                OQCResponse returnResponse = JsonConvert.DeserializeObject<OQCResponse>(json);
                return returnResponse;
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

        internal async Task<DataTable> SingleOrderOQCPost(OQCPost oqcpost)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("oqc_post_data_for_single_quantity", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@pick_no", oqcpost.PickNo);
                da.SelectCommand.Parameters.AddWithValue("@pick_line_no", oqcpost.PickLineNo);
                da.SelectCommand.Parameters.AddWithValue("@good_qty", oqcpost.GoodQty);
                da.SelectCommand.Parameters.AddWithValue("@bad_qty", oqcpost.BadQty);
                da.SelectCommand.Parameters.AddWithValue("@miss_qty", oqcpost.MissingQty);
                da.SelectCommand.Parameters.AddWithValue("@email_id", oqcpost.EmailID);
                da.SelectCommand.Parameters.AddWithValue("@packing_material", oqcpost.PackingMaterial);
                da.SelectCommand.Parameters.AddWithValue("@weight", oqcpost.Weight);
                DataTable dt = new DataTable();
                da.Fill(dt);
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

        internal DataTable ReportSection(string order_no, string action)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("report_invoice", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                da.SelectCommand.Parameters.AddWithValue("@forceful", action == "1" ? 1 : 0);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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

        internal async Task<JsonResult> ReleaseHold(OutboundQualityCheck rhold)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("oqc_release_item", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email_id", rhold.EmailID);
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

        internal async Task<DataSet> GetSlotForOQC(OutboundQualityCheck check)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("oqc_get_slot", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@sorting_zone", check.SortingZone);
                da.SelectCommand.Parameters.AddWithValue("@email_id", check.EmailID);
                da.SelectCommand.Parameters.AddWithValue("@location_id", check.LocationId);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
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

        internal async Task<DataTable> PostOQC(MultiOQCPost check)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("oqc_multi_post", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@oqc_list", ToDataTable(check.lines));
                da.SelectCommand.Parameters.AddWithValue("@email_id", check.EmailId);
                da.SelectCommand.Parameters.AddWithValue("@order_no", check.OrderNo);
                da.SelectCommand.Parameters.AddWithValue("@weight", check.Weight);
                da.SelectCommand.Parameters.AddWithValue("@packing_material", check.PackingMaterial);
                DataTable dt = new DataTable();
                da.Fill(dt);
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

        internal async Task<IActionResult> RangeChange(OQCRange check)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("oqc_range_change", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email_id", check.Email);
                smd.Parameters.AddWithValue("@zone", check.Zone);
                smd.Parameters.AddWithValue("@location_id", check.LocationId);
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

        internal DataTable PartialOrderReportSection(string order_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("partial_order_report", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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

        internal DataTable OQCCancelOrder(MultiOQCPost cancel)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("oqc_cancel_order", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", cancel.OrderNo);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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
    }
}
