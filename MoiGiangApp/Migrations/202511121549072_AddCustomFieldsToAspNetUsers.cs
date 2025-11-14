namespace MoiGiangApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

	public partial class AddCustomFieldsToAspNetUsers : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.AspNetUsers", "LecturerID", c => c.String(maxLength: 128));
			AddColumn("dbo.AspNetUsers", "IsGiangVien", c => c.Boolean(nullable: false, defaultValue: false));
		}

		public override void Down()
		{
			 
			DropColumn("dbo.AspNetUsers", "MaGV");
		}
	}
}
