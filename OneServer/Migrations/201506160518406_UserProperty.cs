namespace OneServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastNameKana", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstNameKana", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FirstNameKana");
            DropColumn("dbo.AspNetUsers", "LastNameKana");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
        }
    }
}
