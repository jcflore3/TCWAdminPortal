namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFieldTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeaturedProperties", "BedRooms", c => c.Int(nullable: false));
            AlterColumn("dbo.FeaturedProperties", "Baths", c => c.Int(nullable: false));
            AlterColumn("dbo.FeaturedProperties", "BuildingSQFeet", c => c.Int(nullable: false));
            AlterColumn("dbo.FeaturedProperties", "LotSQFeet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeaturedProperties", "LotSQFeet", c => c.String());
            AlterColumn("dbo.FeaturedProperties", "BuildingSQFeet", c => c.String());
            AlterColumn("dbo.FeaturedProperties", "Baths", c => c.String());
            AlterColumn("dbo.FeaturedProperties", "BedRooms", c => c.String());
        }
    }
}
