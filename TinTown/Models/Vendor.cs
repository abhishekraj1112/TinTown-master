using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TinTown.Models
{
    public class Vendor
    {

    }
    public class VendorModel
    {

        public string type { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public long mobile_no { get; set; }
        public long phone_no { get; set; } = 0;
        public List<IFormFile> file { get; set; }
        public string email { get; set; }
        public string contact_person_name { get; set; }
        public long cntct_prn_cntct_no { get; set; }
        public string oprtn_prsn_name { get; set; }
        public long oprtn_prsn_cntct_no { get; set; }
        public string accnt_prsn_name { get; set; }
        public long accnt_prsn_cntct_no { get; set; }
        public string warehouse_prsn_name { get; set; }
        public long warehouse_prsn_cntct_no { get; set; }
        public string prchase_order_prsn_name { get; set; }
        public long prchase_order_cntct_no { get; set; }
        public string home_page { get; set; }
        public string gst_type { get; set; }
        public string gst_no { get; set; }
        public string pan_no { get; set; }
        public string currency { get; set; }
        public string created_by { get; set; }


    }

    public class FileModel
    {
        public long id { get; set; }
        public string file { get; set; }
    }

    public class VendorCatalogueModel
    {
        public string vendor_no { get; set; }
        public string item_code { get; set; }
        public string vendor_item_code { get; set; }
        public float cost_per_unit { get; set; }
        public string description { get; set; }
        public string created_by { get; set; }
        public string update_by { get; set; }
        public string flag { get; set; }

    }
}
