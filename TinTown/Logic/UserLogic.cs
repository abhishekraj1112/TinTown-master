using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Hubs;
using TinTown.Models;

namespace TinTown.Logic
{
    public class UserLogic : BaseCode
    {
        private readonly IHubContext<Notification> _hub;
        public UserLogic(IHubContext<Notification> hub)
        {
            _hub = hub;
        }
        public UserLogic() { }
        internal async Task<JsonResult> createUser(User create)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("user_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                smd.Parameters.AddWithValue("@name", create.Name);
                smd.Parameters.AddWithValue("@email", create.Email);
                smd.Parameters.AddWithValue("@password_hash", create.password);
                smd.Parameters.AddWithValue("@role_id", create.roleId);
                smd.Parameters.AddWithValue("@location_id", create.locationId);
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

        internal async Task<JsonResult> allUser(string name, string worktype, int location)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("user_list", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@role_name", name);
                da.SelectCommand.Parameters.AddWithValue("@worktype", worktype);
                da.SelectCommand.Parameters.AddWithValue("@location", location);
                da.Fill(dt);
                List<User> userDetails = new List<User>();
                userDetails = ConvertDataTable<User>(dt);
                return new JsonResult(userDetails);
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

        internal DataTable login_check(User login, string flag)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("user_login", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@flag", flag);
                da.SelectCommand.Parameters.AddWithValue("@email", login.Email);
                da.SelectCommand.Parameters.Add("@menu", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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



        internal async Task<JsonResult> login_set(User login, string flag)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("user_login", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@email", login.Email);
                da.SelectCommand.Parameters.AddWithValue("@flag", flag);
                da.SelectCommand.Parameters.Add("@menu", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                da.Fill(dt);
                string json = da.SelectCommand.Parameters["@menu"].Value.ToString();
                JArray arr = JArray.Parse(json);
                List<User> userDetails = new List<User>();
                userDetails = ConvertDataTable<User>(dt);
                if (userDetails.Count > 0)
                {
                    userDetails[0].menu = arr;
                }

                return new JsonResult(userDetails);
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

        internal async Task<JsonResult> UpdateRole(User roleupdate)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("user_role_change", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email", roleupdate.Email);
                smd.Parameters.AddWithValue("@role_id", roleupdate.roleId);
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

        internal async Task<JsonResult> UpdatePassword(User roleupdate)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("user_password_change", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email", roleupdate.Email);
                smd.Parameters.AddWithValue("@password", roleupdate.password);
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

        internal DataTable logout(User lout)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("user_logout", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@email_id", lout.Email);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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

        internal async Task<JsonResult> LocationList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("SELECT lm.[id] AS [location_id], lm.[name] AS [location_name] FROM [dbo].[location_mst] lm", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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

        internal DataTable logout_signalr(User lout)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter("user_logout_signalr", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@email_id", lout.Email);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
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
    }
}
