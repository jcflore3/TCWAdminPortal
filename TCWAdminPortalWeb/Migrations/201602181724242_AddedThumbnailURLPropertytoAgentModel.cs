namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedThumbnailURLPropertytoAgentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "ThumbnailURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agents", "ThumbnailURL");
        }
    }
}
