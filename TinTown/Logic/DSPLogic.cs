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
    public class DSPLogic : BaseCode
    {
        internal async Task<JsonResult> DSPPartnerList(int location, int code)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("dsp_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", location);
                smd.Parameters.AddWithValue("@code", code);
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

        internal async Task<JsonResult> CreateUpdateDSP(DSP dsp)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("dsp_create_or_update", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@address", dsp.Address);
                smd.Parameters.AddWithValue("@api_url", dsp.APIUrl);
                smd.Parameters.AddWithValue("@country", dsp.Country);
                smd.Parameters.AddWithValue("@description", dsp.Description);
                smd.Parameters.AddWithValue("@dsp_code", dsp.DSPCode);
                smd.Parameters.AddWithValue("@flag", dsp.Flag);
                smd.Parameters.AddWithValue("@gst_no", dsp.GSTNo);
                smd.Parameters.AddWithValue("@location_id", dsp.LocationId);
                smd.Parameters.AddWithValue("@state", dsp.State);
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

        internal async Task<JsonResult> UploadAWB(DSP dspawbno)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("dsp_awb_upload", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@dsp_code", dspawbno.DSPCode);
                smd.Parameters.AddWithValue("@location_id", dspawbno.LocationId);
                smd.Parameters.AddWithValue("@payment_type", dspawbno.PaymentType);
                smd.Parameters.AddWithValue("@awb_no", ToDataTable(dspawbno.AWBNo));
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

        internal DataTable DspAwb(DSP bin)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand(@"SELECT 'True' condition, dam.awb_no FROM dbo.dsp_awb_mst dam 
                                                WHERE dam.location_id = @location AND dam.dsp_code = @dsp_code AND dam.pay_type = @payment_type AND dam.utilized = 0
                                                ORDER BY dam.awb_no
                                                OFFSET @pageno * 50 ROWS
                                                FETCH NEXT 50 ROWS ONLY", cn);
                smd.Parameters.AddWithValue("@location", bin.LocationId);
                smd.Parameters.AddWithValue("@dsp_code", bin.DSPCode);
                smd.Parameters.AddWithValue("@payment_type", bin.PaymentType);
                smd.Parameters.AddWithValue("@pageno", bin.PageNo);
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

        internal async Task<JsonResult> CreateUpdateDSPService(DSPService dspservice)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("dsp_new_service", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@pincode", dspservice.Pincode);
                smd.Parameters.AddWithValue("@express", dspservice.Express);
                smd.Parameters.AddWithValue("@reversepickup", dspservice.ReversePickUp);
                smd.Parameters.AddWithValue("@dsp_code", dspservice.DSPCode);
                smd.Parameters.AddWithValue("@flag", dspservice.Flag);
                smd.Parameters.AddWithValue("@priority", dspservice.Priority);
                smd.Parameters.AddWithValue("@cost", dspservice.Cost);
                smd.Parameters.AddWithValue("@location_id", dspservice.LocationId);
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

        internal async Task<JsonResult> DSPServiceList(int location, string dsp_code)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("dsp_service_list", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@location_id", location);
                smd.Parameters.AddWithValue("@dsp_code", dsp_code);
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
