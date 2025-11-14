using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	[NotMapped]
	public class UserIndexViewModel
	{
		public IPagedList<UserRolesNganhViewModel> Users { get; set; }
		public string SearchString { get; set; }
		public string roleName { get; set; }
		public int? NganhID { get; set; }
		public int? HoViID { get; set; }
		public int? LoaiGVID { get; set; }
		public int? NganhDayID { get; set; }
	}
}