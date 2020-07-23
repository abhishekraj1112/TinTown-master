using System;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Purchase
    {
        public string VendorNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ExpiryDateReceipt { get; set; }
        public string PaymentTerms { get; set; }
        public int LocationId { get; set; }
        public string CreatedBy { get; set; }
        public string RejectionReason { get; set; }
        public string Orderstatus { get; set; }
        public List<CompletePO> lines { get; set; }
    }

    public class AddPO
    {
        public string VendorNo { get; set; }
        public string ItemNo { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

    public class CompletePO
    {
        public string DocumentNo { get; set; }
        public string item_no { get; set; }
        public int quantity { get; set; }
        public decimal cost_per_unit { get; set; }
        public decimal amount { get; set; }
        public decimal discount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal total_amount { get; set; }
        public decimal grand_total { get; set; }
        public int is_expire_date { get; set; }
        public int is_vendor_lotno { get; set; }
    }

}
