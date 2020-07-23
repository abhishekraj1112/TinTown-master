using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TinTown.Models;

namespace TinTown.EntryCode
{
    public class BaseCode
    {

        public async Task<JsonResult> SendRespose(string condition, string message)  // universal response send function to UI
        {
            try
            {
                JObject Jobj = new JObject
                {
                    ["condition"] = condition,
                    ["message"] = message
                };
                JArray jArray = new JArray
                {
                    Jobj
                };
                return new JsonResult(jArray);
            }
            catch (Exception ee)
            {
                return new JsonResult(ee);
            }
        }

        //Convert DataTable to JsonResult//
        public JsonResult DataTableToJsonWithJsonNet(DataTable table)
        {
            JArray array = JArray.Parse(JsonConvert.SerializeObject(table));
            return new JsonResult(array);
        }

        public DataTable ToDataTable<T>(List<T> items)  // convert List object to datatable 
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    Type type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                    object[] values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            catch (Exception)
            {
                DataTable dt = new DataTable();
                return dt;
            }
        }

        public DataTable ConvertListToDataTable(List<string> list) // convert string list to datatable
        {
            try
            {
                DataTable table = new DataTable();
                for (int i = 0; i < 1; i++)
                {
                    table.Columns.Add();
                }
                if (list != null)
                {
                    foreach (string array in list)
                    {
                        table.Rows.Add(array);
                    }
                }

                return table;
            }
            catch (Exception)
            {
                DataTable dt = new DataTable();
                return dt;
            }
        }

        public DataTable ConvertListToDataTable(List<int> list) // convert int list to datatable
        {
            try
            {
                DataTable table = new DataTable();
                for (int i = 0; i < 1; i++)
                {
                    table.Columns.Add();
                }
                if (list != null)
                {
                    foreach (int array in list)
                    {
                        table.Rows.Add(array);
                    }
                }
                return table;
            }
            catch (Exception)
            {
                DataTable dt = new DataTable();
                return dt;
            }
        }

        //Following function will return Distinct records for Pick header no, parent pick no and assigne to column.
        public DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            dtUniqRecords.AcceptChanges();
            return dtUniqRecords;
        }

        public List<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (DataRow row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }





        public async Task<ImageResponse> GetImage(string barcode)
        {
            ImageResponse imageResponse = new ImageResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    string zivame_image_url = Startup._inappuse.GetValue<string>("Modules:ZivameImageUrl:url"); // read url 

                    httpClient.Timeout = TimeSpan.FromMilliseconds(1);

                    Uri uri = new Uri(zivame_image_url + barcode, UriKind.Absolute);
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        imageResponse = JsonConvert.DeserializeObject<ImageResponse>(apiResponse);
                    }
                    if (imageResponse.Message.Equals("No Images", StringComparison.OrdinalIgnoreCase))
                    {
                        imageResponse = NoImage();
                    }
                }
                return imageResponse;
            }
            catch (Exception)
            {
                return NoImage();
            }
        }

        public ImageResponse NoImage()
        {
            try
            {
                string zivame_image_url = Startup._inappuse.GetValue<string>("Modules:NoImageUrl:url"); // read url 
                ImageResponse imageResponse = new ImageResponse
                {
                    img1 = zivame_image_url,
                    img2 = zivame_image_url,
                    img3 = zivame_image_url,
                    img4 = zivame_image_url
                };
                return imageResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }





        #region convert_Datatable_to_list_of_you_given_model
        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        //List<Student> studentDetails = new List<Student>();
        //studentDetails = ConvertDataTable<Student>(dt); 
        #endregion 
    }
}
