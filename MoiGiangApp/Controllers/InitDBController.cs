using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MoiGiangApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoiGiangApp.Controllers
{
    public class InitDBController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		public ActionResult InitializeRolesForUsers()
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

			// Danh sách Role cần tạo
			var roles = new[]
			{
		new { Id="fb0d211e-f743-4980-aca5-5b97c2b40298", Name="Admin" },
		new { Id="8d9ec458-c5d8-4587-98ca-2ece41f35dd4", Name="TruongKhoa" },
		new { Id="4bb1086b-8b41-4d53-ad5f-679a82afa89e", Name="ThuKyKhoa" },
		new { Id="73f77701-3e18-4076-b47a-3bcdd4c2e066", Name="TruongNganhKHDL" },
		new { Id="742e3688-2e64-4365-aa9d-6b4ad6a74216", Name="TruongNganhTMDT" },
		new { Id="73268048-338a-41c6-b8bf-55f3853aa952", Name="TruongNganhCNTT" }
	};

			// Tạo role nếu chưa có
			foreach (var r in roles)
			{
				if (!roleManager.RoleExists(r.Name))
				{
					roleManager.Create(new IdentityRole { Id = r.Id, Name = r.Name });
				}
			}

			// Danh sách User và Role tương ứng
			var userRoles = new[]
			{
		new { Email="hieuhv@uef.edu.vn", RoleName="Admin" },
		new { Email="giangnh@uef.edu.vn", RoleName="TruongKhoa" },
		new { Email="linhnth@uef.edu.vn", RoleName="TruongNganhKHDL" },
		new { Email="trangvtt@uef.edu.vn", RoleName="TruongNganhTMDT" },
		new { Email="hanght@uef.edu.vn", RoleName="ThuKyKhoa" }
	};

			foreach (var ur in userRoles)
			{
				var user = userManager.FindByEmail(ur.Email);
				if (user != null && !userManager.IsInRole(user.Id, ur.RoleName))
				{
					userManager.AddToRole(user.Id, ur.RoleName);
				}
			}

			return Content("Role đã được tạo và gán cho User thành công!");
		}

	}
}