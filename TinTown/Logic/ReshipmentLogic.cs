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
    public class ReshipmentLogic : BaseCode
    {
        internal async Task<JsonResult> AwbList(int location)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("reshipment_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                smd.Parameters.AddWithValue("@location_id", location);

                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);

                string json = smd.Parameters["@jsonOutput"].Value.ToString();

                return new JsonResult(JArray.Parse(json));
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

        internal async Task<JsonResult> NewDspandAwb(Reshipment reshipment)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("reshipment_new_awb", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                smd.Parameters.AddWithValue("@type", reshipment.Type);
                smd.Parameters.AddWithValue("@location_id", reshipment.LocationId);
                smd.Parameters.AddWithValue("@lines", ToDataTable(reshipment.lines));

                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);

                string json = smd.Parameters["@jsonOutput"].Value.ToString();

                return new JsonResult(JArray.Parse(json));
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
