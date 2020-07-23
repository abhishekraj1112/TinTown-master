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
    public class BarcodeGenrateLogic : BaseCode
    {
        internal async Task<JsonResult> BarcodePrintInfoNAV(BarcodeGenrate barcodeGenrate)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                using (SqlCommand smd = new SqlCommand("barcode_vendor_info", cn))
                {
                    smd.CommandType = System.Data.CommandType.StoredProcedure;
                    smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    smd.Parameters.AddWithValue("@mrp", barcodeGenrate.MRP);
                    smd.Parameters.AddWithValue("@vendor_no", barcodeGenrate.VendorNo);
                    smd.Parameters.AddWithValue("@vendor_name", barcodeGenrate.VendorName);
                    smd.Parameters.AddWithValue("@vendor_address", barcodeGenrate.VendorAddress);
                    smd.Parameters.AddWithValue("@vendor_city", barcodeGenrate.VendorCity);
                    smd.Parameters.AddWithValue("@vendor_country", barcodeGenrate.VendorCountry);
                    smd.Parameters.AddWithValue("@barcode", barcodeGenrate.Barcode);
                    await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    string json = smd.Parameters["@jsonOutput"].Value.ToString();
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
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

        internal async Task<DataSet> PrintBarcodeReport(BarcodeGenrate barcodeGenrate)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("barcode_all_info_to_print", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@barcode", barcodeGenrate.Barcode);
                da.SelectCommand.Parameters.AddWithValue("@times", barcodeGenrate.No);
                DataSet dt = new DataSet();
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

        internal async Task<JsonResult> SeachByStyleAndColor(string stylecode, string colorcode)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                DataTable dt = new DataTable();
                using (SqlCommand smd = new SqlCommand("barcode_seach_by_style_and_color", cn))
                {
                    smd.CommandType = System.Data.CommandType.StoredProcedure;
                    smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    smd.Parameters.AddWithValue("@color_code", colorcode);
                    smd.Parameters.AddWithValue("@style_code", stylecode);
                    await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    string json = smd.Parameters["@jsonOutput"].Value.ToString();
                    JArray arr = JArray.Parse(json);
                    return new JsonResult(arr);
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
    }
}
