using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View("Payment");
        }
        public IActionResult TransactionHistory()
        {
            return View("TransactionHistory");
        }
    }
}
