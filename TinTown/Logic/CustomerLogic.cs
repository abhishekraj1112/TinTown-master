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
    public class CustomerLogic : BaseCode
    {
        internal async Task<JsonResult> FindCustomer(string name)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("customer_find", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@filter", name);
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

        internal async Task<JsonResult> CreateCustomer(Customer customer)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("customer_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@name", customer.Name);
                smd.Parameters.AddWithValue("@mobile_no", customer.MobileNo);
                smd.Parameters.AddWithValue("@pan_no", customer.PanNo);
                smd.Parameters.AddWithValue("@gst_type", customer.GSTType);
                smd.Parameters.AddWithValue("@gst_no", customer.GSTNo);
                smd.Parameters.AddWithValue("@email_id", customer.EmailId);
                smd.Parameters.AddWithValue("@addresses", ToDataTable(customer.Addresses));
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

        internal async Task<DataTable> CustomerInfo(string customerid)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT 'True' as [condition], * FROM [dbo].[customer] c INNER JOIN [dbo].[customer_address] ca " +
                                                "ON [c].[customer_id] = [ca].[customer_id] WHERE[c].[customer_id] = @customer_id", cn);
                smd.Parameters.AddWithValue("@customer_id", customerid);
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

        internal async Task<DataTable> CustomerList()
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("SELECT 'True' as [condition], c.customer_id, c.full_name AS [customer_name], c.email_no AS [email], c.mobile_number," +
                    "c.pan_no AS[pan_number], c.gst_type, c.gst_no AS[gst_number] FROM[dbo].[customer] c", cn);
               
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

        internal async Task<JsonResult> UpdateCustomer(Customer customer)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("customer_update", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@name", customer.Name);
                smd.Parameters.AddWithValue("@mobile_no", customer.MobileNo);
                smd.Parameters.AddWithValue("@pan_no", customer.PanNo);
                smd.Parameters.AddWithValue("@gst_no", customer.GSTNo);
                smd.Parameters.AddWithValue("@email_id", customer.EmailId);
                smd.Parameters.AddWithValue("@addresses", ToDataTable(customer.Addresses));
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
