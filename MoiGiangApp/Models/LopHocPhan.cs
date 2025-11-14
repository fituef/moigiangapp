// File: Models/LopHocPhan.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoiGiangApp.Models
{
	[Table("LopHocPhan")]
	public class LopHocPhan
	{
		[Key]
		public int Id { get; set; }

		// --- Thông tin học phần ---
		[Required, StringLength(20)]
		public string MaHP { get; set; } // ITE1115E

		[Required, StringLength(100)]
		public string TenHocPhan { get; set; } // Hệ điều hành

		[Required, StringLength(10)]
		public string LopHP { get; set; } // A01E

		[Required, StringLength(50)]
		public string MaLop { get; set; } // 24D1ITE01

		public int SiSo { get; set; } // 35

		// --- Lịch học (gộp) ---
		[StringLength(50)]
		public string Thu { get; set; } // 2/5 hoặc 2/4/6

		public int Tiet { get; set; } // 7, 10

		[StringLength(50)]
		public string GioHoc { get; set; } // 13:00 - 15:20

		public int Tang { get; set; } // 3

		[StringLength(50)]
		public string Phong { get; set; } // A.10-07 / A.20-03

		// --- Giảng viên (gán sau) ---
		[StringLength(100)]
		public string MaGV { get; set; } // 12345678 / 2345678 (giữ từ Excel)

		[StringLength(20)]
		public string Loai { get; set; } // LT / Máy tính

		// --- Thời gian ---
		[StringLength(200)]
		public string NgayBatDau { get; set; } // 3/2/2026 - 3/5/2026

		[StringLength(200)]
		public string NgayKetThuc { get; set; } // 4/20/2026 - 4/16/2026

		public int Tuan { get; set; } // 27

		[StringLength(100)]
		public string DK { get; set; } // 34-33

		[StringLength(100)]
		public string SL { get; set; } // 24-21

		// --- Gán giảng viên chính (1-1) ---
		public string GiangVienId { get; set; }
		public virtual ApplicationUser GiangVien { get; set; }

		public int NamHocId { get; set; }
		public virtual NamHoc NamHoc { get; set; }

		public int HocKyId { get; set; }
		public virtual HocKy HocKy { get; set; }

		public int NganhId { get; set; }
		public virtual Nganh Nganh { get; set; }

		// --- Audit ---
		public DateTime NgayTao { get; set; } = DateTime.Now;
		[NotMapped]
		public string MauNhom { get; set; } = "bg-light"; // Màu nền nhóm
	}
}