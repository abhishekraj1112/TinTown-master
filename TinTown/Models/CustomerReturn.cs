using System.Collections.Generic;

namespace TinTown.Models
{
    public class CustomerReturn
    {
        public string Type { get; set; }
        public string ParentOrderNo { get; set; }
        public string ParentInvoiceNo { get; set; }
        public int LocationId { get; set; }
        public List<OrderLine> lines { get; set; }

    }

    public class OrderLine
    {
        public int sales_order_line_no { get; set; }
    }
}
