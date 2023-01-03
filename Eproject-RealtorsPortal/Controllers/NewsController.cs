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
        News ForDeleteNews;
        ManyImage ManyImage;
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
            return View(new NewsAdd());
        }
        [HttpPost]
        public IActionResult createNews(NewsAdd model)
        {
            if (HttpContext.Request.Method == "POST")
            {
                // Get the uploaded files
                IFormFileCollection files = HttpContext.Request.Form.Files;

                // Iterate through the files and save each one to a file and the database
                if (files.Count > 0)
                {
                    IFormFile file = files[0];
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "News", fileName);

                    // Use the FileStream class to save the file to a file on the server
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                    // Read the file data into a byte array
                    byte[] data = System.IO.File.ReadAllBytes(filePath);


                    // Create a new Image instance
                    ManyImage = new ManyImage
                    {
                        FileName = fileName,
                        Data = data
                    };
                }

                // Insert the image into the database
            }
            News news = new News
            {
                NewsId = model.NewsId,
                NewsTitle = model.NewsTitle,
                NewsDate = model.NewsDate,
                NewsDesc = model.NewsDesc,
                NewsContent = model.NewsContent,
                NewsImage = ManyImage.FileName
            };
            LQHVContext.News.Add(news);
            if (LQHVContext.SaveChanges() == 1)
            {
                if (HttpContext.Request.Method == "POST")
                {
                    // Get the uploaded files
                    IFormFileCollection files = HttpContext.Request.Form.Files;

                    // Iterate through the files and save each one to a file and the database
                    for (int i = 1; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "News", fileName);

                        // Use the FileStream class to save the file to a file on the server
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fs);
                        }

                        // Read the file data into a byte array
                        byte[] data = System.IO.File.ReadAllBytes(filePath);


                        // Create a new Image instance
                        ManyImage ManyImage = new ManyImage
                        {
                            FileName = fileName,
                            Data = data
                        };
                        Image image = new Image
                        {
                            ImagePath = ManyImage.FileName,
                            NewsId = long.Parse(HttpContext.Session.GetString("NewsId"))

                        };
                        // Insert the image into the database
                        LQHVContext.Images.Add(image);
                        LQHVContext.SaveChanges();
                    }
                }
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
