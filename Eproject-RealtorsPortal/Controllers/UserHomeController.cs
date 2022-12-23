using Microsoft.AspNetCore.Mvc;
using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class UserHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrWhiteSpace(Request.Cookies["user"]))
            {
                string[] uservalues = Request.Cookies["user"].Split('_');
                if (uservalues.Count() == 2)
                {
                    HttpContext.Session.SetString("user", uservalues[0] + "_" + uservalues[1]);
                }
            }
            if (HttpContext.Session.GetString("user") != null)
            {
                return RedirectToAction("Index", "UserHome");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(User model)
        {
            LQHVContext dbContext= new LQHVContext();
            User  user = dbContext.Users.Where(u => u.UsersEmail == model.UsersEmail && u.UsersPassword == model.UsersPassword ).FirstOrDefault();  
            if (user != null)
            {
                //if (model.Remember)
                //{
                //    //Ghi tai khoan da dang nhap vao cookie khi dang nhao tahnh cong
                //    Response.Cookies.Append("user", model.UsersEmail + "_" + model.UsersPassword,
                //        new CookieOptions
                //        {
                //            Expires = DateTime.Now.AddMinutes(5)
                //        });
                //}
                HttpContext.Session.SetString("user", user.UsersEmail + "_" + model.UsersPassword);
                return RedirectToAction("Index", "UserHome");
            }
            ViewData["Err"] = "Invalid Email or Password";
            return View(model);
        }

        public IActionResult AccountProfile()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult AuthenticationForm()
        {
            return View();
        }

    }
}
