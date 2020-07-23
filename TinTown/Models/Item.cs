namespace TinTown.Models
{
    public class Item
    {
        public string item_no { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string purchaseUom { get; set; }
        public string saleUom { get; set; }
        public int base_uom { get; set; }
        public int category { get; set; }
        public int subCategory { get; set; }
        public float unitPrice { get; set; }
        public int gstGroupId { get; set; }
        public int gstHsnCode { get; set; }
        public float costPerUnit { get; set; }
        public float mrp { get; set; }
        public string image_url { get; set; }
        public string flag { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public string Location_id { get; set; }
    }

    public class ItemCategoryModel
    {

        public string code { get; set; }
        public string name { get; set; }
        public long SubId { get; set; }
        public string description { get; set; }
        public string flag { get; set; }
        public string created_by { get; set; }

        public string updated_by { get; set; }


    }

    public class itemCategorydeleteModel
    {
        public long Id { get; set; }
    }

    public class itemattributetypeModel
    {
        public long attribute_no { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string flag { get; set; }
        public string created_by { get; set; }

        public string updated_by { get; set; }

    }

    public class itemattributevalueModel
    {
        public long attribute_type_no { get; set; }
        public string value { get; set; }
        public long attribute_value_no { get; set; }
        public string description { get; set; }
        public string flag { get; set; }
        public string created_by { get; set; }

        public string updated_by { get; set; }

    }
    public class itemAttributedeleteModel
    {
        public long attribute_no { get; set; }
    }


}
