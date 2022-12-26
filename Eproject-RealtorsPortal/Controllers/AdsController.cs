using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class AdsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<ProductBox> rent, sell;

        public IActionResult Sell()
        {
            sell = LQHVContext.Products
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
                LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == 2),
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
            return View("Sell", sell);
        }

        public IActionResult Rent()
        {
            rent = LQHVContext.Products
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
                LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == 1),
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

            return View("Rent", rent);
        }

    }
}
