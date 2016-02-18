namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedThumbnailURLPropertytoModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeaturedProperties", "ThumbnailURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeaturedProperties", "ThumbnailURL");
        }
    }
}
