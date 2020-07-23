using System.Collections.Generic;
using System.Data;

namespace TinTown.Models
{
    public class OutboundQualityCheck
    {
        public string Barcode { get; set; }
        public string SortingZone { get; set; }
        public string Marketplace { get; set; }
        public string EmailID { get; set; }
        public int LocationId { get; set; }
    }

    public class ShipmentAgent
    {
        public List<ShipList> value { get; set; }
        public class ShipList
        {
            public string condition { get; set; } = "True";
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }

    public class OQCPost
    {
        public string PickNo { get; set; }
        public int PickLineNo { get; set; }
        public int GoodQty { get; set; }
        public int BadQty { get; set; }
        public int MissingQty { get; set; }
        public string EmailID { get; set; }
        public float Weight { get; set; }
        public string PackingMaterial { get; set; }
    }

    public class OQCResponse
    {
        public string condition { get; set; }
        public string message { get; set; }
        public string pick_no { get; set; }
        public string pick_line_no { get; set; }
        public string shipment_no { get; set; }
        public string order_no { get; set; }
        public string barcode { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string style { get; set; }
        public string qty_ordered { get; set; }
        public string pick_status { get; set; }
        public string sorting_zone { get; set; }
        public string marketplace_name { get; set; }
        public string qty_picked { get; set; }
        public string oqc_good_qty { get; set; }
        public string oqc_bad_qty { get; set; }
        public string oqc_miss_qty { get; set; }
        public string oqc_person_id { get; set; }
        public string oqc_date { get; set; }
        public string is_cancelled { get; set; }
        public string order_processed { get; set; }
        public ImageResponse images { get; set; }
    }

    public class MultiOQCPostLines
    {
        public string PickNo { get; set; }
        public int PickLineNo { get; set; }
        public int GoodQty { get; set; }
        public int BadQty { get; set; }
        public int MissingQty { get; set; }
    }

    public class MultiOQCPost
    {
        public string EmailId { get; set; }
        public string OrderNo { get; set; }
        public float Weight { get; set; }
        public string PackingMaterial { get; set; }
        public List<MultiOQCPostLines> lines { get; set; }
    }

    public class MultiOQCLines
    {
        public string pick_no { get; set; }
        public string pick_line_no { get; set; }
        public string barcode { get; set; }
        public string qty_picked { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string style { get; set; }
        public string is_cancelled { get; set; }
        public ImageResponse images { get; set; }
        public string goodQty { get; set; }
        public string badQty { get; set; }
        public string missQty { get; set; }
        public string qty_ordered { get; set; }
        public string pick_status { get; set; }

    }

    public class MultiOQCResponse
    {
        public DataTable header { get; set; }
        public List<MultiOQCLines> lines { get; set; }
    }

    public class OQCRange
    {
        public string Zone { get; set; }
        public string Email { get; set; }
        public int LocationId { get; set; }
    }
}
