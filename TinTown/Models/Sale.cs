
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Sale
    {
        public string SaleInvoiceNo { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public int LocationId { get; set; }
        public string SaleOrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string RejectionReason { get; set; }
        public string Orderstatus { get; set; }
        public string CustomerNo { get; set; }
        public string BillToAddress { get; set; }
        public string ShipToAddress { get; set; }
        public string PaymentTerms { get; set; }
        public List<CompleteSO> lines { get; set; }
    }

    public class AddSO
    {
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal CostPerUnit { get; set; }
    }

    public class CompleteSO
    {
        public decimal amount { get; set; }
        public decimal cost_per_unit { get; set; }
        public decimal discount { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal grand_total { get; set; }
        public decimal gst_amount { get; set; }
        public decimal gst_percentage { get; set; }
        public string item_no { get; set; }
        public int quantity { get; set; }
        public decimal total_amount { get; set; }
    }

    public class SaleInfoResponse
    {
        public JArray Sales { get; set; }
        public JObject BilTo { get; set; }
        public JObject ShioTo { get; set; }
    }
}
