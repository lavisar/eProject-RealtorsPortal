using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;

namespace Eproject_RealtorsPortal.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        Eproject_RealtorsPortal.Data.LQHVContext dbContext = new LQHVContext();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginAdmin(string AdminEmail, string AdminPassword)
        {
            Eproject_RealtorsPortal.Models.Admin mail = dbContext.Admins.Where(a => a.AdminEmail.Equals(AdminEmail)).FirstOrDefault();
            if (mail != null)
            {
                if (AdminPassword == mail.AdminPassword)
                {
                    HttpContext.Session.SetString("Admin", mail.AdminEmail + "_" + AdminPassword);
                    HttpContext.Session.SetString("adminId", mail.AdminId.ToString());
                    HttpContext.Session.SetString("adminName", mail.AdminName);
                    if (mail.AdminImage != null)
                    {
                        HttpContext.Session.SetString("adminImage", mail.AdminImage);
                    }
                    if (mail.AdminRole != null)
                    {
                        HttpContext.Session.SetString("adminRole", mail.AdminRole);
                    }
                    HttpContext.Session.SetString("adminEmail", mail.AdminEmail);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.msg = "Invalid Password!";
                return View();
            }
            ViewBag.msg = "Invalid Email!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("adminName");
            HttpContext.Session.Remove("adminEmail");
            HttpContext.Session.Remove("adminRole");
            HttpContext.Session.Remove("adminId");
            HttpContext.Session.Remove("adminImage");

            return RedirectToAction("LoginAdmin", "Home");
        }

        public IActionResult ViewUser(string searchkey)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            List<User> users;
            if (!string.IsNullOrEmpty(searchkey))
            {
                users = dbContext.Users.Where(u => u.UsersFullname.Contains(searchkey) || u.UsersEmail.Contains(searchkey)).ToList();
                return View(users);
            }
            users = dbContext.Users.ToList();
            return View(users);
        }

        public IActionResult AddUser()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User model)
        {
            var email = dbContext.Users.Where(e => e.UsersEmail == model.UsersEmail).FirstOrDefault();
            if (email != null)
            {
                ViewBag.msg = "This email has been register!";
                return View();
            }
            if (model.UsersPassword.Length < 6 || model.UsersPassword.Length > 20)
            {
                ViewBag.msg = "Password must be between 6-20 characters!";
                return View(model);
            }

            var user = new User()
            {
                UsersFullname = model.UsersFullname,
                UsersEmail = model.UsersEmail,
                UsersPhone = model.UsersPhone,
                UsersPassword = model.UsersPassword,
                UsersGender = model.UsersGender,
                UsersAddress = model.UsersAddress,
                UsersImage = "defaultImage.jpg",
                UsersStatus = false,
                PackagesId = 1

            };
            Random random = new Random();
            user.ConfirmEmail = random.Next().ToString();

            HttpContext.Session.SetString("AdminConfirmEmail", user.ConfirmEmail);

            dbContext.Users.Add(user);

            if (dbContext.SaveChanges() < 1)
            {
                HttpContext.Session.Remove("AdminConfirmEmail");
                ViewBag.msg = "Registration failed!";
                return View(model);
            }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("huyenle021039@gmail.com");
            mail.To.Add(model.UsersEmail);
            mail.Subject = "Confirm your account";
            mail.IsBodyHtml = true;
            mail.Body = "<br> " +
                     "Dear Mr/Ms " + user.UsersFullname + ", <br>" +
                     "<h4 style=\"color: orange;\">Welcome to LQHV Realtors Portal</h4>" +
                     "<p> Your authentication code is: <b style=\"color: blue;\">" + user.ConfirmEmail + "</b></p>" +
                     "<p> Please enter the confirmation code to login your account</p><br><br>" +
                     "<i>LQHV,</i>";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("huyenle021039@gmail.com", "fwnwnhallceirfsl");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return View("AdminAuthenticationForm", model);

        }

        public IActionResult AdminAuthenticationForm()
        {
            if (HttpContext.Session.GetString("AdminConfirmEmail") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminAuthenticationForm(User model)
        {

            string ConfirmEmail = HttpContext.Session.GetString("AdminConfirmEmail");
            if (ConfirmEmail == null)
            {
                return View("AddUser");
            }


            if (ConfirmEmail == model.ConfirmEmail)
            {
                User user = dbContext.Users.Where(o => o.ConfirmEmail == ConfirmEmail).FirstOrDefault();
                if (user != null)
                {
                    user.UsersStatus = true;
                    dbContext.Users.Update(user);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Registration successful!";
                        return View("AddUser", model);
                    }
                }
                else
                {
                    ViewBag.msg = "The verification code is not correct!";
                    return View("AdminAuthenticationForm", model);
                }
            }
            ViewBag.msg = "Please enter the verification code to continue logging into your account";
            return View("AdminAuthenticationForm", model);
        }

        public IActionResult DeleteUser(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            User user = dbContext.Users.Where(s => s.UsersId == id).FirstOrDefault();
            if (user != null)
            {
                dbContext.Users.Remove(user);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewUser", "Home");
                }
            }
            return View(user);
        }

        public IActionResult DetailsUser(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            User user = dbContext.Users.Where(s => s.UsersId == id).FirstOrDefault();
            return View(user);
        }


        public IActionResult EditUser(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            User user = dbContext.Users.Where(s => s.UsersId == id).FirstOrDefault();
            return View(user);
        }


        [HttpPost]
        public IActionResult EditUser(User model)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            User user = dbContext.Users.Where(s => s.UsersId == model.UsersId).FirstOrDefault();

            if (user != null)
            {
                user.UsersStatus = model.UsersStatus;
                dbContext.Users.Update(user);
                if (dbContext.SaveChanges() >= 1)
                {
                    ViewBag.msg = "Update successful.";
                    return RedirectToAction("ViewUser", "Home", model);
                }
                ViewBag.msg = "Status update failed !";
                return View();
            }
            return View(model);
        }


        //+++++  admin  +++++//
        public IActionResult ViewAdmin(string searchkey)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            List<Eproject_RealtorsPortal.Models.Admin> admins;
            if (!string.IsNullOrEmpty(searchkey))
            {
                admins = dbContext.Admins.Where(u => u.AdminName.Contains(searchkey) || u.AdminEmail.Contains(searchkey)).ToList();
                return View(admins);
            }
            admins = dbContext.Admins.ToList();
            return View(admins);
        }

        public IActionResult AddAdmin()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(Eproject_RealtorsPortal.Models.Admin model)
        {
            var email = dbContext.Admins.Where(a => a.AdminEmail == model.AdminEmail).FirstOrDefault();
            if (email != null)
            {
                ViewBag.msg = "Email already exists!";
                return View(model);
            }
            if (model.AdminPassword.Length < 6 || model.AdminPassword.Length > 20)
            {
                ViewBag.msg = "Password must be between 6-20 characters!";
                return View(model);
            }

            var admin = new Eproject_RealtorsPortal.Models.Admin()
            {
                AdminName = model.AdminName,
                AdminEmail = model.AdminEmail,
                AdminPassword = model.AdminPassword,
                AdminImage = "defaultImage.jpg",
                AdminStatus = false,
                AdminRole = "Staff"

            };
            Random random = new Random();
            admin.ConfirmEmail = random.Next().ToString();

            HttpContext.Session.SetString("AdminConfirm", admin.ConfirmEmail);

            dbContext.Admins.Add(admin);

            if (dbContext.SaveChanges() < 1)
            {
                HttpContext.Session.Remove("AdminConfirm");
                ViewBag.msg = "Registration failed!";
                return View(model);
            }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("huyenle021039@gmail.com");
            mail.To.Add(model.AdminEmail);
            mail.Subject = "Confirm your account";
            mail.IsBodyHtml = true;
            mail.Body = "<br> " +
                     "Dear Mr/Ms " + admin.AdminName + ", <br>" +
                     "<h4 style=\"color: orange;\">Welcome to LQHV Realtors Portal</h4>" +
                     "<p> Your authentication code is: <b style=\"color: blue;\">" + admin.ConfirmEmail + "</b></p>" +
                     "<p> Please enter the confirmation code to login your account</p><br><br>" +
                     "<i>LQHV,</i>";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("huyenle021039@gmail.com", "fwnwnhallceirfsl");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return View("AdminAuthenticationEmail", model);

        }


        public IActionResult DeleteAdmin(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(s => s.AdminId == id).FirstOrDefault();
            if (admin != null)
            {
                dbContext.Admins.Remove(admin);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewAdmin", "Home");
                }
            }
            return View(admin);
        }

        public IActionResult DetailsAdmin(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Eproject_RealtorsPortal.Models.Admin admins = dbContext.Admins.Where(s => s.AdminId == id).FirstOrDefault();
            return View(admins);
        }

        public IActionResult EditAdmin(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(s => s.AdminId == id).FirstOrDefault();
            ViewBag.admins = admin;
            return View(admin);
        }


        [HttpPost]
        public async Task<IActionResult> EditAdmin(long AdminId, string AdminName, IFormFile AdminImage, bool AdminStatus, string AdminRole)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            var admin = new Eproject_RealtorsPortal.Models.Admin();
            if (AdminImage != null)
            {
                string[] typeAllow = { ".jpg", ".png", ".jpeg", ".jftf" };
                if (!typeAllow.Contains(Path.GetExtension(AdminImage.FileName).ToLower()))
                {
                    ViewBag.errorImage = "You must select true image type (jpg, png, jpeg, jftf)";
                    return View();
                }

                string filePath = "wwwroot/admin/dist/img";
                string fileName = AdminImage.FileName.Replace(Path.GetExtension(AdminImage.FileName), "");
                fileName += DateTime.Now.ToString("yymmssfff") + ".png";
                var fileNameWithPath = string.Concat(filePath, "/", fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await AdminImage.CopyToAsync(stream);
                }
                ViewBag.admins = admin.ChangeInfor(AdminId, AdminName, fileName, AdminRole);

                return View();
            }
            ViewBag.admins = admin.ChangeInfor(AdminId, AdminName, null, AdminRole);
            return View();
        }

        // Profile Admin
        public IActionResult AccountProfile()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            else
            {
                var AdminId = long.Parse(HttpContext.Session.GetString("adminId"));
                Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(s => s.AdminId == AdminId).FirstOrDefault();
                ViewBag.admins = admin;
                return View(admin);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AccountProfile(long AdminId, string AdminName, IFormFile AdminImage, string AdminRole)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            var admin = new Eproject_RealtorsPortal.Models.Admin();
            if (AdminImage != null)
            {
                string[] typeAllow = { ".jpg", ".png", ".jpeg", ".jftf" };
                if (!typeAllow.Contains(Path.GetExtension(AdminImage.FileName).ToLower()))
                {
                    ViewBag.errorImage = "You must select true image type (jpg, png, jpeg, jftf)";
                    return View();
                }

                string filePath = "wwwroot/admin/dist/img";
                string fileName = AdminImage.FileName.Replace(Path.GetExtension(AdminImage.FileName), "");
                fileName += DateTime.Now.ToString("yymmssfff") + ".png";
                var fileNameWithPath = string.Concat(filePath, "/", fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await AdminImage.CopyToAsync(stream);
                }
                ViewBag.admins = admin.ChangeInfor(AdminId, AdminName, fileName, AdminRole);
                HttpContext.Session.SetString("adminName", AdminName);
                HttpContext.Session.SetString("adminImage", fileName);
                ViewBag.sg = "Update successful.";
                return View();
            }
            ViewBag.admins = admin.ChangeInfor(AdminId, AdminName, null, AdminRole);
            HttpContext.Session.SetString("adminName", AdminName);
            ViewBag.sg = "Update successful.";
            return View();
        }


        public IActionResult AdminAuthenticationEmail()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            if (HttpContext.Session.GetString("AdminConfirmEmail") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminAuthenticationEmail(User model)
        {

            string ConfirmEmail = HttpContext.Session.GetString("AdminConfirm");
            if (ConfirmEmail == null)
            {
                return View("AddAdmin");
            }

            if (ConfirmEmail == model.ConfirmEmail)
            {
                Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(o => o.ConfirmEmail == ConfirmEmail).FirstOrDefault();
                if (admin != null)
                {
                    admin.AdminStatus = true;
                    dbContext.Admins.Update(admin);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Registration successful!";
                        return View("AddAdmin", model);
                    }
                }
                else
                {
                    ViewBag.msg = "The verification code is not correct!";
                    return View("AdminAuthenticationEmail", model);
                }
            }
            ViewBag.msg = "Please enter the verification code to continue logging into your account";
            return View("AdminAuthenticationEmail", model);
        }

        public IActionResult ChangePassword()
        {

            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            else
            {
                var AdminId = long.Parse(HttpContext.Session.GetString("adminId"));
                Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(s => s.AdminId == AdminId).FirstOrDefault();
                ViewBag.admins = admin;
                return View(admin);
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(long AdminId, string oldPassword, string newPassword, string ConfirmNewPassword)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Eproject_RealtorsPortal.Models.Admin admin = dbContext.Admins.Where(s => s.AdminId == AdminId).FirstOrDefault();

            ViewBag.admins = admin;
            if (admin.AdminPassword != oldPassword)
            {
                ViewBag.result = "The current password is not correct! ";
                return View();
            }
            if (newPassword.Length > 20 || newPassword.Length < 6 || ConfirmNewPassword.Length > 20 || ConfirmNewPassword.Length < 6)
            {
                ViewBag.result = "Password must be between 6-20 characters!";
                return View();
            }
            if (oldPassword == newPassword)
            {
                ViewBag.result = "Password must be different from your recent password !";
                return View();
            }
            if (newPassword != ConfirmNewPassword)
            {
                ViewBag.result = " Confirm new password does not match.";
                return View();
            }
            ViewBag.result = admin.ChangePassword(AdminId, oldPassword, newPassword, ConfirmNewPassword);
            if (ViewBag.result == false)
            {
                ViewBag.result = "Password change failed, please try again.";
                return View();
            }
            else
            {
                HttpContext.Session.Remove("Admin");
                HttpContext.Session.Remove("adminName");
                HttpContext.Session.Remove("adminEmail");
                HttpContext.Session.Remove("adminRole");
                HttpContext.Session.Remove("adminId");
                HttpContext.Session.Remove("adminImage");

                return RedirectToAction("LoginAdmin", "Home");
            }
        }
    }
}