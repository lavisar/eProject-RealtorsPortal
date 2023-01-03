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
        decimal payment = 0;
        int category;
        int user;
        int listing;

        DateTime yesterday = DateTime.Today.AddDays(-2);
        DateTime Today = DateTime.Now.Date;
        public IActionResult Index()
        {

            if (LQHVContext.Payments != null)
            {
                payment = LQHVContext.Payments.Sum(x => x.PaymentTotal);                
            }

            category = LQHVContext.Categories.Count();
            user = LQHVContext.Users.Count();
            listing = LQHVContext.Products.Count();

            return View(new Report { Payment = payment, CategoriesCount = category, userCount = user, listingCount = listing });
        }
    }
}
