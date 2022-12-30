using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class CateController : Controller    

    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Category> indexList;

        public IActionResult cateList()
        {
            indexList = LQHVContext.Categories.ToList();
            return View("cateList", indexList);
        }

        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {


            LQHVContext.Categories.Add(model);
            if (LQHVContext.SaveChanges() == 1)
            {
                //redirect to package list
                return RedirectToAction("cateList", "Cate");
            }
            return View("Create", model);
        }
        public IActionResult Active(long id)
        {
            using (var context = new LQHVContext())
            {
                // Get the entity that you want to update
                var entity = context.Categories.FirstOrDefault(e => e.CategoryId == id);

                entity.CategoryStatus = true;

                // Save the changes to the database
                context.SaveChanges();
                return RedirectToAction("cateList", "Cate");
            }

            List<Category> list = LQHVContext.Categories.ToList();
            return View("cateList", list);
        }
        public IActionResult Inactive(long id)
        {
            using (var context = new LQHVContext())
            {
                // Get the entity that you want to update
                var entity = context.Categories.FirstOrDefault(e => e.CategoryId == id);

                entity.CategoryStatus = false;

                // Save the changes to the database
                context.SaveChanges();
                return RedirectToAction("cateList", "Cate");
            }

            List<Category> list = LQHVContext.Categories.ToList();
            return View("cateList", list);
        }
        public IActionResult Delete(long id)
        {
            var ForDelete = LQHVContext.Categories.Where(p => p.CategoryId == id).FirstOrDefault();
            if (ForDelete != null)
            {
                LQHVContext.Categories.Remove(ForDelete);
                if (LQHVContext.SaveChanges() > 0)
                {
                    return RedirectToAction("cateList", "Cate");
                }
            }

            List<Category> list = LQHVContext.Categories.ToList();
            return View("cateList", list);
        }

        public ActionResult Update(long id) // To fill data in the form to enable easy editing
        {
            using (var context = new LQHVContext())
            {
                var data = context.Categories.Where(x => x.CategoryId == id).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To specify that this will be invoked when post method is called
        public ActionResult Update(long id, Category model)
        {
            using (var context = new LQHVContext())
            {
                var data = context.Categories.FirstOrDefault(x => x.CategoryId == id); // Use of lambda expression to access particular record from a database
                if (data != null) // Checking if any such record exist 
                {
                    data.CategoryName = model.CategoryName;
                    data.CategoryStatus = model.CategoryStatus;
                    data.BusinessTypesId = model.BusinessTypesId;
                    context.SaveChanges();
                    return RedirectToAction("cateList", "Cate"); // It will redirect to the Read method
                }
                else
                    return View("cateList", data);
            }
        }
    }
}
