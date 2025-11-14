using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoiGiangApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
		public string HoTen { get; set; }
		public string Khoa { get; set; } // "CNTT", "TMĐT", ...
		public string Nganh { get; set; } // Chỉ dùng cho Trưởng Ngành
		public string LecturerID { get; set; } // Chỉ dùng cho Trưởng Ngành
		public bool IsGiangVien { get; set; }
		public bool IsLock { get; set; }
		public string Img { get; set; }
        public int? HocViId { get; set; }
        public virtual HocVi HocVi { get; set; }
        public int? LoaiGVID { get; set; }
        public virtual LoaiGV LoaiGV { get; set; }
        public string DiaChiTT { get; set; }
		public string DaiChi { get; set; }
		public string STK { get; set; }
		public string TenTK { get; set; }
		public string TenNH { get; set; }
		public string ChiNhanh { get; set; }
		public virtual ICollection<UserNganh> UserNganhs { get; set; }
		public virtual ICollection<LopHocPhan> LopHocPhans { get; set; }
        public virtual ICollection<GiangVienDayNganh> GiangVienDayNganhs { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
		}
		public DbSet<LopHocPhan> LopHocPhans { get; set; }
		public DbSet<NamHoc> NamHocs { get; set; }
		public DbSet<HocKy> HocKys { get; set; }
		public DbSet<Nganh> Nganhs { get; set; }
		public DbSet<UserNganh> UserNganhs { get; set; }
		public DbSet<GiangVienDayNganh> GiangVienDayNganhs { get; set; }
		public DbSet<HocVi> HocVis { get; set; }
		public DbSet<LoaiGV> LoaiGVs { get; set; }

		public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Ngăn tạo bảng AspNet... nếu không dùng Identity
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			// Index duy nhất cho MaGV
			// CẤU HÌNH FK
			modelBuilder.Entity<LopHocPhan>()
				.HasOptional(l => l.GiangVien)
				.WithMany(u => u.LopHocPhans)
				.HasForeignKey(l => l.GiangVienId);

			base.OnModelCreating(modelBuilder);
		}

		
	}
}