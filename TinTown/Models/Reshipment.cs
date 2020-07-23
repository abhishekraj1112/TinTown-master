using System.Collections.Generic;

namespace TinTown.Models
{
    public class Reshipment
    {
        public string Type { get; set; }
        public int LocationId { get; set; }
        public List<Selection> lines { get; set; }
    }

    public class Selection
    {
        public string sales_invoice_no { get; set; }
        public string old_dsp { get; set; }
        public string old_awb { get; set; }
        public string customer_id { get; set; }
    }
}
