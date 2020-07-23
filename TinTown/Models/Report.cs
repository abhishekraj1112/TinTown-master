using System.Data;

namespace TinTown.Models
{
    public class Report
    {
        public string condition { get; set; }
        public string message { get; set; }
        public DataTable sale_headers { get; set; }
        public DataTable sale_lines { get; set; }
    }

    public class SlotReport
    {
        public string flag { get; set; }
        public string zone { get; set; }
        public string order_no { get; set; }
        public int LocationId { get; set; }
    }
}
