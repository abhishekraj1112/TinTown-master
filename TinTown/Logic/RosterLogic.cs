using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class RosterLogic : BaseCode
    {
        internal async Task<ReturnMessage> CreateRoster(Roster newRoster)
        {
            SqlConnection cn = null;
            try
            {
                List<SendResponse> SignalRResponse = new List<SendResponse>();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("roster_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@movementOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                smd.Parameters.AddWithValue("@roster_line", ToDataTable(newRoster.Line));
                smd.Parameters.AddWithValue("@roster_header", ToDataTable(newRoster.Header));
                if (newRoster.movement.Count > 0)
                {
                    smd.Parameters.AddWithValue("@roster_movement", ToDataTable(newRoster.movement));
                }
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                string Signalrjson = smd.Parameters["@movementOutput"].Value.ToString();
                smd.Dispose();

                if (Signalrjson.Length > 1)
                {
                    SignalRResponse = JsonConvert.DeserializeObject<List<SendResponse>>(Signalrjson);
                }
                JArray arr = JArray.Parse(json);

                return new ReturnMessage() { data = arr, sendResponse = SignalRResponse };
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

        internal async Task<JsonResult> ManagerData(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("roster_manager_data", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@location_id", location_id);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);

                // Get the values
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

        internal async Task<JsonResult> AllRoster(string email, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("roster_details", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@location_id", location_id);
                smd.Parameters.AddWithValue("@email", email);

                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);

                // Get the values
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

        internal async Task<JsonResult> PickerInZone(string zone, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("roster_pick_no_zone", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@location_id", location_id);
                smd.Parameters.AddWithValue("@zone", zone);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
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
