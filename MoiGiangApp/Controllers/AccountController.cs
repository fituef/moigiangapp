using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MoiGiangApp.App_Start;
using MoiGiangApp.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace MoiGiangApp.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        #region -- Upload
        private string pathFile = "/files/profiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
        private string fileName = "";
        public string UploadImage(HttpPostedFileBase upload)
        {
            fileName = Path.GetFileName(upload.FileName);
            fileName = Processing.UrlImages(fileName);
            bool exsits = System.IO.Directory.Exists(Server.MapPath(pathFile));
            if (!exsits)
                System.IO.Directory.CreateDirectory(Server.MapPath(pathFile));
            var path = Path.Combine(Server.MapPath(pathFile), fileName);
            upload.SaveAs(path);
            return pathFile + fileName;
        }
        #endregion




        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db;
        public AccountController()
        {
            db = new ApplicationDbContext();
        }
        //public ActionResult Index()
        //{
        //	// Sử dụng DbContext trực tiếp
        //	var users = db.Users
        //		.Include(u => u.Roles) // IdentityUserRole
        //		.Include(u => u.UserNganhs.Select(un => un.Nganh)) // Ngành phụ trách
        //		.ToList();

        //	var model = users.Select(u => new UserRolesNganhViewModel
        //	{
        //		HoTen = u.HoTen,
        //		Id = u.Id,
        //		LockoutEnabled = u.LockoutEnabled,
        //		Email = u.Email,
        //		Roles = u.Roles
        //			.Select(ur => db.Roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name ?? "")
        //			.ToList(),
        //		Nganhs = u.UserNganhs.Select(un => un.Nganh.TenNganh).ToList()
        //	}).ToList();

        //	return View(model);
        //}
        [Authorize(Roles = "Admin,TruongKhoa")]
        public ActionResult Index(string searchString, string roleName, int? nganhID, int? hocViId, int? LoaiId, int? nganhgdID, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            var users = db.Users
                .Include(u => u.Roles)
                .Include(u => u.UserNganhs.Select(un => un.Nganh)).Include(u=> u.HocVi)
                .AsQueryable();

            if (nganhID.HasValue && nganhID > 0)
            {
                users = users.Where(u => u.UserNganhs.Any(un => un.NganhId == nganhID));
            }
            if (nganhgdID.HasValue && nganhgdID > 0)
            {
                users = users.Where(u => u.GiangVienDayNganhs.Any(un => un.NganhId == nganhgdID));
            }
            if (!string.IsNullOrEmpty(roleName))
                users = users.Where(u => u.Roles.Any(ur => db.Roles.Any(r => r.Id == ur.RoleId && r.Name == roleName)));
            // Lọc học vị
            if (hocViId.HasValue)
                users = users.Where(u => u.HocViId == hocViId.Value);
            //Loại GV
            if (LoaiId.HasValue)
                users = users.Where(u => u.LoaiGVID == LoaiId.Value);
            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u =>
                    u.Email.Contains(searchString) || u.LecturerID.Contains(searchString) || u.Nganh.Contains(searchString) ||
                    u.HoTen.Contains(searchString));
            }

            // Chuyển thành ViewModel
            var userList = users.Select(u => new UserRolesNganhViewModel
            {
                Id = u.Id,
                HoTen = u.HoTen,
                Email = u.Email,
                Hinh = u.Img,
                MaGV = u.LecturerID,
                LockoutEnabled = u.IsLock,
                TenHocVi = u.HocVi.TenHocVi,
                TenLoaiGV = u.LoaiGV.TenLoai,
                Roles = u.Roles
                    .Select(ur => db.Roles.FirstOrDefault(r => r.Id == ur.RoleId).Name ?? "")
                    .ToList(),
                Nganhs = u.UserNganhs.Select(un => un.Nganh.TenNganh).ToList(),
                NganhDays = u.GiangVienDayNganhs.Select(un => un.Nganh.TenNganh).ToList()
            });

            // Phân trang
            var pagedUsers = userList.OrderByDescending(s => s.Id).ToPagedList(pageNumber, pageSize);

            var model = new UserIndexViewModel
            {
                Users = pagedUsers,
                SearchString = searchString,
                roleName = roleName,
                NganhID = nganhID,
                HoViID = hocViId,
                LoaiGVID = LoaiId,
                NganhDayID = nganhgdID
            };

            ViewBag.NganhList = new SelectList(db.Nganhs.OrderBy(n => n.TenNganh), "Id", "TenNganh", nganhID);
            ViewBag.RoleList = new SelectList(db.Roles.OrderBy(r => r.Name), "Name", "Name", roleName);
            ViewBag.HocViList = new SelectList(db.HocVis.OrderBy(h => h.Id), "Id", "TenHocVi", hocViId);
            ViewBag.LoaiGVList = new SelectList(db.LoaiGVs.OrderBy(h => h.Id), "Id", "TenLoai", LoaiId);
            ViewBag.NganhDayList = new SelectList(db.Nganhs.OrderBy(h => h.Id), "Id", "TenNganh", nganhgdID);
            return View(model);
        }
        [Authorize(Roles = "Admin,TruongKhoa")]
        public async Task<ActionResult> ToggleLock(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsLock = !user.IsLock;
                await UserManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        // GET: /User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) return HttpNotFound();

            var user = db.Users
                .Include(u => u.Roles)
                .Include(u => u.UserNganhs)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return HttpNotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                HoTen = user.HoTen,
                Email = user.Email,
                MaGV = user.LecturerID,
                Hinh = user.Img,
                IsLock = user.IsLock,
                // Thông tin thêm
                HocViId = user.HocViId,
                DiaChiTT = user.DiaChiTT,
                DaiChi = user.DaiChi,
                STK = user.STK,
                TenTK = user.TenTK,
                LoaiGVId = user.LoaiGVID,
                TenNH = user.TenNH,
                ChiNhanh = user.ChiNhanh,
                SelectedRoleIds = user.Roles.Select(r => r.RoleId).ToList(),
                SelectedNganhIds = user.UserNganhs.Select(un => un.NganhId.ToString()).ToList(),
                SelectedDayNganhIds = db.GiangVienDayNganhs.Where(x => x.UserId == user.Id).Select(x => x.NganhId.ToString()).ToList(),
                RoleList = new SelectList(db.Roles.OrderBy(r => r.Name), "Id", "Name"),
                NganhList = new SelectList(db.Nganhs.OrderBy(n => n.TenNganh), "Id", "TenNganh"),
                DayNganhList = new SelectList(db.Nganhs.OrderBy(n => n.TenNganh), "Id", "TenNganh"),
                HocViList = new SelectList(db.HocVis.OrderBy(h => h.Id),"Id","TenHocVi"),
                LoaiGVList = new SelectList(db.LoaiGVs.OrderBy(h => h.Id),"Id", "TenLoai")
            };

            return View(model);
        }
        // POST: /User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel model, HttpPostedFileBase Hinh)
        {
            if (!ModelState.IsValid)
            {
                model.RoleList = new SelectList(db.Roles.OrderBy(r => r.Name), "Id", "Name");
                model.NganhList = new SelectList(db.Nganhs.OrderBy(n => n.TenNganh), "Id", "TenNganh");
                return View(model);
            }

            var user = db.Users
                .Include(u => u.Roles)
                .Include(u => u.UserNganhs)
                .FirstOrDefault(u => u.Id == model.Id);

            if (user == null) return HttpNotFound();

            var currentDayNganhIds = db.GiangVienDayNganhs.Where(x => x.UserId == user.Id).Select(x => x.NganhId).ToList();

            var selectedDayNganh = model.SelectedDayNganhIds.Select(int.Parse).ToList();
            var _toRemove = currentDayNganhIds.Except(selectedDayNganh).ToList();
            foreach (var id in _toRemove)
            {
                var removeItem = db.GiangVienDayNganhs
                    .First(x => x.UserId == user.Id && x.NganhId == id);
                db.GiangVienDayNganhs.Remove(removeItem);
            }
            var _toAdd = selectedDayNganh.Except(currentDayNganhIds).ToList();
            foreach (var id in _toAdd)
            {
                db.GiangVienDayNganhs.Add(new GiangVienDayNganh
                {
                    UserId = user.Id,
                    NganhId = id
                });
            }

            // CẬP NHẬT THÔNG TIN CƠ BẢN
            user.HoTen = model.HoTen;
            user.Email = model.Email;
            user.LecturerID = model.MaGV;
            user.IsLock = model.IsLock;
            // THÔNG TIN BỔ SUNG
            user.HocViId = model.HocViId;
            user.DiaChiTT = model.DiaChiTT;
            user.DaiChi = model.DaiChi;
            user.STK = model.STK;
            user.TenTK = model.TenTK;
            user.TenNH = model.TenNH;
            user.ChiNhanh = model.ChiNhanh;
            user.LoaiGVID = model.LoaiGVId;

            // XỬ LÝ ẢNH
            try
            {
                if (Hinh != null && Hinh.ContentLength > 0)
                {
                    user.Img = UploadImage(Hinh);
                }
            }
            catch { }

            // CẬP NHẬT ROLE
            var currentRoleIds = user.Roles.Select(r => r.RoleId).ToList();
            var toRemove = currentRoleIds.Except(model.SelectedRoleIds).ToList();
            var toAdd = model.SelectedRoleIds.Except(currentRoleIds).ToList();

            foreach (var roleId in toRemove)
                user.Roles.Remove(user.Roles.First(r => r.RoleId == roleId));

            foreach (var roleId in toAdd)
                user.Roles.Add(new IdentityUserRole { UserId = user.Id, RoleId = roleId });

            // CẬP NHẬT NGÀNH
            var currentNganhIds = user.UserNganhs.Select(un => un.NganhId).ToList();
            var nganhToRemove = currentNganhIds.Except(model.SelectedNganhIds.Select(int.Parse)).ToList();
            var nganhToAdd = model.SelectedNganhIds.Select(int.Parse).Except(currentNganhIds).ToList();

            foreach (var nganhId in nganhToRemove)
                db.UserNganhs.Remove(db.UserNganhs.First(un => un.UserId == user.Id && un.NganhId == nganhId));

            foreach (var nganhId in nganhToAdd)
                db.UserNganhs.Add(new UserNganh { UserId = user.Id, NganhId = nganhId });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,TruongKhoa")]
        [HttpGet]
        public ActionResult CreateUser()
        {
            // Lấy danh sách role để hiển thị checkbox hoặc listbox
            ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name");

            ViewBag.Nganhs = new SelectList(db.Nganhs.ToList(), "Id", "TenNganh");
            return View();
        }
        [Authorize(Roles = "Admin,TruongKhoa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name");
                ViewBag.Nganhs = new SelectList(db.Nganhs.ToList(), "Id", "TenNganh");
                return View(model);
            }
            if (model.SelectedRoles.Count == 0)
            {
                ModelState.AddModelError("", "Bạn chưa chọn quyền!");
                ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name");
                ViewBag.Nganhs = new SelectList(db.Nganhs.ToList(), "Id", "TenNganh");
                return View(model);
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, LecturerID = model.LecturerID, HoTen = model.HoTen, Nganh= model.Nganhgd };
            var result = UserManager.Create(user, model.Password);

            db.SaveChanges();

            if (result.Succeeded)
            {
                foreach (var roleId in model.SelectedRoles)
                {
                    var roleName = db.Roles.Find(roleId).Name;
                    UserManager.AddToRole(user.Id, roleName);

                    // Nếu role là Trưởng ngành và có NganhId
                    if (roleName.StartsWith("TruongNganh") && model.NganhId.HasValue)
                    {
                        db.UserNganhs.Add(new UserNganh { UserId = user.Id, NganhId = model.NganhId.Value });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err);
            }

            ViewBag.Roles = new MultiSelectList(db.Roles.ToList(), "Id", "Name");
            ViewBag.Nganhs = new SelectList(db.Nganhs.ToList(), "Id", "TenNganh");
            return View(model);
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            var _user = UserManager.Users.Where(s => s.Email == model.Email && s.IsLock == false).FirstOrDefault();
            if (_user == null)
                return Redirect("/cannot-access");
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    {
                        var user = await UserManager.FindByEmailAsync(model.Email);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Tài khoản không tồn tại.");
                        }
                        else if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                        {
                            ModelState.AddModelError("", "Vui lòng xác nhận email trước khi đăng nhập.");
                        }
                        else
                        {
                            // Tăng số lần đăng nhập sai → khóa tài khoản sau 5 lần
                            await UserManager.AccessFailedAsync(user.Id);
                            var failedCount = await UserManager.GetAccessFailedCountAsync(user.Id);
                            if (failedCount >= 5)
                            {
                                await _userManager.SetLockoutEndDateAsync(user.Id, DateTimeOffset.UtcNow.AddMinutes(15));
                                ModelState.AddModelError("", "Tài khoản bị khóa 15 phút do nhập sai quá nhiều lần.");
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Sai mật khẩu. Còn {5 - failedCount} lần thử.");
                            }
                        }
                        return View(model);
                    }
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}