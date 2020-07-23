using System;

namespace TinTown.Models
{
    public class GateEntry
    {
        public string LocationId { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverNumber { get; set; }
        public string Transporter { get; set; }
        public string LRNo { get; set; }
        public DateTime LRDate { get; set; }
        public string Freight { get; set; }
        public string FreightAmount { get; set; }
        public string VendorNo { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public string ChallanNo { get; set; }
        public string NoofBox { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
    }
}
