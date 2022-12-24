using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class AdsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Product> rent,sell;
        public IActionResult Sell()
        {
            sell = LQHVContext.Products.Where(s => s.Category.BusinessTypes.BusinessTypesId == 2).ToList();
            return View("Sell",sell);
        }
        public IActionResult Rent()
        {
            rent = LQHVContext.Products.Where(s => s.Category.BusinessTypes.BusinessTypesId == 1).ToList();
            return View("Rent",rent);
        }
    }
}
