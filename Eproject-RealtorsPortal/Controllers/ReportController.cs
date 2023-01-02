using Microsoft.AspNetCore.Mvc;
using Eproject_RealtorsPortal.Models;
using Eproject_RealtorsPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class ReportController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        decimal payment;
        int category;
        int user;
        int listing;

        DateTime yesterday = DateTime.Today.AddDays(-1);
        DateTime Today = DateTime.Today;
        public IActionResult Index()
        {
            payment = LQHVContext.Payments
                .Where(p => p.PaymentDatetime > yesterday && p.PaymentDatetime < Today)
                .Select(s => s.PaymentTotal)
                .Sum();

            category = LQHVContext.Categories.Count();
            user = LQHVContext.Users.Count();
            listing = LQHVContext.Products.Count();

            return View(new Report { Payment = payment, CategoriesCount = category, userCount = user, listingCount = listing });
        }

    }
}
