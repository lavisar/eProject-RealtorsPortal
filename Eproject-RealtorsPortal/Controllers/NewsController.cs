using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Eproject_RealtorsPortal.Data;

namespace Eproject_RealtorsPortal.Controllers
{
    public class NewsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<News> indexList;
        News news;
        News ForDeleteNews;
        public IActionResult Index()
        {
            List<News> list = LQHVContext.News.ToList();
            return View(list);
        }
        public IActionResult Details(int id)
        {
            News news = LQHVContext.News.Where(s => s.NewsId == id).FirstOrDefault();
            return View("Details", news);
        }
        [HttpPost]

        public IActionResult Details(News model)
        {
            News news = LQHVContext.News.Where(s => s.NewsId == model.NewsId).FirstOrDefault();
            return View(news);
        }   

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IActionResult createNews()
        {
            return View(new News());
        }
        [HttpPost]
        public IActionResult createNews(News model)
        {
            LQHVContext.News.Add(model);
            if (LQHVContext.SaveChanges() == 1)
            {
                return RedirectToAction("listNews", "News");
            }
            return View("createNews", model);
        }
        public IActionResult listNews()
        {
            indexList = LQHVContext.News.ToList();
            return View("listNews", indexList);
        }

        
        [HttpGet]
        public IActionResult Delete(long ID)
        {
            ForDeleteNews = LQHVContext.News.Where(p => p.NewsId == ID).FirstOrDefault();
            if (ForDeleteNews != null)
            {
                LQHVContext.News.Remove(ForDeleteNews);
                if (LQHVContext.SaveChanges() > 0)
                {
                    TempData["msg"] = "Delete successfully";
                    return RedirectToAction("listNews", "News");
                }
            }

            List<News> indexList = LQHVContext.News.ToList();
            TempData["msg"] = "Delete failed";
            return View("listNews", indexList);
        }

        public void UploadImage(HttpContext context)
        {
            if (context.Request.Method == "POST")
            {
                var file = context.Request.Form.Files[0];
                if (file != null)
                {
                    // Read the file and save it to a desired location
                    using (var stream = new FileStream("path/to/save/file.jpg", FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
        }

        public IActionResult updateNews(long ID)
        {
            using (var context = new LQHVContext())
            {
                var data = context.News.Where(x => x.NewsId == ID).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult updateNews(long ID, News model)
        {
            using (var context = new LQHVContext())
            {
                var data = context.News.FirstOrDefault(x => x.NewsId == ID);
                if (data != null)
                {
                    data.NewsTitle = model.NewsTitle;
                    data.NewsDate = model.NewsDate;
                    data.NewsDesc = model.NewsDesc;
                    data.NewsContent = model.NewsContent;
                    data.NewsImage = model.NewsImage;
                    context.SaveChanges();
                    return RedirectToAction("listNews", "News");
                }
                else
                    return View("listNews", data);
            }
        }

    }
}
