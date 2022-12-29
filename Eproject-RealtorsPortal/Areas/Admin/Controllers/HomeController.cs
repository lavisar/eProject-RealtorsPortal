using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
