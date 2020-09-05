namespace OpenOrderFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Longitude");
            DropColumn("dbo.AspNetUsers", "Latitude");
        }
    }
}
