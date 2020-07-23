using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class BaseExceptionLogic : BaseCode
    {
        internal async Task<JsonResult> ExceptionLog(BaseException baseExec)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter smd = new SqlDataAdapter("exception_application_log_insert", cn);
                smd.SelectCommand.CommandType = CommandType.StoredProcedure;
                smd.SelectCommand.Parameters.AddWithValue("@Exception", baseExec.Exception);
                smd.SelectCommand.Parameters.AddWithValue("@ExceptionType", baseExec.ExceptionType);
                smd.SelectCommand.Parameters.AddWithValue("@fragment", baseExec.fragment);
                smd.SelectCommand.Parameters.AddWithValue("@lineNo", baseExec.lineNo);
                smd.SelectCommand.Parameters.AddWithValue("@method", baseExec.method);
                DataTable dt = new DataTable();
                smd.Fill(dt);
                smd.Dispose();
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
