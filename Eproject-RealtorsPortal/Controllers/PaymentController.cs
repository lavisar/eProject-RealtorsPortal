using Microsoft.AspNetCore.Mvc;
using Eproject_RealtorsPortal.Models;
using Eproject_RealtorsPortal.Data;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PaymentController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Payment> payments;
        public IActionResult Pay()
        {
            return View("Pay",new Payment());
        }
        [HttpPost]
        public IActionResult Pay(Payment pay)
        {
            LQHVContext.Payments.Add(pay);
            if (LQHVContext.SaveChanges() == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Pay",pay);
        }
        public IActionResult TransactionHistory()
        {
            long userID = long.Parse(HttpContext.Session.GetString("UserId"));
            payments = LQHVContext.Payments.Where(s => s.PaymentStatus == true && s.UsersId == userID).ToList();
            return View("TransactionHistory",payments);
        }
    }
}
