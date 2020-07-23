using Newtonsoft.Json.Linq;

namespace TinTown.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public int roleId { get; set; }
        public string condition { get; set; }
        public string message { get; set; }
        public int locationId { get; set; }
        public string location_name { get; set; }
        public int shiftID { get; set; }
        public string WorkType { get; set; }
        public string PrinterIP { get; set; }
        public int PrinterPort { get; set; }
        public int gateentry { get; set; }
        public string barcode { get; set; }
        public string shipment { get; set; }
        public string pick { get; set; }
        public JArray menu { get; set; }
    }
}
