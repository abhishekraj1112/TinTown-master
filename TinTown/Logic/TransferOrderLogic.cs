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
    public class TransferOrderLogic : BaseCode
    {
        internal async Task<JsonResult> InboundList(TransferOrder inboundTransfer)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SET @jsonOutput = (SELECT 'True' as [condition], gph.document_no, (SELECT lm.name FROM dbo.location_mst lm WHERE lm.id = gph.from_location_id) from_location," +
                                                "(SELECT lm.name FROM dbo.location_mst lm WHERE lm.id = gph.to_location_id) to_location, sm.name AS order_status, " +
                                                "gph.created_date, gph.created_by FROM dbo.gate_pass_header gph INNER JOIN dbo.status_mst sm ON gph.order_status = sm.id " +
                                                "WHERE gph.document_type = 'Transfer Order' AND gph.from_location_id = @location_id for json path)", cn);
                smd.Parameters.AddWithValue("@location_id", inboundTransfer.LocationId);
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

        internal async Task<JArray> AddItem(TransferOrder inboundTransfer)
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
                smd.Parameters.AddWithValue("@item_no", inboundTransfer.ItemNo);
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

        internal async Task<JsonResult> NewTransferNo(TransferOrder inboundTransfer)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT CONCAT( dp.prefix , NEXT VALUE FOR dbo.seq_transfer_no) FROM dbo.document_perfix dp WHERE dp.type = 'Transfer'", cn);
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

        internal async Task<JsonResult> Complete(TransferComplete inboundTransfer)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("tansfer_order_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@transfer_no", inboundTransfer.TransferNo);
                smd.Parameters.AddWithValue("@created_by", inboundTransfer.CreatedBy);
                smd.Parameters.AddWithValue("@from_location", inboundTransfer.FromLocation);
                smd.Parameters.AddWithValue("@to_location", inboundTransfer.ToLocation);
                smd.Parameters.AddWithValue("@lines", ToDataTable(inboundTransfer.Lines));
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

        internal async Task<JsonResult> TransferOrderInfo(TransferComplete info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[transfer_order_info]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@transfer_return_info_json", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@address", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@transfer_order_no", info.TransferNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string sale_info = smd.Parameters["@transfer_return_info_json"].Value.ToString();
                string address = smd.Parameters["@address"].Value.ToString();
                smd.Dispose();
                Connection.CloseConnection(ref cn);
                TransferOrderInfoResponse transferOrderInfoResponse = new TransferOrderInfoResponse();

                JArray Jaddress = JArray.Parse(address);
                transferOrderInfoResponse.TransferOrder = JArray.Parse(sale_info);
                if (Jaddress[0]["location_type"].ToString() == "from")
                {
                    transferOrderInfoResponse.FromLocation = (JObject)Jaddress[0];
                    if (Jaddress.Count > 1)
                    {
                        transferOrderInfoResponse.ToLocation = (JObject)Jaddress[1];
                    }
                    else
                    {
                        transferOrderInfoResponse.ToLocation = (JObject)Jaddress[0];
                    }
                }
                else if (Jaddress.Count > 1)
                {
                    transferOrderInfoResponse.FromLocation = (JObject)Jaddress[1];
                    transferOrderInfoResponse.ToLocation = (JObject)Jaddress[0];
                }
                else
                {
                    transferOrderInfoResponse.FromLocation = (JObject)Jaddress[0];
                    transferOrderInfoResponse.ToLocation = (JObject)Jaddress[0];
                }

                return new JsonResult(transferOrderInfoResponse);
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