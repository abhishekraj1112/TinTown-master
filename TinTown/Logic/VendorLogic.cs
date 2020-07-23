using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class VendorLogic : BaseCode
    {
        internal async Task<JsonResult> GetVendorDetail(string no_or_name)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("vendor_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@no_or_name", no_or_name);
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


        internal async Task<JsonResult> AllVendorList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT vm.vendor_no AS id, vm.vendor_name AS name, vm.vendor_address AS address, vm.gst_reg_no,vm.vendor_type, vm.pincode, vm.city, vm.[state], vm.country," +
                    "vm.mobile_no, vm.phone_no, vm.email_id, vm.contact_person_name, vm.contact_person_cntct_no, vm.operation_person_name, vm.operation_person_cntct_no," +
                    "vm.account_person_name, vm.account_person_cntct_no, vm.warehouse_person_name, vm.warehouse_person_cntct_no, vm.perchase_order_person_name," +
                    "vm.perchase_order_person_cntct_no, vm.home_page, vm.gst_reg_no, vm.gst_type, vm.pan_no, vm.currency  FROM dbo.vendor_mst vm", cn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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
        internal async Task<JsonResult> CreateVendor(VendorModel vm, List<FileModel> file)
        {
            SqlConnection cn = null;

            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[vendor_create]", cn)
                {

                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@JsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@type", vm.type);
                smd.Parameters.AddWithValue("@name", vm.name);
                smd.Parameters.AddWithValue("@address", vm.address);
                smd.Parameters.AddWithValue("@pincode", vm.pincode);
                smd.Parameters.AddWithValue("@city", vm.city);
                smd.Parameters.AddWithValue("@state", vm.state);
                smd.Parameters.AddWithValue("@country", vm.country);
                smd.Parameters.AddWithValue("@mobile_no", vm.mobile_no);
                smd.Parameters.AddWithValue("@phone_no", vm.phone_no);
                smd.Parameters.AddWithValue("@email", vm.email);
                smd.Parameters.AddWithValue("@contact_person_name", vm.contact_person_name);
                smd.Parameters.AddWithValue("@contact_person_cntct_no", vm.cntct_prn_cntct_no);
                smd.Parameters.AddWithValue("@oprtn_prsn_name", vm.oprtn_prsn_name);
                smd.Parameters.AddWithValue("@oprtn_prsn_cntct_no", vm.oprtn_prsn_cntct_no);
                smd.Parameters.AddWithValue("@accnt_prsn_name", vm.accnt_prsn_name);
                smd.Parameters.AddWithValue("@accnt_prsn_cntct_no", vm.accnt_prsn_cntct_no);
                smd.Parameters.AddWithValue("@warehouse_prsn_name", vm.warehouse_prsn_name);
                smd.Parameters.AddWithValue("@warehouse_prsn_cntct_no", vm.warehouse_prsn_cntct_no);
                smd.Parameters.AddWithValue("@prchase_order_prsn_name", vm.prchase_order_prsn_name);
                smd.Parameters.AddWithValue("@prchase_order_cntct_no", vm.prchase_order_cntct_no);
                smd.Parameters.AddWithValue("@home_page", vm.home_page);
                smd.Parameters.AddWithValue("@gst_type", vm.gst_type);
                smd.Parameters.AddWithValue("@gst_no", vm.gst_no);
                smd.Parameters.AddWithValue("@pan_no", vm.pan_no);
                smd.Parameters.AddWithValue("@currency", vm.currency);
                if (file != null)
                {
                    smd.Parameters.AddWithValue("@file", ToDataTable(file));
                }
                smd.Parameters.AddWithValue("@created_by", vm.created_by);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@JsonOutput"].Value.ToString();
                JArray arr = JArray.Parse(json);
                return new JsonResult(arr);
            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                Connection.CloseConnection(ref cn);
            }
        }

        internal async Task<JsonResult> VendorCatalogueList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT vc.[vendor_no], vc.[item_no] item_code, vc.[vendor_item_code], vc.[cost_per_unit], vc.[description],[vm].[vendor_name] " +
                                                        "FROM [dbo].[vendor_catalog] vc INNER JOIN [dbo].[vendor_mst] vm ON [vc].[vendor_no] = [vm].[vendor_no]", cn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
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

        internal async Task<JsonResult> CreateUpdateVendorCatalogue(VendorCatalogueModel vcm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("vendor_catalogue_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@vendor_name", vcm.vendor_no);
                smd.Parameters.AddWithValue("@item_code", vcm.item_code);
                smd.Parameters.AddWithValue("@vendor_item_code", vcm.vendor_item_code);
                smd.Parameters.AddWithValue("@cost_per_unit", vcm.cost_per_unit);
                smd.Parameters.AddWithValue("@description", vcm.description);
                smd.Parameters.AddWithValue("@created_by", vcm.created_by);
                smd.Parameters.AddWithValue("@update_by", vcm.update_by);
                smd.Parameters.AddWithValue("@flag", vcm.flag);
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

        internal async Task<JsonResult> DeleteVendorCatalogue(VendorCatalogueModel vcm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[vendor_catalogue_delete]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@vendor_name", vcm.vendor_no);
                smd.Parameters.AddWithValue("@item_code", vcm.item_code);
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
