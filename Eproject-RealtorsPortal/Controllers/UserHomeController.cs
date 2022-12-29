using Microsoft.AspNetCore.Mvc;
using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Authentication;
using System.Net.Mail;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Eproject_RealtorsPortal.Controllers
{

    public class UserHomeController : Controller
    {
       
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Logout", "UserHome");
            }
            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("Logout", "UserHome");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {

            Random random = new Random();
            LQHVContext dbContext = new LQHVContext();
            var email = dbContext.Users.Where(e => e.UsersEmail == model.UsersEmail).FirstOrDefault();
            if (email != null)
            {
                ViewBag.msg = "This email has been register!";
                return RedirectToAction("Register", model);
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
                PackagesId = 2,//sử dụng id có sẵn làm mặc định

            };
            user.ConfirmEmail = random.Next().ToString();

            HttpContext.Session.SetString("UserEmail", user.UsersEmail);
            HttpContext.Session.SetString("UserConfirmEmail", user.ConfirmEmail);

            dbContext.Users.Add(user);
            if (dbContext.SaveChanges() < 1)
            {
                HttpContext.Session.Remove("UserEmail");
                HttpContext.Session.Remove("UserConfirmEmail");
                ViewBag.msg = "Registration failed!";
                return View(model);
            }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("huyenle021039@gmail.com");
            mail.To.Add(model.UsersEmail);
            mail.Subject = "Confirm your account";
            mail.Body = "Welcome to LQHV Realtors Portal Your authentication code is: " + user.ConfirmEmail + " . Please enter the confirmation code to login your account.  <LQHV>";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("huyenle021039@gmail.com", "fwnwnhallceirfsl");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return View("AuthenticationForm", model);

        }

        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("User") != null)
            {
                if (HttpContext.Session.GetString("UserAccount") == null)
                {
                    return RedirectToAction("Index", "UserHome");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UsersEmail, string UsersPassword)
        {
            LQHVContext dbContext = new LQHVContext();

            var user = dbContext.Users.Where(n => n.UsersEmail == UsersEmail && n.UsersPassword == UsersPassword).FirstOrDefault();
            if (user != null)
            {
                if (user.UsersStatus == false)
                {
                    ViewBag.msg = "Your account is not authenticated or locked!";
                    return View();
                }
                else
                {
                    Package package = dbContext.Packages.Where(p => p.PackagesId == user.PackagesId).FirstOrDefault();
                    PackageType packageTypes = dbContext.PackageTypes.Where(p => p.PackageTypeId == package.PackageTypeId).FirstOrDefault();

                    if (package != null && packageTypes.PackageTypeName.Contains("account"))
                    {
                        HttpContext.Session.SetString("User", user.UsersEmail + "_" + UsersPassword);
                        HttpContext.Session.SetString("UserAccount", user.UsersEmail);
                        HttpContext.Session.SetString("UserEmail", user.UsersEmail);
                        HttpContext.Session.SetString("UserId", user.UsersId.ToString());
                        HttpContext.Session.SetString("UserName", user.UsersFullname);
                        HttpContext.Session.SetString("UserImage", user.UsersImage);
                        //agent
                        return RedirectToAction("Index", "UserHome");
                    }
                    else
                    {
                        HttpContext.Session.SetString("User", user.UsersEmail + "_" + UsersPassword);
                        HttpContext.Session.SetString("UserEmail", user.UsersEmail);
                        HttpContext.Session.SetString("UserId", user.UsersId.ToString());
                        HttpContext.Session.SetString("UserName", user.UsersFullname);
                        HttpContext.Session.SetString("UserImage", user.UsersImage);
                        //saleler
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ViewBag.msg = "Invalid Email or Password";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("UserAccount");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserImage");

            return View("Login");
        }

        public IActionResult ViewAccountProfile()
        {

            var user = new User();

            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction(controllerName: "UserHome", actionName: "Logout");
            }
            else
            {
                var id = long.Parse(HttpContext.Session.GetString("UserId"));

                var users = user.GetUser(id);
                ViewBag.users = users;
                return View(users);
            }
        }


        [HttpPost] 
        public async Task<IActionResult> ChangeInfor(long UsersId, string UsersFullname, string UsersPhone, string UsersAddress, bool UsersGender, IFormFile UsersImage)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction(controllerName: "UserHome", actionName: "Logout");
            }

            var user = new User();
            if (UsersImage != null)
            {
                string[] typeAllow = { ".jpg", ".png", ".jpeg", ".jftf" };
                if (!typeAllow.Contains(Path.GetExtension(UsersImage.FileName).ToLower()))
                {
                    ViewBag.errorImage = "You must select true image type (jpg, png, jpeg, jftf)";
                    return View();
                }

                string filePath = "wwwroot/Image/User";
                string fileName = UsersImage.FileName.Replace(Path.GetExtension(UsersImage.FileName), "") + ".png";
                var fileNameWithPath = string.Concat(filePath, "/", fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await UsersImage.CopyToAsync(stream);
                }
                ViewBag.user = user.ChangeInfor(UsersId, UsersFullname, UsersPhone, UsersAddress, UsersGender, fileName);
                HttpContext.Session.SetString("UserId", UsersId.ToString());
                HttpContext.Session.SetString("UserName", UsersFullname);
                HttpContext.Session.SetString("UserImage", fileName);
                return RedirectToAction("ViewAccountProfile", "UserHome", UsersId);
            }
            ViewBag.user = user.ChangeInfor(UsersId, UsersFullname, UsersPhone, UsersAddress, UsersGender, null);
            HttpContext.Session.SetString("UserId", UsersId.ToString());
            HttpContext.Session.SetString("UserName", UsersFullname);
            return RedirectToAction("ViewAccountProfile", "UserHome", UsersId);
        }


        public IActionResult ChangePassword()
        {

            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction(controllerName: "UserHome", actionName: "Logout");
            }
            else
            {
                var user = new User();
                var UsersId = long.Parse(HttpContext.Session.GetString("UserId"));

                var users = user.GetUser(UsersId);
                ViewBag.users = users;
                return View();
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(long UsersId, string oldPassword, string newPassword, string ConfirmNewPassword)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction(controllerName: "UserHome", actionName: "Logout");
            }
            var user = new User();
            ViewBag.users = user.GetUser(UsersId);
            var users = user.GetUser(UsersId);

            if (users.UsersPassword != oldPassword)
            {
                ViewBag.result = "The current password is not correct! ";
                return View();
            }
            if (newPassword != ConfirmNewPassword)
            {
                ViewBag.result = " Confirm new password does not match.";
                return View();
            }
            ViewBag.result = user.ChangePassword(UsersId, oldPassword, newPassword, ConfirmNewPassword);
            if (ViewBag.result == false)
            {
                ViewBag.result = "Password change failed, please try again.";
                return View();
            }
            else
            {
                HttpContext.Session.Remove("UserEmail");
                HttpContext.Session.Remove("User");
                HttpContext.Session.Remove("UserAccount");
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserName");
                HttpContext.Session.Remove("UserImage");
                return View("Login");
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult AuthenticationForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AuthenticationForm(User model)
        {
            LQHVContext dbContext = new LQHVContext();

            string? email = HttpContext.Session.GetString("UserEmail");
            string ConfirmEmail = HttpContext.Session.GetString("UserConfirmEmail");
            if (ConfirmEmail == null)
            {
                return View("Register");
            }

            if (ConfirmEmail == model.ConfirmEmail)
            {
                User user = dbContext.Users.Where(o => o.ConfirmEmail == ConfirmEmail).FirstOrDefault();
                if (user != null)
                {
                    //check
                    user.UsersStatus = true;
                    dbContext.Users.Update(user);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        return RedirectToAction("Login", "UserHome");
                    }
                }
                else
                {
                    ViewBag.msg = "The verification code is not correct!";
                    return RedirectToAction("AuthenticationForm");
                }
            }
            ViewBag.msg = "Please enter the verification code to continue logging into your account";
            return View("AuthenticationForm", model);
        }
    }
}
