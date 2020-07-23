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
    public class ManifestLogic : BaseCode
    {

        internal async Task<DataTable> GetLastScanAwbNo(string created_by)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT * FROM [dbo].[get_post_manifest_data]('" + created_by + "')", cn);
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

        internal async Task<JsonResult> shipment_partner()
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT dpm.dsp_code as [code], dpm.dsp_code as [name] FROM dbo.dsp_partner_mst dpm", cn);
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

        internal async Task<JsonResult> ScanAwbNoManifestCreate(Post_Manifest post_Manifest)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_sorting_scan", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@awb_no", post_Manifest.awb_no);
                smd.Parameters.AddWithValue("@created_by", post_Manifest.created_by);
                smd.Parameters.AddWithValue("@location_id", post_Manifest.location_id);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();
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

        internal async Task<JsonResult> manifest_create(Manifest manifest)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_get_for_posting", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.Add("@json_output_shipment", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@dsp_docket_name", manifest.ship_agent_code);
                smd.Parameters.AddWithValue("@location_id", manifest.location_id);
                smd.Parameters.AddWithValue("@created_by", manifest.created_by);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();

                string json_output_shipment = smd.Parameters["@json_output_shipment"].Value.ToString();

                smd.Dispose();

                JArray arr = JArray.Parse(json);
                JArray arr2 = new JArray();

                if (json_output_shipment != null && json_output_shipment != "")
                {
                    arr2 = JArray.Parse(json_output_shipment);
                }

                array_wrapper array_wrapper1 = new array_wrapper()
                {
                    Manifest_Data = arr,
                    Manifest_Ship_Data = arr2
                };

                return new JsonResult(array_wrapper1);
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

        internal async Task<JsonResult> manifest_mark_release(Manifest manifest)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_release", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@manifest_no", manifest.manifest_no);
                smd.Parameters.AddWithValue("@location_id", manifest.location_id);
                smd.Parameters.AddWithValue("@created_by", manifest.created_by);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();

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

        internal async Task<JsonResult> manifest_mark_created(Manifest manifest)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_mark_created", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@manifest_no", manifest.manifest_no);
                smd.Parameters.AddWithValue("@created_by", manifest.created_by);
                smd.Parameters.AddWithValue("@location_id", manifest.location_id);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();

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

        //HANDOVER PROCESS

        internal async Task<DataTable> get_handover_ship_agent_code(string ship_agent_code, int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT 'TRUE' as condition,ml.sub_manifest_no,ml.ship_agent_code,
                            (SELECT count(*) FROM dbo.manifest_line_awb_no mlan INNER JOIN dbo.manifest_header mh2 ON mh2.manifest_no = mlan.manifest_no WHERE mh2.STATUS = 'CREATED' 
                            AND mlan.ship_agent_code = ml.ship_agent_code AND mlan.posted = 0) AS pending_ship_agent 
                            FROM manifest_header mh INNER JOIN dbo.manifest_line ml ON mh.manifest_no = ml.manifest_no WHERE mh.STATUS = 'CREATED' and ml.posted = 0 
                            AND ml.ship_agent_code = @ship_agent_code AND mh.location_id = @location_id", cn);
                da.SelectCommand.Parameters.AddWithValue("@ship_agent_code", ship_agent_code);
                da.SelectCommand.Parameters.AddWithValue("@location_id", location_id);
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

        internal async Task<DataTable> get_lines_with_ship_agent_code(HandoverLineNew handover_Line_New)
        {
            SqlConnection cn = null;
            try
            {
                string sub_manifest_no = "";
                for (int i = 0; i < handover_Line_New.sub_manifest_no.Length; i++)
                {
                    if (i == handover_Line_New.sub_manifest_no.Length - 1)
                    {
                        sub_manifest_no = sub_manifest_no + "'" + handover_Line_New.sub_manifest_no[i] + "'";
                    }
                    else
                    {
                        sub_manifest_no = sub_manifest_no + "'" + handover_Line_New.sub_manifest_no[i] + "',";
                    }
                };

                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT 'TRUE' as condition, 
                                                        '0'  AS selected_item,
                                                        'No' AS cancelled, 
                                                        mlan.manifest_no,
                                                        mlan.sub_manifest_no,
                                                        mlan.[sub_manifest_line_no],
                                                        mlan.ship_agent_code, 
                                                        mlan.awb_no, 
                                                        mlan.web_order_no
                                                        FROM dbo.manifest_line_awb_no mlan 
                                                        INNER JOIN manifest_header mh ON mh.manifest_no = mlan.manifest_no 
                                                        WHERE mh.location_id = " + handover_Line_New.LocationId + " and mh.status = 'CREATED' AND mlan.ship_agent_code = '" + handover_Line_New.ship_agent_code + "' " +
                                                        "AND mlan.sub_manifest_no IN (" + sub_manifest_no + ") AND mlan.posted = 0", cn);
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

        internal async Task<JArray> handover_create(handover_create handover_Create)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_handover_create", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@handover_selected_line", ToDataTable(handover_Create.handover_Line));
                smd.Parameters.AddWithValue("@location_id", handover_Create.location_id);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();
                smd.Dispose();
                return JArray.Parse(json);
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
        internal async Task<JArray> get_reprint_handover(string handover_no)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlCommand smd = new SqlCommand("manifest_handover_reprint", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                smd.Parameters.Add("@json_output", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                smd.Parameters.AddWithValue("@handover_no", @handover_no);
                // Execute the command
                await smd.ExecuteNonQueryAsync().ConfigureAwait(false);
                // Get the values
                string json = smd.Parameters["@json_output"].Value.ToString();
                smd.Dispose();
                return JArray.Parse(json);
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

        internal async Task<DataTable> get_ReportManifestPost(int location_id)
        {
            SqlConnection cn = null;
            try
            {
                cn = Connection.GetConnection();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT * FROM dbo.manifest_post mp WHERE mp.mark_manifested=0 and mp.location_id = " + location_id + "", cn);
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
