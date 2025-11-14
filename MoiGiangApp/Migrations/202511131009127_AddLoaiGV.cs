namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoaiGV : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoaiGV",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(),
                        Mota = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LoaiGVID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "LoaiGVID");
            AddForeignKey("dbo.AspNetUsers", "LoaiGVID", "dbo.LoaiGV", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "LoaiGVID", "dbo.LoaiGV");
            DropIndex("dbo.AspNetUsers", new[] { "LoaiGVID" });
            DropColumn("dbo.AspNetUsers", "LoaiGVID");
            DropTable("dbo.LoaiGV");
        }
    }
}
