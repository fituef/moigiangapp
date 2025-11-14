using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoiGiangApp.Models
{
	public class UserEditViewModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập họ tên")]
		[Display(Name = "Họ Tên")]
		public string HoTen { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập email")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Mã Giảng Viên")]
		public string MaGV { get; set; }

		[Display(Name = "Hình Ảnh")]
		public string Hinh { get; set; }

		[Display(Name = "Khóa tài khoản")]
		public bool IsLock { get; set; }

		public List<string> SelectedRoleIds { get; set; } = new List<string>();
		public List<string> SelectedNganhIds { get; set; } = new List<string>();
        public List<string> SelectedDayNganhIds { get; set; } = new List<string>();
        // Thông tin bổ sung
        [Display(Name = "Học Vị")]
        public int? HocViId { get; set; }

        [Display(Name = "Loại giảng viên")]
        public int? LoaiGVId { get; set; }
        [Display(Name = "Địa Chỉ Thường Trú")]
        public string DiaChiTT { get; set; }

        [Display(Name = "Địa Chỉ Liên Hệ")]
        public string DaiChi { get; set; }

        [Display(Name = "Số Tài Khoản")]
        public string STK { get; set; }

        [Display(Name = "Tên Tài Khoản")]
        public string TenTK { get; set; }

        [Display(Name = "Tên Ngân Hàng")]
        public string TenNH { get; set; }

        [Display(Name = "Chi Nhánh")]
        public string ChiNhanh { get; set; }
       

        public SelectList DayNganhList { get; set; }
        // Dropdown data
        public SelectList RoleList { get; set; }
		public SelectList NganhList { get; set; }
        public SelectList HocViList { get; set; }
        public SelectList LoaiGVList { get; set; }
    }
}