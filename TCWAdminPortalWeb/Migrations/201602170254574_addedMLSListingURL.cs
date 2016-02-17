namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMLSListingURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeaturedProperties", "MLSListingURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeaturedProperties", "MLSListingURL");
        }
    }
}
