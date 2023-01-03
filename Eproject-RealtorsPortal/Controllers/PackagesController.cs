using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class PackagesController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Package> indexBox;
        List<Package> indexList;
        Package package;
        Package ForDeletePackage;

        
        public IActionResult Index()
        {
            var user = new User();

            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction(controllerName: "UserHome", actionName: "Logout");
            }
            else
            {
                //Hiển thị trạng thái khi trạng thái == true / đã kích hoạt
                indexBox = LQHVContext.Packages.Where(s => s.PackagesStatus == true && s.PackageTypeId == 2).ToList();
                return View("Index", indexBox);
            }


        }
        public IActionResult Pay(int ID,decimal price)
        {
            package = LQHVContext.Packages.Where(s => s.PackagesId == ID && s.PackagesPrice == price).FirstOrDefault();

            HttpContext.Session.SetString("PackagesId", package.PackagesId.ToString());
            HttpContext.Session.SetString("PackagesPrice", package.PackagesPrice.ToString());

            return RedirectToAction("Pay","Payment");
        }

        /// <summary>
        /// View details of packages 
        /// </summary>
        /// <param name="ID">ID of package from DB</param>
        /// <returns>details of package</returns>
        public IActionResult packageDetails(long ID)
        {
            //Link qua trang details dựa theo ID
            package = LQHVContext.Packages.Where(s => s.PackagesId == ID).FirstOrDefault();
            return View("packageDetails", package);
        }

        /// <summary>
        /// Method create new package
        /// </summary>
        /// <returns>list of package</returns>
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
                return RedirectToAction("listPackage", "Packages");
            }
            return View("createPackage", model);
        }
        public IActionResult listPackage()
        {
            indexList = LQHVContext.Packages.ToList();
            return View("listPackage", indexList);
        }

        public IActionResult Delete(long id)
        {
            ForDeletePackage = LQHVContext.Packages.Where(p => p.PackagesId == id).FirstOrDefault();
            if (ForDeletePackage != null)
            {
                LQHVContext.Packages.Remove(ForDeletePackage);
                if (LQHVContext.SaveChanges() > 0)
                {
                    TempData["msg"] = "Delete successfully";
                    return RedirectToAction("listPackage","Packages");
                }
            }

            List<Package> list = LQHVContext.Packages.ToList();
            TempData["msg"] = "Delete failed";
            return View("listPackage",list);
        }
        /// <summary>
        /// Active a package if it was status = false
        /// </summary>
        /// <returns>list of package</returns>
        public IActionResult Active(long id)
        {
            using (var context = new LQHVContext())
            {
                // Get the entity that you want to update
                var entity = context.Packages.FirstOrDefault(e => e.PackagesId == id);
               
                entity.PackagesStatus = true;

                // Save the changes to the database
                context.SaveChanges();
                return RedirectToAction("listPackage", "Packages");
            }

            List<Package> list = LQHVContext.Packages.ToList();
            return View("listPackage", list);
        }
        public IActionResult Inactive(long id)
        {
            using (var context = new LQHVContext())
            {
                // Get the entity that you want to update
                var entity = context.Packages.FirstOrDefault(e => e.PackagesId == id);

                entity.PackagesStatus = false;

                // Save the changes to the database
                context.SaveChanges();
                return RedirectToAction("listPackage", "Packages");
            }

            List<Package> list = LQHVContext.Packages.ToList();
            return View("listPackage", list);
        }

        public ActionResult updatePackage(long id) // To fill data in the form to enable easy editing
        {
            using (var context = new LQHVContext())
            {
                var data = context.Packages.Where(x => x.PackagesId == id).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To specify that this will be invoked when post method is called
        public ActionResult updatePackage(long id, Package model)
        {
            using (var context = new LQHVContext())
            {
                var data = context.Packages.FirstOrDefault(x => x.PackagesId == id); // Use of lambda expression to access particular record from a database
                if (data != null) // Checking if any such record exist 
                {
                    data.PackagesName = model.PackagesName;
                    data.PackagesDuration = model.PackagesDuration;
                    data.PackagesPrice = model.PackagesPrice;
                    data.PackagesDesc = model.PackagesDesc;
                    data.PackageType = model.PackageType;
                    data.PackagesStatus = model.PackagesStatus;
                    context.SaveChanges();
                    return RedirectToAction("listPackage", "Packages"); // It will redirect to the Read method
                }
                else                    
                    return View("listPackage", data);
            }
        }


    }
}
