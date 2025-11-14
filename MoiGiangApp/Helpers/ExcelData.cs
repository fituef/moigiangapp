using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoiGiangApp.Models;

namespace MoiGiangApp.Helpers
{
	public class ExcelData
	{
		public string MaHP { get; set; }
		public string TenHocPhan { get; set; }
		public string LopHP { get; set; }
		public string MaLop { get; set; }
		public int SiSo { get; set; }
		public string Thu { get; set; }
		public int Tiet { get; set; }
		public string GioHoc { get; set; }
		public int Tang { get; set; }
		public string Phong { get; set; }
		public string MaGV { get; set; }
		public string Loai { get; set; }
		public string NgayBatDau { get; set; }
		public string NgayKetThuc { get; set; }
		public int Tuan { get; set; }
		public string DK { get; set; }
		public string SL { get; set; }
	}

	public static class ExcelProcessor
	{
		public static List<LopHocPhan> ProcessAndMergeExcel(HttpPostedFileBase file)
		{
			var rawData = new List<ExcelData>();

			try
			{
				using (var package = new ExcelPackage(file.InputStream))
				{
					// KIỂM TRA CÓ SHEET NÀ ĐỌNG NÀO KHÔNG
					if (package.Workbook.Worksheets.Count == 0)
					{
						throw new Exception("File Excel không có sheet nào!");
					}

					// LẤY SHEET ĐẦU TIÊN (AN TOÀN)
					var ws = package.Workbook.Worksheets
				.FirstOrDefault(w => w.Hidden == eWorkSheetHidden.Visible && w.Dimension != null);

					// KIỂM TRA CÓ DỮ LIỆU KHÔNG
					if (ws.Dimension == null || ws.Dimension.Rows < 2)
					{
						throw new Exception("File Excel không có dữ liệu hoặc không có dòng header!");
					}

					int rows = ws.Dimension.Rows;

					for (int r = 2; r <= rows; r++)
					{
						try
						{
							var row = new ExcelData
							{
								MaHP = ws.Cells[r, 1].Value?.ToString()?.Trim(),
								TenHocPhan = ws.Cells[r, 2].Value?.ToString()?.Trim(),
								LopHP = ws.Cells[r, 3].Value?.ToString()?.Trim(),
								MaLop = ws.Cells[r, 4].Value?.ToString()?.Trim(),
								SiSo = GetInt(ws.Cells[r, 5]),
								Thu = ws.Cells[r, 6].Value?.ToString()?.Trim(),
								Tiet = GetInt(ws.Cells[r, 7]),
								GioHoc = ws.Cells[r, 8].Value?.ToString()?.Trim(),
								Tang = GetInt(ws.Cells[r, 9]),
								Phong = ws.Cells[r, 10].Value?.ToString()?.Trim(),
								MaGV = ws.Cells[r, 11].Value?.ToString()?.Trim(),
								Loai = ws.Cells[r, 12].Value?.ToString()?.Trim(),
								NgayBatDau = ws.Cells[r, 13].Value?.ToString()?.Trim(),
								NgayKetThuc = ws.Cells[r, 14].Value?.ToString()?.Trim(),
								Tuan = GetInt(ws.Cells[r, 15]),
								DK = ws.Cells[r, 16].Value?.ToString()?.Trim(),
								SL = ws.Cells[r, 17].Value?.ToString()?.Trim()
							};

							if (!string.IsNullOrEmpty(row.MaHP))
								rawData.Add(row);
						}
						catch { /* Bỏ qua dòng lỗi */ }
					}
				}
			}
			catch (Exception ex)
			{
				// NÉM LẠI LỖI ĐỂ CONTROLLER BẮT
				throw new Exception("Lỗi đọc file Excel: " + ex.Message);
			}

			return MergeData(rawData);
		}

		private static int GetInt(ExcelRange cell)
		{
			if (cell?.Value == null) return 0;

			int val = 0;  // Khai báo rõ ràng để tránh lỗi unassigned
			if (int.TryParse(cell.Value.ToString(), out val))
			{
				return val;
			}
			return 0;
		}

