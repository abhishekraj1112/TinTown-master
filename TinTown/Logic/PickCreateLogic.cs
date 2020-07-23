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
    public class PickCreateLogic : BaseCode
    {
        internal DataTable FindOrder(PickCreate find)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_find_order", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@pick_type", find.PickType);
                smd.Parameters.AddWithValue("@document_type", find.DocumentType);
                smd.Parameters.AddWithValue("@marketplace", find.Marketplace);
                smd.Parameters.AddWithValue("@order_no", ConvertListToDataTable(find.OrderNolist));
                smd.Parameters.AddWithValue("@state", ConvertListToDataTable(find.State));
                smd.Parameters.AddWithValue("@city", ConvertListToDataTable(find.City));
                smd.Parameters.AddWithValue("@pincode", ConvertListToDataTable(find.Pincodelist));
                smd.Parameters.AddWithValue("@priority", ConvertListToDataTable(find.Priority));
                smd.Parameters.AddWithValue("@shipping_agent", ConvertListToDataTable(find.ShippingAgent));
                smd.Parameters.AddWithValue("@binzone", ConvertListToDataTable(find.BinZone));
                smd.Parameters.AddWithValue("@location_id", find.LocationId);
                SqlDataAdapter da = new SqlDataAdapter(smd);
                da.Fill(dt);
                return dt;
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

        internal async Task<JsonResult> PickCreation(PickCreate pickCreate)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_creation", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@document_type", pickCreate.DocumentType);
                smd.Parameters.AddWithValue("@marketplace", pickCreate.Marketplace);
                smd.Parameters.AddWithValue("@pick_type", pickCreate.PickType);
                smd.Parameters.AddWithValue("@email_id", pickCreate.EmailId);
                smd.Parameters.AddWithValue("@location_id", pickCreate.LocationId);
                smd.Parameters.AddWithValue("@orders", ToDataTable(pickCreate.document_no));
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
