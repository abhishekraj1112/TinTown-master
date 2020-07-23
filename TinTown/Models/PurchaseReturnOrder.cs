using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class PurchaseReturnOrder
    {
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public int LocationId { get; set; }
    }

    public class PurchaseReturnOrderComplete
    {
        public string PurchaseNo { get; set; }
        public string CreatedBy { get; set; }
        public int FromLocation { get; set; }
        public int VendorNo { get; set; }
        public List<PurchaseLine> Lines { get; set; }
    }

    public class PurchaseLine
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

    public class PurchaseReturnOrderResponse {

        public JArray PurchaseReturnOrderInfo { get; set; }
        public JArray VendorDetail { get; set; }
        public JArray LocationDetail { get; set; }

    }
}
