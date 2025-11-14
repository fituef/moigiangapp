namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGV : DbMigration
    {
        public override void Up()
        {
			//DropTable("dbo.GiangVien");
		}
        
        public override void Down()
        {
        }
    }
}
