using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TinTown.EntryCode;
using TinTown.Models;

namespace TinTown.Logic
{
    public class PickLogic : BaseCode
    {
        internal async Task<JsonResult> pickZone()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter smd = new SqlDataAdapter("pick_zone_list", cn);
                smd.SelectCommand.CommandType = CommandType.StoredProcedure;
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

        internal async Task<ReturnResponse> PickerInfo(string email, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("picker_info", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email_id", email);
                smd.Parameters.AddWithValue("@location_id", location_id);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<ReturnResponse> PickStart(GetPick getPick)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_start", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@email_id", getPick.EmailId);
                smd.Parameters.AddWithValue("@user_zone", getPick.PickZone);
                smd.Parameters.AddWithValue("@shift", getPick.ShiftId);
                smd.Parameters.AddWithValue("@tray", getPick.TrayNo);
                smd.Parameters.AddWithValue("@work_type", getPick.WorkType);
                smd.Parameters.AddWithValue("@location_id", getPick.LocationId);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<ReturnResponse> ScanBinBarcode(PickScan pickScan)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_scan_bin_barcode", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@PickNo", pickScan.PickNo);
                smd.Parameters.AddWithValue("@TrayNo", pickScan.TrayNo);
                smd.Parameters.AddWithValue("@Barcode", pickScan.Barcode);
                smd.Parameters.AddWithValue("@EmailId", pickScan.EmailId);
                smd.Parameters.AddWithValue("@PickLineNo", pickScan.PickLineNo);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<ReturnMessage> ForceAssigment(ForcePickAssig forcePick)
        {
            SqlConnection cn = null;
            try
            {
                List<SendResponse> SignalRResponse = new List<SendResponse>();
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_force_assignment_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@focefulassigmentOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                smd.Parameters.AddWithValue("@pick_no", forcePick.PickNo);
                smd.Parameters.AddWithValue("@user_zone_list", ToDataTable(forcePick.ZoneWithPicker));

                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);

                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                string Signalrjson = smd.Parameters["@focefulassigmentOutput"].Value.ToString();

                smd.Dispose();


                if (Signalrjson.Length > 1)
                {
                    SignalRResponse = JsonConvert.DeserializeObject<List<SendResponse>>(Signalrjson);
                }
                JArray arr = JArray.Parse(json);

                return new ReturnMessage() { data = arr, sendResponse = SignalRResponse };
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

        internal async Task<JsonResult> PickInfoForForceAssigment(PickSearch PickNo)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_info_for_force_assigment", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@pick_no", PickNo.PickNo);
                smd.Parameters.AddWithValue("@location_id", PickNo.LocationId);
                smd.Parameters.AddWithValue("@work_type", PickNo.WorkType);
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

        internal async Task<ReturnResponse> ProductImageMisMatch(PickScan pIMM)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_image_mismatch", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@Barcode", pIMM.Barcode);
                smd.Parameters.AddWithValue("@Bincode", pIMM.Bincode);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<string> NotFoundNewLine(List<PickNF> pickNF)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_not_found_line", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                DataTable dt = ToDataTable(pickNF);
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@NF_lines", ToDataTable(pickNF));
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                return json;
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

        internal async Task<JsonResult> OrderHoldOrUnHold(OrderCancel stage)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("order_hold_or_unhold", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@order_no", stage.OrderNo);
                smd.Parameters.AddWithValue("@order_status", stage.OrderStatus);
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

        internal async Task<JsonResult> PickSetupAll(PickSetup setup)
        {
            SqlConnection cn = null;
            try
            {
                DataTable dt = new DataTable();
                cn = Connection.GetConnection();
                if (setup.flag == "select")
                {
                    SqlDataAdapter da = new SqlDataAdapter("select 'True' as [condition], min_bin from pick_setup", cn);
                    da.Fill(dt);
                }
                else if (setup.flag == "update")
                {
                    SqlDataAdapter da = new SqlDataAdapter("update pick_setup set min_bin = " + setup.NoOfBin + ";select 'True' as [condition], min_bin from pick_setup", cn);
                    da.Fill(dt);
                }
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

        internal async Task<ReturnResponse> ProductNotFound(PickScan pNF)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_not_found", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@pick_no", pNF.PickNo);
                smd.Parameters.AddWithValue("@pick_line_no", pNF.PickLineNo);
                smd.Parameters.AddWithValue("@email_id", pNF.EmailId);
                smd.Parameters.AddWithValue("@user_zone", pNF.UserZone);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<ReturnResponse> CloseTray(PickScan closetray)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_close_tray", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@pick_no", closetray.PickNo);
                smd.Parameters.AddWithValue("@tray", closetray.TrayNo);
                smd.Parameters.AddWithValue("@email_id", closetray.EmailId);
                smd.Parameters.AddWithValue("@location_id", closetray.LocationId);
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                string json = smd.Parameters["@jsonOutput"].Value.ToString();
                smd.Dispose();
                ReturnResponse returnResponse = JsonConvert.DeserializeObject<ReturnResponse>(json);
                return returnResponse;
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

        internal async Task<JsonResult> CancelOrderComplete(OrderCancel ordercancel)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("cancel_order_complete", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@order_no", ordercancel.OrderNo);
                smd.Parameters.AddWithValue("@order_status", ordercancel.OrderStatus);
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

        internal async Task<JsonResult> PriorityList()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT psap.name AS [Priorty], id as [PriortyId] FROM dbo.pick_status_and_priorty psap WHERE psap.type = 'Priority'", cn);
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

        internal async Task<JsonResult> PickPriority(string worktype, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_for_priorty", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.AddWithValue("@worktype", worktype);
                smd.Parameters.AddWithValue("@location_id", location_id);
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

        internal async Task<JsonResult> ChangePickPriority(Priority change)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("pick_priority_change", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                smd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@pick_no", change.PickNo);
                smd.Parameters.AddWithValue("@priorty_id", change.PriorityID);
                smd.Parameters.AddWithValue("@email_id", change.EmailID);
                smd.Parameters.AddWithValue("@worktype", change.WorkType);
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
