using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PackagesController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Package> indexBox;
        public IActionResult Index()
        {
            //Hiển thị trạng thái khi trạng thái == true / đã kích hoạt
            indexBox = LQHVContext.Packages.Where(s => s.PackagesStatus == true).ToList();
            return View("Index", indexBox);
        }

    }
}
