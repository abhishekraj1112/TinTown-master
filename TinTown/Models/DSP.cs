using System.Collections.Generic;

namespace TinTown.Models
{
    public class DSP
    {
        public int LocationId { get; set; }
        public string DSPCode { get; set; }
        public string Description { get; set; }
        public string APIUrl { get; set; }
        public string Address { get; set; }
        public string GSTNo { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Flag { get; set; }
        public string PaymentType { get; set; }
        public string PageNo { get; set; }
        public List<AWB> AWBNo { get; set; }

    }

    public class DSPService
    {
        public int LocationId { get; set; }
        public string Pincode { get; set; }
        public string DSPCode { get; set; }
        public bool Express { get; set; }
        public bool ReversePickUp { get; set; }
        public int Priority { get; set; }
        public float Cost { get; set; }
        public string Flag { get; set; }
    }
    public class AWB
    {
        public string AWBNo { get; set; }
    }
}
