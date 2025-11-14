using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	public class Nganh
	{
		[Key]
		public int Id { get; set; }
		[Required, StringLength(50)]
		public string TenNganh { get; set; } // CNTT, TMĐT
	}
}