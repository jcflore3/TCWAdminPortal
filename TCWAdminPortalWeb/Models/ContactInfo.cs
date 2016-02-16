using System;

namespace TCWAdminPortalWeb.Models
{
    public class ContactInfo
    {
        public int ID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
