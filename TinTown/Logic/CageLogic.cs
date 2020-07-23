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
    public class CageLogic : BaseCode
    {
        internal async Task<JsonResult> CageList(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("cage_list", cn)
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

        internal async Task<JsonResult> AddCage(Cage cage)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("cage_add", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@cage_id", cage.CageId);
                smd.Parameters.AddWithValue("@location_id", cage.LocationId);
                smd.Parameters.AddWithValue("@consolidation_zone", cage.ConsolidationZone);
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

        internal async Task<JsonResult> CageListByZone(string zone, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("cage_list_by_zone", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@consolidation_zone", zone);
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
    }
}
