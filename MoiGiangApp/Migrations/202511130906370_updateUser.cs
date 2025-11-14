namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HocVi", c => c.String());
            AddColumn("dbo.AspNetUsers", "DiaChiTT", c => c.String());
            AddColumn("dbo.AspNetUsers", "DaiChi", c => c.String());
            AddColumn("dbo.AspNetUsers", "STK", c => c.String());
            AddColumn("dbo.AspNetUsers", "TenTK", c => c.String());
            AddColumn("dbo.AspNetUsers", "TenNH", c => c.String());
            AddColumn("dbo.AspNetUsers", "ChiNhanh", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ChiNhanh");
            DropColumn("dbo.AspNetUsers", "TenNH");
            DropColumn("dbo.AspNetUsers", "TenTK");
            DropColumn("dbo.AspNetUsers", "STK");
            DropColumn("dbo.AspNetUsers", "DaiChi");
            DropColumn("dbo.AspNetUsers", "DiaChiTT");
            DropColumn("dbo.AspNetUsers", "HocVi");
        }
    }
}
