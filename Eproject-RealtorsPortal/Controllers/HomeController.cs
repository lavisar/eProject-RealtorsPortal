using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Eproject_RealtorsPortal.Data;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class HomeController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<ProductBox> featured;
        ProductDetail product;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            featured = LQHVContext.Products.Where(w => w.Featured == true)
               .Join(
               LQHVContext.Categories,
               p => p.CategoryId,
               c => c.CategoryId,
               (p, c) => new
               {
                   Product = p,
                   Category = c
               })
           .Join(
               LQHVContext.BusinessTypes,
               ca => ca.Category.BusinessTypesId,
               bu => bu.BusinessTypesId,
               (ca, bu) => new
               {
                   ca.Product,
                   ca.Category,
                   BusinessTypeID = bu.BusinessTypesId
               }
           )
           .Select(s => new ProductBox
           {
               ProductID = s.Product.ProductId,
               ProductTitle = s.Product.ProductTitle,
               ProductPrice = s.Product.ProductPrice,
               ProductArea = s.Product.ProductArea,
               ProductAddress = s.Product.ProductAddress,
               BusinessTypeID = s.BusinessTypeID
           })
           .ToList();

            return View(featured);
        }        
        public IActionResult UserHome()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult FAQ()
        {
            return View("FAQ");
        }
        public IActionResult AboutUs()
        {
            return View("AboutUs");
        }
        [HttpPost]
        public IActionResult createContact(Contact model)
        {
            LQHVContext.Contacts.Add(model);
            if (LQHVContext.SaveChanges() == 1)
            {
                return RedirectToAction("AboutUs", "Home");
            }
            return View("createContact", model);
        }
    }
}