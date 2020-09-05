namespace OpenOrderFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Slot", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Slot");
        }
    }
}
