using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class TransferOrder
    {
        public int LocationId { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
    }

    public class TransferComplete
    {
        public string TransferNo { get; set; }
        public string CreatedBy { get; set; }
        public int FromLocation { get; set; }
        public int ToLocation { get; set; }
        public List<TransferLine> Lines { get; set; }
    }

    public class TransferLine
    {
        public decimal amount_with_tax { get; set; }
        public decimal amount_without_tax { get; set; }
        public decimal cost_per_unit { get; set; }
        public decimal gst_amount { get; set; }
        public decimal gst_percentage { get; set; }
        public string item_no { get; set; }
        public int quantity { get; set; }
        public decimal transfer_cost { get; set; }
    }


    public class TransferOrderInfoResponse
    {
        public JArray TransferOrder { get; set; }
        public JObject FromLocation { get; set; }
        public JObject ToLocation { get; set; }
    }

}
