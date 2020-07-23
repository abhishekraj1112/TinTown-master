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
    public class RoleLogic : BaseCode
    {
        internal async Task<JsonResult> Role_process(Role role)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("role_process", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@role_id", role.roleId);
                da.SelectCommand.Parameters.AddWithValue("@role_name", role.roleName);
                da.SelectCommand.Parameters.AddWithValue("@created_by", role.userId);
                da.SelectCommand.Parameters.AddWithValue("@flag", role.flag);
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
        internal async Task<JsonResult> RolePermissionDetail(int role_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand sc = new SqlCommand("role_permission_detail", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sc.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                sc.Parameters.AddWithValue("@role_id", role_id);
                await sc.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = sc.Parameters["@jsonOutput"].Value.ToString();
                sc.Dispose();
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

        internal async Task<JsonResult> RolePermissionUpdate(NewRole newRole)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand sc = new SqlCommand("role_permission_change", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sc.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                sc.Parameters.AddWithValue("@role_id", newRole.RoleId);
                sc.Parameters.AddWithValue("@page_permission_number", ConvertListToDataTable(newRole.PageId));
                await sc.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = sc.Parameters["@jsonOutput"].Value.ToString();
                sc.Dispose();
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
