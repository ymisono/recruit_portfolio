namespace OneServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemosController : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Memos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        OwnerId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Memos", "ApplicationUserId_Id", "dbo.AspNetUsers");
            //DropIndex("dbo.Memos", new[] { "ApplicationUserId_Id" });
            DropTable("dbo.Memos");
        }
    }
}
