namespace TCWAdminPortalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        ImageUrl = c.String(),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FeaturedProperties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImageURL = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        BedRooms = c.String(),
                        Baths = c.String(),
                        BuildingSQFeet = c.String(),
                        LotSQFeet = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FeaturedProperties");
            DropTable("dbo.ContactInfoes");
            DropTable("dbo.Agents");
        }
    }
}
