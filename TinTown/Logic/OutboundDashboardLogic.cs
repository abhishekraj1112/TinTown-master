using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;

namespace TinTown.Logic
{
    public class OutboundDashboardLogic : BaseCode
    {
        internal async Task<JsonResult> dashboard_Data(string order_type)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("report_dashboard_data", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@order_type", order_type);
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
        internal async Task<JsonResult> WaveWiseZoneActivity(string emailid)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("outbound_dashboard_waveZone", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@email_id", emailid);
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
