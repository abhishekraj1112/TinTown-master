using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class PartialLogic : BaseCode
    {

        internal async Task<JsonResult> ScanOrderNo(Partial partial)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("partial_oqc_put_in_slot", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", partial.OrderNo);
                da.SelectCommand.Parameters.AddWithValue("@email_id", partial.EmailId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return new JsonResult(dt);
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

        internal async Task<JsonResult> ScanSlot(Partial partial)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                DataTable dt = new DataTable();
                using (SqlCommand smd = new SqlCommand("partial_slot_scan", cn))
                {
                    smd.CommandType = System.Data.CommandType.StoredProcedure;
                    smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    smd.Parameters.AddWithValue("@order_no", partial.OrderNo);
                    smd.Parameters.AddWithValue("@slot_id", partial.SlotId);
                    smd.Parameters.AddWithValue("@email_id", partial.EmailId);
                    await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    string json = smd.Parameters["@jsonOutput"].Value.ToString();
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
                }
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

        internal async Task<DataSet> Get_focefully_flush_out_scan_order_no(Partial check)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("focefully_flush_out_scan_order_no", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", check.OrderNo);
                da.SelectCommand.Parameters.AddWithValue("@email_id", check.EmailId);
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
        internal async Task<JsonResult> ReleaseHold(Partial rhold)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand(@"UPDATE ss SET  ss.flusher_id=NULL, ss.under_sorting=11  FROM dbo.sorting_slot ss WHERE ss.slot_id=@slot_id AND ss.order_no=@order_no", cn);
                smd.Parameters.AddWithValue("@slot_id", rhold.SlotId);
                smd.Parameters.AddWithValue("@order_no", rhold.OrderNo);
                int result = await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                if (result > 0)
                {
                    return await SendRespose("True", "Successfull Release Oreder.").ConfigureAwait(false);
                }
                else
                {
                    return await SendRespose("False", "Not Release Oreder.").ConfigureAwait(false);
                }
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

        internal async Task<JArray> ForcefullyFlushOutPostOQCPostOQC(MultiOQCPost check)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                DataTable dt = new DataTable();
                using (SqlCommand smd = new SqlCommand("forcefully_flush_out_post", cn))
                {
                    smd.CommandType = System.Data.CommandType.StoredProcedure;
                    smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    smd.Parameters.AddWithValue("@oqc_list", ToDataTable(check.lines));
                    smd.Parameters.AddWithValue("@email_id", check.EmailId);
                    smd.Parameters.AddWithValue("@order_no", check.OrderNo);
                    smd.Parameters.AddWithValue("@weight", check.Weight);
                    smd.Parameters.AddWithValue("@packing_material", check.PackingMaterial);
                    await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    string json = smd.Parameters["@jsonOutput"].Value.ToString();
                    return JArray.Parse(json);
                }
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

        internal async Task<JsonResult> PartialOrderInfo(string order_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                DataTable dt = new DataTable();
                using (SqlCommand smd = new SqlCommand("partial_order_info", cn))
                {
                    smd.CommandType = System.Data.CommandType.StoredProcedure;
                    smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    smd.Parameters.AddWithValue("@order_no", order_no);
                    await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    string json = smd.Parameters["@jsonOutput"].Value.ToString();
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
                }
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
