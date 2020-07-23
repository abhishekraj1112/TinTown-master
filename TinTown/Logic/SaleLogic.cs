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
    public class SaleLogic : BaseCode
    {
        internal async Task<JsonResult> SaleOrderlist(Sale sale)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sale_order_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", sale.LocationId);
                smd.Parameters.AddWithValue("@start_date", sale.Startdate);
                smd.Parameters.AddWithValue("@end_date", sale.Enddate);
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

        internal async Task<JsonResult> GetSaleNo()
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT 'True' AS [condition], concat( dp.[prefix], NEXT VALUE FOR [dbo].[seq_sale_order]) AS [so_no] FROM [dbo].[document_perfix] dp WHERE [dp].[type] ='SaleOrder'", cn);
                SqlDataAdapter da = new SqlDataAdapter(smd);
                da.Fill(dt);
                return new JsonResult(dt);
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

        internal async Task<JArray> AddItem(AddSO so)
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
                smd.Parameters.AddWithValue("@item_no", so.ItemNo);
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

        internal async Task<JsonResult> SaleOrderCreation(Sale sale)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sale_order_complete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@customer_no", sale.CustomerNo);
                smd.Parameters.AddWithValue("@payment_terms", sale.PaymentTerms);
                smd.Parameters.AddWithValue("@location_id", sale.LocationId);
                smd.Parameters.AddWithValue("@bill_to_address", sale.BillToAddress);
                smd.Parameters.AddWithValue("@ship_to_address", sale.ShipToAddress);
                smd.Parameters.AddWithValue("@createdby", sale.CreatedBy);
                smd.Parameters.AddWithValue("@lines", ToDataTable(sale.lines));
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

        internal async Task<JsonResult> SaleInfo(Sale info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sales_order_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@sale_info_json", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@address", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@sales_order_no", info.SaleOrderNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string sale_info = smd.Parameters["@sale_info_json"].Value.ToString();
                string address = smd.Parameters["@address"].Value.ToString();
                smd.Dispose();
                Connection.CloseConnection(ref cn);
                SaleInfoResponse saleInfoResponse = new SaleInfoResponse();

                JArray Jaddress = JArray.Parse(address);
                saleInfoResponse.Sales = JArray.Parse(sale_info);
                if (Jaddress[0]["address_type"].ToString() == "Bil")
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[0];
                    if (Jaddress.Count > 1)
                    {
                        saleInfoResponse.ShioTo = (JObject)Jaddress[1];
                    }
                    else
                    {
                        saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                    }
                }
                else if (Jaddress.Count > 1)
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[1];
                    saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                }
                else
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[0];
                    saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                }



                return new JsonResult(saleInfoResponse);
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

        internal async Task<JsonResult> SOForApporoval(int locationid)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sale_list_for_approval", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@location_id", locationid);
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

        internal async Task<JsonResult> SOApporoved(Sale approval)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sale_approval_confirm", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@document_no", approval.SaleOrderNo);
                smd.Parameters.AddWithValue("@location_id", approval.LocationId);
                smd.Parameters.AddWithValue("@approved_by", approval.CreatedBy);
                smd.Parameters.AddWithValue("@document_rejection_reason", approval.RejectionReason);
                smd.Parameters.AddWithValue("@order_status", approval.Orderstatus);
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

        internal async Task<JsonResult> SaleInvoiceList(Sale info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sales_invoice_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@sale_order_no", info.SaleOrderNo);
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

        internal async Task<JsonResult> SaleInvoiceInfo(Sale info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("sales_invoice_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@sale_info_json", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@address", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@sales_invoice_no", info.SaleInvoiceNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string sale_info = smd.Parameters["@sale_info_json"].Value.ToString();
                string address = smd.Parameters["@address"].Value.ToString();
                smd.Dispose();
                Connection.CloseConnection(ref cn);
                SaleInfoResponse saleInfoResponse = new SaleInfoResponse();

                JArray Jaddress = JArray.Parse(address);
                saleInfoResponse.Sales = JArray.Parse(sale_info);
                if (Jaddress[0]["address_type"].ToString() == "Bil")
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[0];
                    if (Jaddress.Count > 1)
                    {
                        saleInfoResponse.ShioTo = (JObject)Jaddress[1];
                    }
                    else
                    {
                        saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                    }
                }
                else if (Jaddress.Count > 1)
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[1];
                    saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                }
                else
                {
                    saleInfoResponse.BilTo = (JObject)Jaddress[0];
                    saleInfoResponse.ShioTo = (JObject)Jaddress[0];
                }
                return new JsonResult(saleInfoResponse);
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
