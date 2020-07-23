using Newtonsoft.Json.Linq;

namespace TinTown.Models
{
    public class IQC
    {
        public string Barcode { get; set; }
        public string GRNHeaderNo { get; set; }
        public string GRNLineNo { get; set; }
        public string Bincode { get; set; }
        public string RejectionReason { get; set; }
        public string VendorLotNo { get; set; }
        public string ExpireDate { get; set; }
        public int Quantity { get; set; }
        public int LocationId { get; set; }
    }

    public class IQCResponse
    {
        public JArray Header { get; set; }
        public JArray Lines { get; set; }
    }
}
