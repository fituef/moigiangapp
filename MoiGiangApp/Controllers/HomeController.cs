using MoiGiangApp.Helpers;
using MoiGiangApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
namespace MoiGiangApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // Trang chủ: Upload
        [Authorize(Roles = "Admin,ThuKy,TruongKhoa,PhoKhoa,TruongNganh")]
        public ActionResult Index()
        {
            ViewBag.NamHocs = db.NamHocs.ToList();
            ViewBag.HocKys = db.HocKys.ToList();
            ViewBag.Nganhs = db.Nganhs.ToList();
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UploadExcel(HttpPostedFileBase file)
        //{
        //	if (file == null || file.ContentLength == 0)
        //	{
        //		ViewBag.Error = "Vui lòng chọn file Excel!";
        //		return View("Index");
        //	}

        //	if (!file.FileName.EndsWith(".xlsx"))
        //	{
        //		ViewBag.Error = "Chỉ hỗ trợ file .xlsx!";
        //		return View("Index");
        //	}

        //	try
        //	{
        //		var mergedData = ExcelProcessor.ProcessAndMergeExcel(file);

        //		if (!mergedData.Any())
        //		{
        //			ViewBag.Warning = "File Excel không có dữ liệu hợp lệ để import!";
        //			return View("Index");
        //		}

        //		db.LopHocPhans.AddRange(mergedData);
        //		db.SaveChanges();

        //		ViewBag.Success = $"Đã import và gộp thành công {mergedData.Count} lớp học phần!";
        //		return View("Index");
        //	}
        //	catch (Exception ex)
        //	{
        //		ViewBag.Error = ex.Message; // Hiển thị lỗi chi tiết
        //		return View("Index");
        //	}
        //}
        [Authorize(Roles = "Admin,ThuKy,TruongKhoa,PhoKhoa,TruongNganh")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcel(HttpPostedFileBase file, int NamHocId, int HocKyId, int NganhId)
        {
            if (file == null || file.ContentLength == 0)
            {
                Session["Error"] = "Vui lòng chọn file Excel!";
                return RedirectToAction("Index");
            }

            if (!file.FileName.EndsWith(".xlsx"))
            {
                Session["Error"] = "Chỉ hỗ trợ file .xlsx!";
                return RedirectToAction("Index");
            }

            try
            {
                var mergedData = ExcelProcessor.ProcessAndMergeExcel(file);

                if (!mergedData.Any())
                {
                    Session["Error"] = "File Excel không có dữ liệu hợp lệ để import!";
                    return RedirectToAction("Index");
                }
                // GÁN 3 TRƯỜNG CHO TẤT CẢ LỚP
                foreach (var lop in mergedData)
                {
                    lop.NamHocId = NamHocId;
                    lop.HocKyId = HocKyId;
                    lop.NganhId = NganhId;
                }
                db.LopHocPhans.AddRange(mergedData);
                db.SaveChanges();

                Session["Success"] = $"Đã import và gộp thành công {mergedData.Count} lớp học phần!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.Message; // Hiển thị lỗi chi tiết
                return RedirectToAction("Index");
            }
        }
        // Danh sách lớp + gán GV
        //public ActionResult DanhSach()
        //{
        //	var list = db.LopHocPhans.Include(l => l.GiangVien).ToList();
        //	ViewBag.GiangViens = db.GiangViens.ToList();
        //	return View(list);
        //}
        //public ActionResult DanhSach()
        //{
        //	var list = db.LopHocPhans.Include(l => l.GiangVien).ToList();

        //	// LẤY DANH SÁCH TẤT CẢ GV
        //	var allGV = db.GiangViens.ToList();

        //	// TRUYỀN QUA VIEWBAG
        //	ViewBag.AllGiangViens = allGV;

        //	return View(list);
        //}
        //[HttpGet]
        //public ActionResult DanhSach(int? namHocId, int? hocKyId, int? nganhId, int? giangVienId)
        //{
        //	// LẤY DANH SÁCH ĐỂ LỌC
        //	var query = db.LopHocPhans
        //		.Include(l => l.GiangVien)
        //		.Include(l => l.NamHoc)
        //		.Include(l => l.HocKy)
        //		.Include(l => l.Nganh)
        //		.AsQueryable();

        //	// ÁP DỤNG LỌC
        //	if (namHocId.HasValue)
        //		query = query.Where(l => l.NamHocId == namHocId.Value);
        //	if (hocKyId.HasValue)
        //		query = query.Where(l => l.HocKyId == hocKyId.Value);
        //	if (nganhId.HasValue)
        //		query = query.Where(l => l.NganhId == nganhId.Value);
        //	if (giangVienId.HasValue)
        //		query = query.Where(l => l.GiangVienId == giangVienId.Value);

        //	var list = query.ToList();

        //	// TRUYỀN DỮ LIỆU CHO DROPDOWN
        //	ViewBag.NamHocs = db.NamHocs.ToList();
        //	ViewBag.HocKys = db.HocKys.ToList();
        //	ViewBag.Nganhs = db.Nganhs.ToList();
        //	ViewBag.GiangViens = db.GiangViens.ToList();

        //	// GIỮ GIÁ TRỊ ĐÃ CHỌN
        //	ViewBag.SelectedNamHoc = namHocId;
        //	ViewBag.SelectedHocKy = hocKyId;
        //	ViewBag.SelectedNganh = nganhId;
        //	ViewBag.SelectedGiangVien = giangVienId;

        //	return View(list);
        //}
        [Authorize(Roles = "Admin,ThuKy,TruongKhoa,PhoKhoa,TruongNganh")]
        [HttpGet]
        public ActionResult DanhSach(int? namHocId, int? hocKyId, int? nganhId, string giangVienId = null, string monHoc = null, int page = 1)
        {
            const int PageSize = 12; // Tối đa 12 dòng/trang

            var user = User.Identity.Name;
            var roles = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims
                            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                            .Select(c => c.Value)
                            .ToList();

            var query = db.LopHocPhans
                .Include(l => l.GiangVien)
                .Include(l => l.NamHoc)
                .Include(l => l.HocKy)
                .Include(l => l.Nganh)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(monHoc))
            {
                monHoc = monHoc.Trim().ToUpper();
                query = query.Where(l =>
                    l.MaHP.ToUpper().Contains(monHoc) ||
                    l.TenHocPhan.ToUpper().Contains(monHoc));
            }
            if (namHocId.HasValue) query = query.Where(l => l.NamHocId == namHocId.Value);
            if (hocKyId.HasValue) query = query.Where(l => l.HocKyId == hocKyId.Value);
            if (nganhId.HasValue) query = query.Where(l => l.NganhId == nganhId.Value);
            if (giangVienId != null && giangVienId != "") 
                query = query.Where(l => l.GiangVienId == giangVienId);
            var list = new List<LopHocPhan>();
            try
            {
                list = query
            .OrderBy(l => l.MaHP)
            .ThenBy(l => l.LopHP)
            .ThenBy(l => l.Thu)
            .ThenBy(l => l.GioHoc)
            .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            // DANH SÁCH MÀU (10 MÀU NGẪU NHIÊN)
            var mauSac = new[] { "dary-subtle",
        "bg-light", "bg-primary", "bg-success", "bg-danger"
    };

            var random = new Random();
            string mautruoc = "";
            // TÔ MÀU: CHỈ KHI 2 DÒNG LIÊN TIẾP TRÙNG → TÔ CÙNG MÀU, SAU ĐÓ RESET
            for (int i = 0; i < list.Count; i++)
            {
                var lop = list[i];

                // MẶC ĐỊNH: KHÔNG TÔ MÀU
                lop.MauNhom = "";

                // CHỈ XỬ LÝ KHI CÓ DÒNG TIẾP THEO
                if (i < list.Count - 1)
                {
                    var lopTiepTheo = list[i + 1];

                    string maHP1 = (lop.MaHP ?? "").Trim();
                    string lopHP1 = (lop.LopHP ?? "").Trim();
                    string maHP2 = (lopTiepTheo.MaHP ?? "").Trim();
                    string lopHP2 = (lopTiepTheo.LopHP ?? "").Trim();

                    // NẾU 2 DÒNG LIÊN TIẾP TRÙNG → TÔ CÙNG MÀU
                    if (maHP1 == maHP2 && lopHP1 == lopHP2)
                    {

                        string mau = mauSac[random.Next(mauSac.Length)];
                        while (mautruoc != "" && mautruoc == mau)
                        {
                            mau = mauSac[random.Next(mauSac.Length)];
                        }
                        lop.MauNhom = mau;
                        lopTiepTheo.MauNhom = mau; // CÙNG MÀU VỚI DÒNG TRƯỚC
                        i++; // BỎ QUA DÒNG TIẾP THEO (đã xử lý)
                        mautruoc = mau;
                    }
                }
            }

            // === TẠO DANH SÁCH NHÓM ===
            var nhomList = list
                .GroupBy(l => new { l.MaHP, l.LopHP })
                .Select(g => new
                {
                    Nhom = g.Key,
                    DanhSach = g.ToList(),
                    SoDong = g.Count()
                })
                .ToList();

            // === PHÂN TRANG CHÍNH XÁC ===
            var trangHienTai = new List<LopHocPhan>();
            int dongConLai = PageSize;
            int nhomIndex = 0;

            // Bỏ qua các trang trước
            for (int p = 1; p < page && nhomIndex < nhomList.Count; p++)
            {
                dongConLai = PageSize;
                while (nhomIndex < nhomList.Count && dongConLai >= nhomList[nhomIndex].SoDong)
                {
                    dongConLai -= nhomList[nhomIndex].SoDong;
                    nhomIndex++;
                }
            }

            // Lấy dữ liệu trang hiện tại
            dongConLai = PageSize;
            while (nhomIndex < nhomList.Count && dongConLai >= nhomList[nhomIndex].SoDong)
            {
                var nhom = nhomList[nhomIndex];
                trangHienTai.AddRange(nhom.DanhSach);
                dongConLai -= nhom.SoDong;
                nhomIndex++;
            }

            // === TÍNH TỔNG TRANG ===
            int tongTrang = 0;
            int temp = PageSize;
            foreach (var nhom in nhomList)
            {
                if (nhom.SoDong > PageSize)
                {
                    tongTrang += (int)Math.Ceiling(nhom.SoDong / (double)PageSize);
                    temp = PageSize;
                }
                else
                {
                    if (temp < nhom.SoDong)
                    {
                        tongTrang++;
                        temp = PageSize;
                    }
                    temp -= nhom.SoDong;
                }
            }
            if (temp < PageSize) tongTrang++;

            // Truyền dữ liệu
            ViewBag.NamHocs = db.NamHocs.ToList();
            ViewBag.HocKys = db.HocKys.ToList();
            ViewBag.Nganhs = db.Nganhs.ToList();
            ViewBag.GiangViens = db.Users.Where(s => s.IsGiangVien == true).ToList();

            ViewBag.SelectedNamHoc = namHocId;
            ViewBag.SelectedHocKy = hocKyId;
            ViewBag.SelectedNganh = nganhId;
            ViewBag.SelectedGiangVien = giangVienId;
            ViewBag.MonHoc = monHoc; // Lưu giá trị tìm kiếm

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = Math.Max(1, tongTrang);

            return View(trangHienTai);
        }
        //[HttpPost]
        //public ActionResult GanGiangVien(int lopId, int giangVienId)
        //{
        //	var lop = db.LopHocPhans.Find(lopId);
        //	if (lop == null) return HttpNotFound();

        //	var gv = db.GiangViens.Find(giangVienId);
        //	if (gv == null)
        //	{
        //		TempData["Error"] = "Không tìm thấy giảng viên!";
        //		return RedirectToAction("DanhSach");
        //	}

        //	lop.GiangVienId = giangVienId;
        //	db.SaveChanges();

        //	TempData["Success"] = $"Đã gán lớp <b>{lop.MaHP} - {lop.LopHP}</b> cho <b>{gv.TenGV}</b>";
        //	return RedirectToAction("DanhSach");
        //}

        // HÀM HỖ TRỢ - DÙNG Item1, Item2
        private (int Item1, int Item2) ParseGioHoc(string gioHoc)
        {
            var parts = gioHoc.Split('-').Select(t => t.Trim()).ToArray();
            int start = TimeToMinutes(parts[0]);
            int end = TimeToMinutes(parts[1]);
            return (Item1: start, Item2: end);
        }

        private int TimeToMinutes(string time)
        {
            var p = time.Split(':');
            int h = int.Parse(p[0]);
            int m = p.Length > 1 ? int.Parse(p[1]) : 0;
            return h * 60 + m;
        }
        [Authorize(Roles = "Admin,ThuKy,TruongKhoa,PhoKhoa,TruongNganh")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GanGiangVien(int lopId, string giangVienId,
    int? namHocId, int? hocKyId, int? nganhId, int? giangVienFilterId, string monHoc, int page = 1)
        {
            try
            {
                // 1. LẤY DỮ LIỆU
                var lop = db.LopHocPhans.Find(lopId);
                if (lop == null) throw new Exception("Không tìm thấy lớp học phần.");

                var gv = db.Users.Find(giangVienId);
                if (gv == null) throw new Exception("Không tìm thấy giảng viên.");

                // 2. CHUẨN BỊ DỮ LIỆU ĐỂ KIỂM TRA
                var thuHienTai = lop.Thu.Split('/').Select(t => t.Trim()).ToHashSet();
                var range = ParseGioHoc(lop.GioHoc);
                int gioBatDauHienTai = range.Item1;  // Item1 = phút bắt đầu
                int gioKetThucHienTai = range.Item2; // Item2 = phút kết thúc

                // 3. LẤY TẤT CẢ LỚP CỦA GIẢNG VIÊN (TRỪ LỚP HIỆN TẠI) → .ToList() ĐỂ TRÁNH LỖI EF
                var cacLopKhac = db.LopHocPhans
                    .Where(l => l.GiangVienId == giangVienId && l.Id != lop.Id)
                    .Select(l => new { l.Id, l.LopHP, l.Thu, l.GioHoc })
                    .ToList(); // ← Quan trọng: đưa về C#

                // 4. KIỂM TRA XUNG ĐỘT TRONG BỘ NHỚ
                bool xungDot = false;
                LopHocPhan lopitem = new LopHocPhan();
                foreach (var l in cacLopKhac)
                {
                    var thuKhac = l.Thu.Split('/').Select(t => t.Trim());
                    if (!thuHienTai.Overlaps(thuKhac)) continue;

                    var (bdKhac, ktKhac) = ParseGioHoc(l.GioHoc);

                    if (gioBatDauHienTai < ktKhac && bdKhac < gioKetThucHienTai && lop.LopHP[0] == l.LopHP[0])
                    {

                        xungDot = true;
                        break;
                    }
                }

                // 5. XỬ LÝ KẾT QUẢ
                if (xungDot)
                {
                    Session["Error"] = $"Giảng viên {gv.HoTen} đã có lịch trùng! " +
                                      $"Không thể sắp lớp: {lop.MaHP} - {lop.LopHP} - {lop.TenHocPhan}";
                }
                else
                {
                    lop.GiangVienId = giangVienId;
                    db.SaveChanges();
                    Session["Success"] = $"Đã gán {gv.HoTen} cho lớp {lop.MaHP} - {lop.LopHP} - {lop.TenHocPhan}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
            }

            // 6. QUAY LẠI ĐÚNG TRANG + BỘ LỌC
            return RedirectToAction("DanhSach", new
            {
                namHocId,
                hocKyId,
                nganhId,
                giangVienId = giangVienFilterId,
                monHoc,
                page
            });
        }



        // XÓA static
        public List<ApplicationUser> GetGiangVienTrong(LopHocPhan lop, string excludeGVId = null)
        {

            // LẤY TẤT CẢ LỚP CỦA TẤT CẢ GV → .ToList() để ra RAM
            var allLop = db.LopHocPhans
                .Where(l => l.GiangVienId != null)
                .ToList();

            // LỌC GV TRỐNG TRONG RAM (C#)
            List<ApplicationUser> lst = new List<ApplicationUser>();
            lst = db.Users
               .Where(gv => excludeGVId == null || gv.Id != excludeGVId)
               .ToList() // Ra RAM
               .Where(gv => !allLop.Any(l =>
                   l.GiangVienId == gv.Id &&
                   l.Id != lop.Id &&
                   (l.Thu.Contains(lop.Thu[0]) || l.Thu.Contains(lop.Thu[lop.Thu.Length - 1])) && lop.Tiet == l.Tiet && lop.LopHP[0] == l.LopHP[0]))
               .ToList();
            return lst;
        }


        //// Quản lý GV
        //public ActionResult QuanLyGV()
        //{
        //	return View(db.Users.Where(s=>s.IsGiangVien == true).ToList());
        //}

        //[HttpPost]
        //public ActionResult ThemGV(string maGV, string tenGV, string email = "", string sdt = "")
        //{
        //	if (string.IsNullOrWhiteSpace(maGV) || string.IsNullOrWhiteSpace(tenGV))
        //	{
        //		TempData["Error"] = "Vui lòng nhập Mã GV và Tên GV!";
        //		return RedirectToAction("QuanLyGV");
        //	}

        //	if (db.Users.Any(g => g.Id == maGV.Trim()))
        //	{
        //		TempData["Error"] = "Mã GV đã tồn tại!";
        //		return RedirectToAction("QuanLyGV");
        //	}

        //	var gv = new ApplicationUser
        //	{
        //		Id = maGV.Trim(),
        //		HoTen = tenGV.Trim(),
        //		Email = email?.Trim(),
        //		PhoneNumber = sdt?.Trim()
        //	};

        //	db.GiangViens.Add(gv);
        //	db.SaveChanges();
        //	TempData["Success"] = $"Đã thêm GV: {gv.TenGV} ({gv.MaGV})";
        //	return RedirectToAction("QuanLyGV");
        //}

        protected override void Dispose(bool disposing)
        {
            db?.Dispose();
            base.Dispose(disposing);
        }
    }
}