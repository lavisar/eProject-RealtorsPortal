using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class AdsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Product> rent,sell;

        //public IActionResult Sell()
        //{
        //    sell = LQHVContext.Products;
        //        .Join(
        //        LQHVContext.Categories,
        //        p => p.CategoryId,
        //        c => c.CategoryId,
        //        (p, c) => new
        //        {
        //            ProductTitle = p.ProductTitle,
        //            ProductPrice = p.ProductPrice,
        //            ProductImage = p.ProductImage,
        //            CategoryID = c.CategoryId
        //        })
        //    .Join(
        //        LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == 2),

        //    )
        //    .Select;
        //    return View("Sell", sell);
        //}

        public IActionResult Rent()
        {
            rent = LQHVContext.Products.Where(s => s.Category.BusinessTypes.BusinessTypesId == 1).ToList();
            
            return View("Rent",rent);
        }

    }
}
