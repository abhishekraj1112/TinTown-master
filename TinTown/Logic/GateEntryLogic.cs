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
    public class GateEntryLogic : BaseCode
    {
        internal async Task<JsonResult> AllGateEntryList(int location)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("gateentry_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location", location);
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

        internal async Task<JsonResult> CreateGateEntry(GateEntry gateEntry)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("gateentry_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@ChallanDate", gateEntry.ChallanDate);
                smd.Parameters.AddWithValue("@ChallanNo", gateEntry.ChallanNo);
                smd.Parameters.AddWithValue("@DocumentNo", gateEntry.DocumentNo);
                smd.Parameters.AddWithValue("@DocumentType", gateEntry.DocumentType);
                smd.Parameters.AddWithValue("@DriverName", gateEntry.DriverName);
                smd.Parameters.AddWithValue("@DriverNumber", gateEntry.DriverNumber);
                smd.Parameters.AddWithValue("@Freight", gateEntry.Freight);
                smd.Parameters.AddWithValue("@FreightAmount", gateEntry.FreightAmount);
                smd.Parameters.AddWithValue("@LocationId", gateEntry.LocationId);
                smd.Parameters.AddWithValue("@LRDate", gateEntry.LRDate);
                smd.Parameters.AddWithValue("@LRNo", gateEntry.LRNo);
                smd.Parameters.AddWithValue("@NoofBox", gateEntry.NoofBox);
                smd.Parameters.AddWithValue("@Transporter", gateEntry.Transporter);
                smd.Parameters.AddWithValue("@VehicleNo", gateEntry.VehicleNo);
                smd.Parameters.AddWithValue("@VendorNo", gateEntry.VendorNo);
                smd.Parameters.AddWithValue("@CreatedBy", gateEntry.CreatedBy);
                smd.Parameters.AddWithValue("@Description", gateEntry.Description);
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

        internal async Task<JsonResult> GateEntryInfoByid(string gateentry_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("gateentry_info_by_id", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@gateentry_no", gateentry_no);
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
