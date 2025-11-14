namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNamHoc_HocKy_Nganh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HocKy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenHocKy = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NamHoc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenNamHoc = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nganh",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenNganh = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.LopHocPhan", "NamHocId", c => c.Int(nullable: false));
            AddColumn("dbo.LopHocPhan", "HocKyId", c => c.Int(nullable: false));
            AddColumn("dbo.LopHocPhan", "NganhId", c => c.Int(nullable: false));
            CreateIndex("dbo.LopHocPhan", "NamHocId");
            CreateIndex("dbo.LopHocPhan", "HocKyId");
            CreateIndex("dbo.LopHocPhan", "NganhId");
            AddForeignKey("dbo.LopHocPhan", "HocKyId", "dbo.HocKy", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LopHocPhan", "NamHocId", "dbo.NamHoc", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LopHocPhan", "NganhId", "dbo.Nganh", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LopHocPhan", "NganhId", "dbo.Nganh");
            DropForeignKey("dbo.LopHocPhan", "NamHocId", "dbo.NamHoc");
            DropForeignKey("dbo.LopHocPhan", "HocKyId", "dbo.HocKy");
            DropIndex("dbo.LopHocPhan", new[] { "NganhId" });
            DropIndex("dbo.LopHocPhan", new[] { "HocKyId" });
            DropIndex("dbo.LopHocPhan", new[] { "NamHocId" });
            DropColumn("dbo.LopHocPhan", "NganhId");
            DropColumn("dbo.LopHocPhan", "HocKyId");
            DropColumn("dbo.LopHocPhan", "NamHocId");
            DropTable("dbo.Nganh");
            DropTable("dbo.NamHoc");
            DropTable("dbo.HocKy");
        }
    }
}
