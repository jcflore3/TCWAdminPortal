namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedEnabledFlagsAndGarage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "Enabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.FeaturedProperties", "FullBaths", c => c.Int(nullable: false));
            AddColumn("dbo.FeaturedProperties", "HalfBaths", c => c.Int(nullable: false));
            AddColumn("dbo.FeaturedProperties", "Garage", c => c.Int(nullable: false));
            AddColumn("dbo.FeaturedProperties", "Enabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.FeaturedProperties", "Baths");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FeaturedProperties", "Baths", c => c.Int(nullable: false));
            DropColumn("dbo.FeaturedProperties", "Enabled");
            DropColumn("dbo.FeaturedProperties", "Garage");
            DropColumn("dbo.FeaturedProperties", "HalfBaths");
            DropColumn("dbo.FeaturedProperties", "FullBaths");
            DropColumn("dbo.Agents", "Enabled");
        }
    }
}
