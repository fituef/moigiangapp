namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahih : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GiangVienDayNganh",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        NganhId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nganh", t => t.NganhId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.NganhId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GiangVienDayNganh", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GiangVienDayNganh", "NganhId", "dbo.Nganh");
            DropIndex("dbo.GiangVienDayNganh", new[] { "NganhId" });
            DropIndex("dbo.GiangVienDayNganh", new[] { "UserId" });
            DropTable("dbo.GiangVienDayNganh");
        }
    }
}