		// HÀM GỘP CHÍNH – TRẢ VỀ List<LopHocPhan>
		private static List<LopHocPhan> MergeData(List<ExcelData> data)
		{
			if (!data.Any()) return new List<LopHocPhan>();

			//return data
			//	.GroupBy(x => new
			//	{
			//		x.MaHP,
			//		x.TenHocPhan,
			//		x.LopHP,
			//		x.MaLop,
			//		x.SiSo,
			//		x.Tiet,
			//		x.GioHoc,
			//		x.Tang,
			//		x.Phong
			//	})
			//	.Select(g => new LopHocPhan
			//	{
			//		MaHP = g.Key.MaHP,
			//		TenHocPhan = g.Key.TenHocPhan,
			//		LopHP = g.Key.LopHP,
			//		MaLop = g.Key.MaLop,
			//		SiSo = g.Key.SiSo,
			//		Thu = string.Join("/", g.Select(x => x.Thu).Distinct().OrderBy(t => t)),
			//		Tiet = g.Key.Tiet,
			//		GioHoc = g.Key.GioHoc,
			//		Tang = g.Key.Tang,
			//		Phong = g.Key.Phong,
			//		MaGV = string.Join(" / ", g.Select(x => x.MaGV).Where(m => !string.IsNullOrEmpty(m)).Distinct()),
			//		Loai = string.Join(" / ", g.Select(x => x.Loai).Where(l => !string.IsNullOrEmpty(l)).Distinct()),
			//		NgayBatDau = JoinDates(g.Select(x => x.NgayBatDau)),
			//		NgayKetThuc = JoinDates(g.Select(x => x.NgayKetThuc)),
			//		Tuan = g.First().Tuan,
			//		DK = string.Join("-", g.Select(x => x.DK).Distinct().OrderBy(d => int.TryParse(d, out int n) ? n : 0)),
			//		SL = string.Join("-", g.Select(x => x.SL).Distinct().OrderBy(s => int.TryParse(s, out int n) ? n : 0)),
			//		NgayTao = DateTime.Now
			//	})
			//	.ToList();
			// TRONG HÀM MergeData()
			return data
				.GroupBy(x => new
				{
					x.MaHP,
					x.TenHocPhan,
					x.LopHP,
					x.MaLop,
					x.SiSo,
					x.Tiet,
					x.GioHoc,
					x.Tang,
					x.Phong
				})
				.Select(g => new LopHocPhan
				{
					MaHP = g.Key.MaHP,
					TenHocPhan = g.Key.TenHocPhan,
					LopHP = g.Key.LopHP,
					MaLop = g.Key.MaLop,
					SiSo = g.Key.SiSo,
					Thu = string.Join("/", g.Select(x => x.Thu).Distinct().OrderBy(t => t)),
					Tiet = g.Key.Tiet,
					GioHoc = g.Key.GioHoc,
					Tang = g.Key.Tang,
					Phong = g.Key.Phong,
					MaGV = string.Join(" / ", g.Select(x => x.MaGV).Where(m => !string.IsNullOrEmpty(m)).Distinct()),

					// GỘP LOẠI PHÒNG: LOẠI BỎ TRÙNG, SẮP XẾP, NỐI BẰNG " / "
					Loai = string.Join(" / ",
						g.Select(x => x.Loai)
						 .Where(l => !string.IsNullOrEmpty(l))
						 .Select(l => l.Trim())
						 .Distinct()
						 .OrderBy(l => l)
					),

					NgayBatDau = JoinDates(g.Select(x => x.NgayBatDau)),
					NgayKetThuc = JoinDates(g.Select(x => x.NgayKetThuc)),
					Tuan = g.First().Tuan,
					DK = string.Join("-", g.Select(x => x.DK).Distinct().OrderBy(d => int.TryParse(d, out int n) ? n : 0)),
					SL = string.Join("-", g.Select(x => x.SL).Distinct().OrderBy(s => int.TryParse(s, out int n) ? n : 0)),
					NgayTao = DateTime.Now
				})
				.ToList();
		}

		private static string JoinDates(IEnumerable<string> dates)
		{
			var valid = dates
				.Where(d => !string.IsNullOrEmpty(d))
				.SelectMany(d => d.Split(new[] { '-', '/' }, StringSplitOptions.RemoveEmptyEntries))
				.Select(d => d.Trim())
				.Where(d => DateTime.TryParseExact(d, "M/d/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
				.Distinct()
				.OrderBy(d => DateTime.ParseExact(d, "M/d/yyyy", null))
				.ToList();

			return string.Join(" - ", valid);
		}
	}
}