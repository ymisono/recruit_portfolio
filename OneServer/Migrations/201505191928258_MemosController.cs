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
                        ApplicationUserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId_Id)
                .Index(t => t.ApplicationUserId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Memos", "ApplicationUserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Memos", new[] { "ApplicationUserId_Id" });
            DropTable("dbo.Memos");
        }
    }
}
