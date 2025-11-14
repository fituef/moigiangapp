// File: Models/GiangVien.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoiGiangApp.Models
{
	[Table("GiangVien")]
	public class GiangVien
	{
		[Key]
		public int Id { get; set; }

		[Required, StringLength(20), Index(IsUnique = true)]
		public string MaGV { get; set; } // 12345678

		[Required, StringLength(100)]
		public string TenGV { get; set; } // Nguyễn Văn A

		[StringLength(100), EmailAddress]
		public string Email { get; set; }

		[StringLength(20)]
		public string SoDienThoai { get; set; }

		// Quan hệ: 1 GV dạy nhiều lớp
		public virtual ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();
	}
}