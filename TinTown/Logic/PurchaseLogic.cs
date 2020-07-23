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
    public class PurchaseLogic : BaseCode
    {
        internal async Task<JsonResult> PurchaseOrderlist(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_order_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", location_id);
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

        internal async Task<JsonResult> ActivePurchaseOrderByVendor(string vendor_no, int location)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("po_by_vendor_active", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@vendor_no", vendor_no);
                smd.Parameters.AddWithValue("@locationid", location);
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

        internal async Task<JsonResult> GetVendorCompleteDetailWithPoNo(string vendor_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_order_no_with_vendorinfo", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@vendor_no", vendor_no);
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

        internal async Task<JArray> GetVendorItem(AddPO vendor)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_order_add_new_item", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@item_no", vendor.ItemNo);
                smd.Parameters.AddWithValue("@vendor_id", vendor.VendorNo);
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

        internal async Task<JsonResult> POForApporoval(int locationid)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_list_for_approval", cn)
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

        internal async Task<JsonResult> POInfoForUpdate(Purchase infoup)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_order_info_for_update", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@lineOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@document_no", infoup.PurchaseOrderNo);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                string lines = smd.Parameters["@lineOutput"].Value.ToString();
                smd.Dispose();

                JArray arr = JArray.Parse(json);
                JArray linejArray = JArray.Parse(lines);
                JObject jObject = arr[0] as JObject;
                jObject.Add(new JProperty("lines", linejArray));
                arr.Clear();
                arr.Add(jObject);


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

        internal async Task<JsonResult> POInfo(Purchase info)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT 'True' as [condition], (SELECT concat_ws(' , ',vm.vendor_name, vm.vendor_address, vm.pincode, vm.city, vm.[state]) 
                                                        FROM dbo.vendor_mst vm WHERE vm.vendor_no = poh.vendor_no) AS vendor_address, poh.document_no, poh.vendor_no, poh.order_date, 
                                                        poh.exp_date, poh.expiry_date_receipt, poh.pay_terms,
                                                        poh.document_status, poh.created_by, pol.item_no, pol.quantity, pol.mrp, pol.amount, pol.discount, pol.gst_percentage,
                                                        pol.gst_amount, pol.net_amount, pol.total_amount, pol.recived_quantity as received_quantity, pol.accepted_quantity, pol.rejected_quantity, 
                                                        sum(pol.quantity) OVER (ORDER BY poh.document_no) AS [total_ordered_qty], 
                                                        sum(pol.recived_quantity)OVER (ORDER BY poh.document_no) AS [total_received_qty], sum(pol.accepted_quantity)
                                                        OVER (ORDER BY poh.document_no) AS [total_accepted_qty],
                                                        sum(pol.rejected_quantity)OVER (ORDER BY poh.document_no) AS [total_reject_qty]
                                                        from purchase_order_header poh inner
                                                        join purchase_order_line pol on poh.document_no = pol.document_no
                                                        WHERE poh.document_no = @purchase_order_no", cn);

                da.SelectCommand.Parameters.AddWithValue("@purchase_order_no", info.PurchaseOrderNo);

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

        internal async Task<JsonResult> POGRNInfo(Purchase info)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand da = new SqlCommand(@"SELECT 'True' as [condition], gh.gate_entry_no, gh.grn_no,(SELECT sm.name FROM dbo.status_mst AS sm WHERE sm.id = gh.grn_status) AS grn_status, gh.grn_created_by, 
                                                        gh.grn_created_datetime, gh.document_type, gh.external_document_no, gh.external_document_date, gh.accpeted_qty, gh.rejected_qty, gh.grn_completed_by, 
                                                        gh.grn_completed_datetime, gib.item_no, SUM(gib.accepted_qty) AS accepted_qty, SUM(gib.rejected_qty) AS rejected_qty, 
                                                        SUM(gib.putway_pending_qty) AS [putway_qty], gib.vendor_lot_no, gib.expire_date
                                                        FROM dbo.grn_header AS gh INNER JOIN (SELECT gib.* FROM dbo.grn_iqc_barcode gib UNION ALL SELECT gib.* FROM Archive_TinTown.dbo.grn_iqc_barcode gib) AS gib ON gh.grn_no = gib.grn_no
                                                        WHERE gh.document_no = @purchase_order_no
                                                        GROUP BY gh.grn_created_by, gh.grn_created_datetime, gh.document_type, gh.external_document_no, gh.external_document_date, 
                                                        gh.accpeted_qty, gh.rejected_qty, gh.grn_completed_by, gh.grn_completed_datetime, gib.item_no, gh.gate_entry_no, gh.grn_no,
                                                        gib.vendor_lot_no, gib.expire_date, gh.grn_status FOR JSON AUTO;", cn);

                da.Parameters.AddWithValue("@purchase_order_no", info.PurchaseOrderNo);
                string json = (string)await da.ExecuteScalarAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(json))
                {
                    return new JsonResult(JArray.Parse(json));
                }
                else
                {
                    return await SendRespose("True", "No GRN Done").ConfigureAwait(false);
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

        internal async Task<JsonResult> POApporoved(Purchase approval)
        {

            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_approval_confirm", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@document_no", approval.PurchaseOrderNo);
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

        internal async Task<JsonResult> PurchaseOrderCreation(Purchase purchase)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("purchase_order_complete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@vendor_no", purchase.VendorNo);
                smd.Parameters.AddWithValue("@purchase_order_no", purchase.PurchaseOrderNo);
                smd.Parameters.AddWithValue("@expirydate", purchase.ExpiryDate);
                smd.Parameters.AddWithValue("@expirydatereceipt", purchase.ExpiryDateReceipt);
                smd.Parameters.AddWithValue("@payment_terms", purchase.PaymentTerms);
                smd.Parameters.AddWithValue("@location_id", purchase.LocationId);
                smd.Parameters.AddWithValue("@createdby", purchase.CreatedBy);
                smd.Parameters.AddWithValue("@lines", ToDataTable(purchase.lines));
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
