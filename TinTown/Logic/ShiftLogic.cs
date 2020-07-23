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
    public class ShiftLogic : BaseCode
    {
        internal async Task<JsonResult> CreateShift(Shift new_shift)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("shift_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                DataTable dt = ConvertListToDataTable(new_shift.supervisor);
                if (dt.Rows.Count > 0)
                {
                    smd.Parameters.AddWithValue("@supervisor_id", dt);
                }
                smd.Parameters.AddWithValue("@name", new_shift.name);
                smd.Parameters.AddWithValue("@start_datetime", new_shift.startDatetime.TimeOfDay);
                smd.Parameters.AddWithValue("@end_datetime", new_shift.endDatetime.TimeOfDay);
                smd.Parameters.AddWithValue("@location_id", new_shift.LocationId);
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

        internal async Task<JsonResult> CurrentShift(string supervisor, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("shift_current", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@supervisor", supervisor);
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

        internal async Task<JsonResult> AllShift(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("shift_details", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", location_id);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

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

        internal async Task<JsonResult> DeleteShift(Shift deleteshift)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("shift_delete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@id", deleteshift.id);

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

        internal async Task<JsonResult> UpdateShift(Shift updateShift)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = ConvertListToDataTable(updateShift.supervisor);
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("shift_update", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@location_id", updateShift.LocationId);
                smd.Parameters.AddWithValue("@id", updateShift.id);
                smd.Parameters.AddWithValue("@start_datetime", updateShift.startDatetime);
                smd.Parameters.AddWithValue("@end_datetime", updateShift.endDatetime);
                if (dt.Rows.Count > 0)
                {
                    smd.Parameters.AddWithValue("@supervisor", dt);
                }
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
