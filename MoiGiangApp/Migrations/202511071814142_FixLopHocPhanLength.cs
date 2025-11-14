namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLopHocPhanLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LopHocPhan", "MaHP", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "TenHocPhan", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.LopHocPhan", "LopHP", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "MaLop", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.LopHocPhan", "Thu", c => c.String());
            AlterColumn("dbo.LopHocPhan", "GioHoc", c => c.String());
            AlterColumn("dbo.LopHocPhan", "Phong", c => c.String());
            AlterColumn("dbo.LopHocPhan", "MaGV", c => c.String());
            AlterColumn("dbo.LopHocPhan", "Loai", c => c.String());
            AlterColumn("dbo.LopHocPhan", "NgayBatDau", c => c.String());
            AlterColumn("dbo.LopHocPhan", "NgayKetThuc", c => c.String());
            AlterColumn("dbo.LopHocPhan", "DK", c => c.String());
            AlterColumn("dbo.LopHocPhan", "SL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LopHocPhan", "SL", c => c.String(maxLength: 100));
            AlterColumn("dbo.LopHocPhan", "DK", c => c.String(maxLength: 100));
            AlterColumn("dbo.LopHocPhan", "NgayKetThuc", c => c.String(maxLength: 200));
            AlterColumn("dbo.LopHocPhan", "NgayBatDau", c => c.String(maxLength: 200));
            AlterColumn("dbo.LopHocPhan", "Loai", c => c.String(maxLength: 20));
            AlterColumn("dbo.LopHocPhan", "MaGV", c => c.String(maxLength: 100));
            AlterColumn("dbo.LopHocPhan", "Phong", c => c.String(maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "GioHoc", c => c.String(maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "Thu", c => c.String(maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "MaLop", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.LopHocPhan", "LopHP", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.LopHocPhan", "TenHocPhan", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.LopHocPhan", "MaHP", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
