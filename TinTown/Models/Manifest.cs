using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Manifest
    {
        public string manifest_no { get; set; }
        public string ship_agent_code { get; set; }
        public string created_by { get; set; }
        public int location_id { get; set; }
    }



    public class array_wrapper
    {
        public JArray Manifest_Data { get; set; }
        public JArray Manifest_Ship_Data { get; set; }
        public JArray Manifest_Pending_Data { get; set; }
    }

    public class Post_Manifest
    {
        public string awb_no { get; set; }
        public string created_by { get; set; }
        public int location_id { get; set; }
    }

    public class handover_create
    {
        public int location_id { get; set; }
        public List<handover_line> handover_Line { get; set; }
    }

    public class handover_line
    {
        public string manifest_no { get; set; }
        public string sub_manifest_no { get; set; }
        public string sub_manifest_line_no { get; set; }
        public string web_order_no { get; set; }
        public string awb_no { get; set; }
        public string ship_agent_code { get; set; }
        public string created_by { get; set; }
        public string vehicle_no { get; set; }
    }

    public class HandoverLineNew
    {
        public string[] sub_manifest_no { get; set; }
        public string ship_agent_code { get; set; }
        public int LocationId { get; set; }
    }

}
