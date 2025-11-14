namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHocViTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HocVi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenHocVi = c.String(),
                        Mota = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "HocViId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "HocViId");
            AddForeignKey("dbo.AspNetUsers", "HocViId", "dbo.HocVi", "Id");
            DropColumn("dbo.AspNetUsers", "HocVi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "HocVi", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "HocViId", "dbo.HocVi");
            DropIndex("dbo.AspNetUsers", new[] { "HocViId" });
            DropColumn("dbo.AspNetUsers", "HocViId");
            DropTable("dbo.HocVi");
        }
    }
}
