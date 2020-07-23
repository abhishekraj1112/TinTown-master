using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class RejectionLogic : BaseCode
    {
        internal async Task<JsonResult> RejectionWork(Rejection rejection)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("rejection_all_in_one", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@flag", rejection.Flag);
                smd.Parameters.AddWithValue("@rejection_type", rejection.RejectionType);
                smd.Parameters.AddWithValue("@rejection_description", rejection.RejectionDescription);
                smd.Parameters.AddWithValue("@rejection_code", rejection.RejectionCode);

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
    }
}
