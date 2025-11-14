using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	public class NamHoc
	{
		[Key]
		public int Id { get; set; }
		[Required, StringLength(20)]
		public string TenNamHoc { get; set; } // 2025-2026
	}
}