using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Pay()
        {
            return View("Pay");
        }
        public IActionResult TransactionHistory()
        {
            return View("TransactionHistory");
        }
    }
}
