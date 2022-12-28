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
        //List<Product> products;
        ProductDetail product;

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
                ProductImage = s.Product.ProductImage,
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
                ProductImage = s.Product.ProductImage,
                BusinessTypeID = s.BusinessTypeID
            })
            .ToList();

            return View("Rent", rent);
        }
        public IActionResult Details(long ID)
        {
            product = LQHVContext.Products
                .Where(s => s.ProductId == ID)
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
            .Select(s => new ProductDetail
            {
                ProductID = s.Product.ProductId,
                ProductTitle = s.Product.ProductTitle,
                ProductDesc = s.Product.ProductDesc,
                ProductPrice = s.Product.ProductPrice,
                ProductArea = s.Product.ProductArea,
                ProductAddress = s.Product.ProductAddress,
                ProductImage = s.Product.ProductImage,
                ProductInterior = s.Product.ProductInterior,
                ProductLegal = s.Product.ProductLegal,
                PhoneNumber = s.Product.PhoneNumber,
                NumToilets = s.Product.NumToilets,
                NumBedrooms = s.Product.NumBedrooms,
                NumOfFloors = s.Product.NumOfFloors,
                ContactName = s.Product.ContactName,
                ContactAddress = s.Product.ContactAddress,
                ContactEmail = s.Product.ContactEmail,
                BalconyOrientation = s.Product.BalconyOrientation,
                HomeOrientation = s.Product.HomeOrientation,
                BusinessTypeID = s.BusinessTypeID
            }).FirstOrDefault();
            return View("Details", product);
        }
        public IActionResult CreateAds()
        {

            return View(new Product());
        }
        [HttpPost]
        public IActionResult CreateAds(Product model)
        {
            LQHVContext.Products.Add(model);
            if (LQHVContext.SaveChanges() == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("CreateAds", model);
        }
    }
}
