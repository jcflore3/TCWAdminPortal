using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace TCWAdminPortalWeb.Models
{
    public class TCWAdminContext : IdentityDbContext<TCWPortalUser>
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

        public static TCWAdminContext Create()
        {
            return new TCWAdminContext();
        }
    }
}
