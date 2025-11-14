namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GiangVien",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaGV = c.String(nullable: false, maxLength: 20),
                        TenGV = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        SoDienThoai = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.MaGV, unique: true);
            
            CreateTable(
                "dbo.LopHocPhan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaHP = c.String(nullable: false, maxLength: 20),
                        TenHocPhan = c.String(nullable: false, maxLength: 100),
                        LopHP = c.String(nullable: false, maxLength: 10),
                        MaLop = c.String(nullable: false, maxLength: 50),
                        SiSo = c.Int(nullable: false),
                        Thu = c.String(maxLength: 50),
                        Tiet = c.Int(nullable: false),
                        GioHoc = c.String(maxLength: 50),
                        Tang = c.Int(nullable: false),
                        Phong = c.String(maxLength: 50),
                        MaGV = c.String(maxLength: 100),
                        Loai = c.String(maxLength: 20),
                        NgayBatDau = c.String(maxLength: 200),
                        NgayKetThuc = c.String(maxLength: 200),
                        Tuan = c.Int(nullable: false),
                        DK = c.String(maxLength: 100),
                        SL = c.String(maxLength: 100),
                        GiangVienId = c.Int(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GiangVien", t => t.GiangVienId)
                .Index(t => t.GiangVienId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LopHocPhan", "GiangVienId", "dbo.GiangVien");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LopHocPhan", new[] { "GiangVienId" });
            DropIndex("dbo.GiangVien", new[] { "MaGV" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LopHocPhan");
            DropTable("dbo.GiangVien");
        }
    }
}
