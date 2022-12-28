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

        /// <summary>
        /// View details of packages 
        /// </summary>
        /// <param name="ID">ID of package from DB</param>
        /// <returns></returns>
        public IActionResult packageDetails(long ID)
        {
            //Link qua trang details dựa theo ID
            package = LQHVContext.Packages.Where(s => s.PackagesId == ID).FirstOrDefault();
            return View("packageDetails", package);
        }

        /// <summary>
        /// Method create new package
        /// </summary>
        /// <returns></returns>
        public IActionResult createPackage()
        {
            return View(new Package());
        }
        [HttpPost]
        public IActionResult createPackage(Package model)
        {
            LQHVContext.Packages.Add(model); 
            if (LQHVContext.SaveChanges() == 1)
            {
                //redirect to package list
                return RedirectToAction("packageList", "Packages");
            }
            return View("createPackage", model);
        }
    }
}
