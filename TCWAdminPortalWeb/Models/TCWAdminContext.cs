using System;
using System.Data.Entity;

namespace TCWAdminPortalWeb.Models
{
    public class TCWAdminContext : DbContext
    {
        public TCWAdminContext() : base("TCWAdminPortalContext")
        {

        }

        public TCWAdminContext(string connString) : base(connString)
        {
        }

        public DbSet<FeaturedProperty> FeaturedProperties { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}
