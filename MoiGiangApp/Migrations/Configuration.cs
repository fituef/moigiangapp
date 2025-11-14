namespace MoiGiangApp.Migrations
{
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using MoiGiangApp.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<MoiGiangApp.Models.ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(ApplicationDbContext context)
		{
			// Năm học
			if (!context.NamHocs.Any())
			{
				context.NamHocs.AddRange(new[]
				{
				new NamHoc { TenNamHoc = "2025-2026" },
				new NamHoc { TenNamHoc = "2026-2027" },
				new NamHoc { TenNamHoc = "2027-2028" }
			});
			}

			// Học kỳ
			if (!context.HocKys.Any())
			{
				context.HocKys.AddRange(new[]
				{
				new HocKy { TenHocKy = "HK1" },
				new HocKy { TenHocKy = "HK2" },
				new HocKy { TenHocKy = "HK3" }
			});
			}

			// Ngành
			if (!context.Nganhs.Any())
			{
				context.Nganhs.AddRange(new[]
				{
				new Nganh { TenNganh = "CÔNG NGHỆ THÔNG TIN" },
				new Nganh { TenNganh = "KỸ THUẬT PHẦN MỀM" },
				new Nganh { TenNganh = "THIẾT KẾ ĐỒ HỌA" },
				new Nganh { TenNganh = "THƯƠNG MẠI ĐIỆN TỬ" },
				new Nganh { TenNganh = "AN NINH MẠNG" },
				new Nganh { TenNganh = "KHOA HỌC DỮ LIỆU" },
				new Nganh { TenNganh = "TIN HỌC ỨNG DỤNG" }
			});
			}

			context.SaveChanges();
			CreateRolesAndUsers(context);
		}

		private void CreateRolesAndUsers(ApplicationDbContext context)
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			// Tạo các role
			string[] roleNames = { "Admin", "TruongKhoa", "ThuKyKhoa", "TruongNganhCNTT", "TruongNganhKHDL","PhoKhoa","TruongNganhTMDT" };
			foreach (var roleName in roleNames)
			{
				if (!roleManager.RoleExists(roleName))
					roleManager.Create(new IdentityRole(roleName));
			}

			// Tạo Admin mặc định
			if (userManager.FindByEmail("admin@moigiang.vn") == null)
			{
				var adminUser = new ApplicationUser { UserName = "admin@moigiang.vn", Email = "admin@moigiang.vn" };
				userManager.Create(adminUser, "Admin@1234");
				userManager.AddToRole(adminUser.Id, "Admin");
			}

			// Danh sách email user mới
			var newUsers = new List<(string Email, string Role)>
	{
		("hieuhv@uef.edu.vn", "TruongNganhCNTT"),
		("giangnh@uef.edu.vn", "TruongKhoa"),
		("linhnth@uef.edu.vn", "TruongNganhKHDL"),
		("trangvtt@uef.edu.vn", "PhoKhoa"),
		("hanght@uef.edu.vn", "TruongNganhTMDT")
	};

			foreach (var (Email, Role) in newUsers)
			{
				if (userManager.FindByEmail(Email) == null)
				{
					var user = new ApplicationUser { UserName = Email, Email = Email };
					userManager.Create(user, "Admin@1234");
					userManager.AddToRole(user.Id, Role);
				}
			}
		}
	}
}
