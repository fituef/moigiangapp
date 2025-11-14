using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	public class UserNganh
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int NganhId { get; set; }

		public virtual ApplicationUser User { get; set; }
		public virtual Nganh Nganh { get; set; }
	}
}