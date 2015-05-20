namespace OneServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Memo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Memos", "ApplicationUserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Memos", new[] { "ApplicationUserId_Id" });
            AddColumn("dbo.Memos", "OwnerId", c => c.String());
            DropColumn("dbo.Memos", "ApplicationUserId_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memos", "ApplicationUserId_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Memos", "OwnerId");
            CreateIndex("dbo.Memos", "ApplicationUserId_Id");
            AddForeignKey("dbo.Memos", "ApplicationUserId_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
