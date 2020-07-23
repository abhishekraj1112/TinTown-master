using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;

namespace TinTown.Logic
{
    public class SignalRLogic : BaseCode
    {
        internal async Task<DataTable> Signalr_access(string flag, string key, string connectionId)
        {
            SqlConnection cn = null;
            SqlCommand smd = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                smd = new SqlCommand("signalr_manage_connection", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@type", flag);
                smd.Parameters.AddWithValue("@email_id", key);
                smd.Parameters.AddWithValue("@connectionid", connectionId);
                SqlDataAdapter da = new SqlDataAdapter(smd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                smd.Dispose();
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<List<string>> Signalr_access(List<string> email)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("signalr_connection_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.AddWithValue("@email_id", ConvertListToDataTable(email));

                List<string> response = new List<string>();

                using (SqlDataReader reader = await smd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        response.Add(reader["connectionid"].ToString());
                    }
                }
                smd.Dispose();
                return response;

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

        internal async Task<JsonResult> AllNotification(string email)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("signalr_all_notification", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email_id", email);
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
