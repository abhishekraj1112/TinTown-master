using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;

namespace TinTown.Logic
{
    public class MarketPlaceLogic : BaseCode
    {
        internal async Task<DataTable> MarketPlace_invoice(string order_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT 'True' as condition,sh.order_no,sh.docket_no,'OQC Completed' as order_status,sh.invoice_url,sh.shipment_lable_url,sh.marketplace_shipment_partner FROM dbo.sales_header sh WHERE sh.order_no IN (" + order_no + ") and sh.order_status=9;", cn);
                DataTable dt = new DataTable();
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
        internal async Task<JsonResult> MarketPlace_invoice_sync(string order_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("marketplace_tracking_no_update_sync", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                DataTable dt = new DataTable();
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
    }
}
