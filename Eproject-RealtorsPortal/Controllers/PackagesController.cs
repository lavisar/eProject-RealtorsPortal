using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PackagesController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Package> indexBox;
        Package package;
        public IActionResult Index()
        {
            //Hiển thị trạng thái khi trạng thái == true / đã kích hoạt
            indexBox = LQHVContext.Packages.Where(s => s.PackagesStatus == true).ToList();
            return View("Index", indexBox);
        }

        public IActionResult packageDetails(long ID)
        {
            //Link qua trang details dựa theo ID
            package = LQHVContext.Packages.Where(s => s.PackagesId == ID).FirstOrDefault();
            return View("packageDetails", package);
        }
    }
}
