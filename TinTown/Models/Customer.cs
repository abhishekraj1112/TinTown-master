using System.Collections.Generic;

namespace TinTown.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string PanNo { get; set; }
        public string GSTType { get; set; }
        public string GSTNo { get; set; }
        public List<CustomerAddress> Addresses { get; set; }
    }

    public class CustomerAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Country { get; set; }
        public string Village { get; set; }
        public string Taluka { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }

    }
}
