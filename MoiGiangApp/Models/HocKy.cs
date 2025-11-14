using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	public class HocKy
	{
		[Key]
		public int Id { get; set; }
		[Required, StringLength(10)]
		public string TenHocKy { get; set; } // HK1, HK2
	}
}