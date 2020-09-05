namespace OpenOrderFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        TokenDT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TokenID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tokens");
        }
    }
}
