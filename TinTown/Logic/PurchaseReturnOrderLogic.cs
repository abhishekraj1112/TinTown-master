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
    public class PurchaseReturnOrderLogic : BaseCode
    {
        internal async Task<JsonResult> PROList(PurchaseReturnOrder pro)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SET @jsonOutput = (SELECT 'True' as [condition], gph.document_no, (SELECT vm.vendor_name FROM dbo.vendor_mst vm WHERE vm.vendor_no = gph.party_id) vendor_name," +
                   " sm.name AS order_status, gph.created_date, gph.created_by FROM dbo.gate_pass_header gph INNER JOIN dbo.status_mst sm ON gph.order_status = sm.id " +
                   "WHERE gph.document_type = 'Purchase Return Order' AND gph.from_location_id = @location_id for json path)", cn);
                smd.Parameters.AddWithValue("@location_id", pro.LocationId);
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

        internal async Task<JsonResult> NewPRONo(PurchaseReturnOrder pro)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT CONCAT( dp.prefix , NEXT VALUE FOR dbo.seq_pro) FROM dbo.document_perfix dp WHERE dp.type = 'PurchaseReturn'", cn);
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

        internal async Task<JArray> AddItem(PurchaseReturnOrder pro)
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
                smd.Parameters.AddWithValue("@item_no", pro.ItemNo);
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

        internal async Task<JsonResult> Complete(PurchaseReturnOrderComplete pro)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_return_order_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@pro_no", pro.PurchaseNo);
                smd.Parameters.AddWithValue("@created_by", pro.CreatedBy);
                smd.Parameters.AddWithValue("@from_location", pro.FromLocation);
                smd.Parameters.AddWithValue("@vendor_no", pro.VendorNo);
                smd.Parameters.AddWithValue("@lines", ToDataTable(pro.Lines));
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


        internal async Task<JsonResult> PurchaseReturnOrderInfo(PurchaseReturnOrderComplete info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[purchase_return_order_info]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@purchase_return_info_json", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@vendor_detail", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@location_detail", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@purchase_return_order_no", info.PurchaseNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string purchase_return_order_info = smd.Parameters["@purchase_return_info_json"].Value.ToString();
                string vendor_detail= smd.Parameters["@vendor_detail"].Value.ToString();
                string location_detail = smd.Parameters["@location_detail"].Value.ToString();
                smd.Dispose();
                Connection.CloseConnection(ref cn);
                PurchaseReturnOrderResponse proInfoResponse = new PurchaseReturnOrderResponse();

                //JArray Jaddress = JArray.Parse(address);
                proInfoResponse.PurchaseReturnOrderInfo = JArray.Parse(purchase_return_order_info);
                proInfoResponse.VendorDetail = JArray.Parse(vendor_detail);
                proInfoResponse.LocationDetail = JArray.Parse(location_detail);

                return new JsonResult(proInfoResponse);
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
