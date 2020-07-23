using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace TinTown.EntryCode
{
    public static class Connection
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection cn = new SqlConnection(Startup.connection);
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                return cn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void CloseConnection(ref SqlConnection cn)
        {
            try
            {
                cn.Close();
                cn.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
