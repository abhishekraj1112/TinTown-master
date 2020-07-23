namespace TinTown.Models
{
    public class Putway
    {
        public string GRNHeaderNo { get; set; }
        public string Barcode { get; set; }
        public string Bincode { get; set; }
        public string PutwayHeaderNo { get; set; }
        public int Quantity { get; set; }

        public string ItemNo { get; set; }
        public string CreatedBy { get; set; }
        public string PutwayType { get; set; }
        public int LocationId { get; set; }
        public string Type { get; set; }
        public string ExpiryDate { get; set; }
        public string VendorLot { get; set; }

    }

    public class Delete
    {
        public string putway_no { get; set; }
        public string putway_line_no { get; set; }
    }
}
