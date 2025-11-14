using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
	public class CreateUserViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		
		public string HoTen { get; set; }
		[Required]
		[Display(Name = "Mã GV")]
		public string LecturerID { get; set; }

		// Danh sách Role Id được chọn
		public List<string> SelectedRoles { get; set; } = new List<string>();
		public int? NganhId { get; set; }
		public string Nganhgd { get; set; }
	}

}