using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Eproject_RealtorsPortal.Data;

namespace Eproject_RealtorsPortal.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            LQHVContext lQHVContext = new LQHVContext();
            List<News> list = lQHVContext.News.ToList();
            return View(list);
        }
        public IActionResult Details(int id)
        {
            LQHVContext lQHVContext = new LQHVContext();
            News news = lQHVContext.News.Where(s => s.NewsId == id).FirstOrDefault();
            return View("Details", news);
        }
        [HttpPost]

        public IActionResult Details(News model)
        {
            LQHVContext lQHVContext = new LQHVContext();
            News news = lQHVContext.News.Where(s => s.NewsId == model.NewsId).FirstOrDefault();
            return View(news);
        }
    }
}
