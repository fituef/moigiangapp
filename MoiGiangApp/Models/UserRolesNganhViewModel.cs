using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoiGiangApp.Models
{
	[NotMapped]
	public class UserRolesNganhViewModel
	{
		public string Id { get; set; } // User Id
		public string HoTen { get; set; }
		public string Email { get; set; }
		public string MaGV { get; set; }
		public string Hinh { get; set; }
		public string TenHocVi { get; set; }
        public string TenLoaiGV { get; set; }
        public SelectList LoaiGVList { get; set; }
        public bool LockoutEnabled { get; set; } // Lock/Unlock status
		public List<string> Roles { get; set; }
		public List<string> Nganhs { get; set; }
		public List<string> NganhDays { get; set; }
	}


}