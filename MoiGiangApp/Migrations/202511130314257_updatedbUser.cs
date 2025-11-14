namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedbUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsLock", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Img", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Img");
            DropColumn("dbo.AspNetUsers", "IsLock");
        }
    }
}
