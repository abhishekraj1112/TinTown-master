using System.Collections.Generic;

namespace TinTown.Models
{
    public class PickCreate
    {
        public string PickType { get; set; }
        public string DocumentType { get; set; }
        public string OrderType { get; set; }
        public string OrderNo { get; set; }
        public List<string> OrderNolist { get; set; }
        public List<string> State { get; set; }
        public List<string> City { get; set; }
        public string Pincode { get; set; }
        public List<string> Pincodelist { get; set; }
        public List<int> Priority { get; set; }
        public List<string> ShippingAgent { get; set; }
        public List<string> BinZone { get; set; }
        public int LocationId { get; set; }



        public int Marketplace { get; set; }
        public int NoofOrder { get; set; }
        public string EmailId { get; set; }





        public bool IsManual { get; set; }
        public List<document> document_no { get; set; }
        public List<string> Picker { get; set; }




    }

    public class document
    {
        public string document_no { get; set; }
        public string picker { get; set; }
    }
}
