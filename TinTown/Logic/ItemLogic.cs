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
    public class ItemLogic : BaseCode
    {
        internal async Task<JsonResult> FindItem(string name_or_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("item_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@no_or_name", name_or_no);
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

        internal async Task<JsonResult> ItemList(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("item_list_with_inventory", cn)
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

        internal async Task<JsonResult> ItemFullInfo(Item icm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("item_full_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", icm.Location_id);
                smd.Parameters.AddWithValue("@item_no", icm.item_no);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@bindata", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                string bindata = smd.Parameters["@bindata"].Value.ToString();
                smd.Dispose();

                JArray bindataarr = JArray.Parse(bindata);
                JArray arr = JArray.Parse(json);
                JObject iteminfo = arr[0] as JObject;
                iteminfo.Add("Bininfo", bindataarr);
                JArray retrun = new JArray(iteminfo);
                return new JsonResult(retrun);
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

        internal async Task<JsonResult> ItemCategoryList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT icm.id, icm.cat_code AS code, icm.cat_name AS name, icm.sub_id, icm.description, icm.created_by FROM dbo.item_category_mst icm WHERE sub_id='0'", cn);
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

        internal async Task<JsonResult> ItemCategoryCreate(ItemCategoryModel icm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_category_create]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@code", icm.code);
                smd.Parameters.AddWithValue("@name", icm.name);
                smd.Parameters.AddWithValue("@sub_id", icm.SubId);
                smd.Parameters.AddWithValue("@description", icm.description);
                smd.Parameters.AddWithValue("@flag", icm.flag);
                smd.Parameters.AddWithValue("@created_by", icm.created_by);
                smd.Parameters.AddWithValue("@update_by", icm.updated_by);
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

        internal async Task<JsonResult> ItemSubCategoryList(long id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT icm.id, icm.cat_code AS code, icm.cat_name AS name, icm.sub_id, icm.description, icm.created_by FROM dbo.item_category_mst icm WHERE sub_id=" + id, cn);
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

        internal async Task<JsonResult> ItemCategoryDelete(itemCategorydeleteModel icdm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_category_delete]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@id", icdm.Id);
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

        internal async Task<JsonResult> ItemAttributeTypeList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT iat.attribute_no, iat.code, iat.description, iat.created_by FROM dbo.item_attribute_type iat", cn);
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

        internal async Task<JsonResult> ItemAttributeTypeCreate(itemattributetypeModel iatm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_attribute_type_create]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@attribute_no", iatm.attribute_no);
                smd.Parameters.AddWithValue("@code", iatm.code);
                smd.Parameters.AddWithValue("@description", iatm.description);
                smd.Parameters.AddWithValue("@created_by", iatm.created_by);
                smd.Parameters.AddWithValue("@update_by", iatm.updated_by);
                smd.Parameters.AddWithValue("@flag", iatm.flag);
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

        internal async Task<JsonResult> ItemAttributeValueList(long attribute_type_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT iav.attribute_value_no, iav.[value], iav.description, iav.created_by FROM dbo.item_attribute_value iav WHERE iav.attribute_type_no=" + attribute_type_no, cn);
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

        internal async Task<JsonResult> ItemAttributeValueCreate(itemattributevalueModel iavm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_attribute_value_create]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@attribute_type_no", iavm.attribute_type_no);
                smd.Parameters.AddWithValue("@attribute_value_no", iavm.attribute_value_no);
                smd.Parameters.AddWithValue("@value", iavm.value);
                smd.Parameters.AddWithValue("@description", iavm.description);
                smd.Parameters.AddWithValue("@created_by", iavm.created_by);
                smd.Parameters.AddWithValue("@update_by", iavm.updated_by);
                smd.Parameters.AddWithValue("@flag", iavm.flag);
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

        internal async Task<JsonResult> ItemAttributeDelete(itemAttributedeleteModel icdm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_attribute_delete]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@attribute_no", icdm.attribute_no);
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

        internal async Task<JsonResult> GstGroupIdValue()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT gs.gst_group_id,CONCAT(gs.name,'','(',gs.percentage,'%',')') AS gst_group FROM dbo.gst_setup gs", cn);
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

        internal async Task<JsonResult> GstHsnCode(int GstGroupId)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ghm.hsn_id, ghm.gst_group_id, ghm.description FROM dbo.gst_hsn_mst ghm WHERE ghm.gst_group_id=" + GstGroupId, cn);
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

        internal async Task<JsonResult> BaseUomValue()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT bum.id,bum.name FROM base_uom_mst bum", cn);
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

        internal async Task<JsonResult> ItemCreate(Item itm)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("[item_create]", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@item_no", itm.item_no);
                smd.Parameters.AddWithValue("@name", itm.name);
                smd.Parameters.AddWithValue("@description", itm.description);
                smd.Parameters.AddWithValue("@base_uom", itm.base_uom);
                smd.Parameters.AddWithValue("@purchase_uom", itm.purchaseUom);
                smd.Parameters.AddWithValue("@sale_uom", itm.saleUom);
                smd.Parameters.AddWithValue("@category", itm.category);
                smd.Parameters.AddWithValue("@sub_category", itm.subCategory);
                smd.Parameters.AddWithValue("@unit_price", itm.unitPrice);
                smd.Parameters.AddWithValue("@gst_group_id", itm.gstGroupId);
                smd.Parameters.AddWithValue("@gst_hsn_code", itm.gstHsnCode);
                smd.Parameters.AddWithValue("@cost_per_unit", itm.costPerUnit);
                smd.Parameters.AddWithValue("@mrp", itm.mrp);
                smd.Parameters.AddWithValue("@image_url", itm.image_url);
                smd.Parameters.AddWithValue("@flag", itm.flag);
                smd.Parameters.AddWithValue("@created_by", itm.created_by);
                smd.Parameters.AddWithValue("@updated_by", itm.updated_by);
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
    }
}
