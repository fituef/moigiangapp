namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HoTen", c => c.String());
            AddColumn("dbo.AspNetUsers", "Khoa", c => c.String());
            AddColumn("dbo.AspNetUsers", "Nganh", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Nganh");
            DropColumn("dbo.AspNetUsers", "Khoa");
            DropColumn("dbo.AspNetUsers", "HoTen");
        }
    }
}
