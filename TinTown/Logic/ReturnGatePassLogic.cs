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
    public class ReturnGatePassLogic : BaseCode
    {
        internal async Task<JsonResult> RGPList(ReturnGatePass returnGatePass)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SET @jsonOutput = (SELECT 'True' as [condition], gph.document_no, (SELECT rp.party_name FROM dbo.rgp_party rp WHERE rp.party_no = gph.party_id) party_name," +
                   " sm.name AS order_status, gph.created_date, gph.created_by FROM dbo.gate_pass_header gph INNER JOIN dbo.status_mst sm ON gph.order_status = sm.id " +
                   "WHERE gph.document_type = 'Return GatePass' AND gph.from_location_id = @location_id for json path)", cn);
                smd.Parameters.AddWithValue("@location_id", returnGatePass.LocationId);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                if (json.Length > 2)
                {
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
                }
                else
                {
                    return await SendRespose("False", "No Record Found").ConfigureAwait(true);
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

        internal async Task<JsonResult> PartyList(ReturnGatePass returnGatePass)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SET @jsonOutput  = (SELECT rp.party_no, rp.party_name as [name] FROM dbo.rgp_party rp FOR JSON PATH)", cn);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                if (json.Length > 2)
                {
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
                }
                else
                {
                    return await SendRespose("False", "No Record Found").ConfigureAwait(true);
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

        internal async Task<JsonResult> NewRGPNo(ReturnGatePass returnGatePass)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT CONCAT( dp.prefix , NEXT VALUE FOR dbo.seq_rgp_no) FROM dbo.document_perfix dp WHERE dp.type = 'ReturnGatePass'", cn);
                string num = (string)await smd.ExecuteScalarAsync().ConfigureAwait(false);
                return await SendRespose("True", num).ConfigureAwait(true);
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

        internal async Task<JArray> AddItem(ReturnGatePass returnGatePass)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sale_order_add_new_item", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@item_no", returnGatePass.ItemNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();

                JArray arr = JArray.Parse(json);

                return arr;
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

        internal async Task<JsonResult> Complete(RGPComplete returnGatePass)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("rgp_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@rgp_no", returnGatePass.RGPNo);
                smd.Parameters.AddWithValue("@created_by", returnGatePass.CreatedBy);
                smd.Parameters.AddWithValue("@from_location", returnGatePass.FromLocation);
                smd.Parameters.AddWithValue("@party_id", returnGatePass.PartyId);
                smd.Parameters.AddWithValue("@lines", ToDataTable(returnGatePass.Lines));
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
