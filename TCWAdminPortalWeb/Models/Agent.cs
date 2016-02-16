using System;

namespace TCWAdminPortalWeb.Models
{
    public class Agent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public Boolean Enabled { get; set; }

    }
}
