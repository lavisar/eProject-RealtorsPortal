using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using System.Linq;

namespace Eproject_RealtorsPortal.Controllers
{
    public class AdsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<ProductBox> rent, sell, allOwnAds,search;
        //List<Product> products;
        ProductDetail product;
        Product products;
        List<Package> package;
        List<Category> category;
        List<Area> areas;
        List<City> city;
        List<Country> country;
        public IActionResult Sell()
        {
            sell = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.Status == "active")
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
            rent = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.Status == "active")
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
            //productAdd.Categories = LQHVContext.Categories.ToList();
            //productAdd.Packages = LQHVContext.Packages.ToList();

            package = LQHVContext.Packages.Where(pac => pac.PackageTypeId == 2).ToList();
            category = LQHVContext.Categories.ToList();
            areas = LQHVContext.Areas.ToList();
            city = LQHVContext.Cities.ToList();
            //region = LQHVContext.Regions.ToList();
            country = LQHVContext.Countries.ToList();
            return View(new ProductAdd { Package = package, Categories = category, Countries = country, Areas = areas, Cities = city });
        }
        [HttpPost]
        public IActionResult CreateAds(ProductAdd model)
        {
            products = new Product
            {
                ProductAddress = model.ProductAddress + ", " + model.AreaName + ", " + model.CityName + ", " + model.CityName,
                ProductArea = model.ProductArea,
                PackagesId = model.PackagesId,
                ProductDesc = model.ProductDesc,
                PhoneNumber = model.PhoneNumber,
                ProductImage = model.ProductImage,
                ProductInterior = model.ProductInterior,
                ProductLegal = model.ProductLegal,
                ProductPrice = model.ProductPrice,
                ProductTitle = model.ProductTitle,
                StartDate = model.StartDate,
                EndDate = model.StartDate.AddDays(model.NumDays),
                Featured = model.Featured,
                ContactAddress = model.ContactEmail,
                CategoryId = model.CategoryId,
                ContactEmail = model.ContactEmail,
                ContactName = model.ContactName,
                NumDays = model.NumDays,
                NumBedrooms = model.NumBedrooms,
                NumOfFloors = model.NumOfFloors,
                NumToilets = model.NumToilets,
                HomeOrientation = model.HomeOrientation,
                BalconyOrientation = model.BalconyOrientation,
                UsersId = model.UsersId
            };
            LQHVContext.Products.Add(products);
            if (LQHVContext.SaveChanges() == 1)
            {
                HttpContext.Session.SetString("ProductId", products.ProductId.ToString());
                HttpContext.Session.SetString("PackageId", products.PackagesId.ToString());
                if (products.Packages.PackageType.PackageTypeId == 2)
                {
                    HttpContext.Session.SetString("PayType", "ads");
                    HttpContext.Session.SetString("PackagePrice", products.Packages.PackagesPrice.ToString());
                }
                return RedirectToAction("Pay", "Payment");
            }
            return View("CreateAds", model);
        }
        public IActionResult AllOwnAds(int Id)
        {
            allOwnAds = LQHVContext.Products.Where(dd => dd.UsersId == Id)
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
            return View(allOwnAds);
        }
        public IActionResult Search(long Type, string Area, string City, decimal StartPrice, decimal EndPrice)
        {

            if (Area == null)
            {
                search = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.ProductAddress.Contains(City) && d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active").Join(
                LQHVContext.Categories,
                p => p.CategoryId,
                c => c.CategoryId,
               (p, c) => new
               {
                   Product = p,
                   Category = c
               })
               .Join(LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == Type),
                     ca => ca.Category.BusinessTypesId,
                     bu => bu.BusinessTypesId,
                (ca, bu) => new
                {
                    ca.Product,
                    ca.Category,
                    BusinessTypeID = bu.BusinessTypesId
                })
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
            }
            if (City == null)
            {
                search = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.ProductAddress.Contains(Area) && d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active").Join(
                LQHVContext.Categories,
                p => p.CategoryId,
                c => c.CategoryId,
               (p, c) => new
               {
                   Product = p,
                   Category = c
               })
               .Join(LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == Type),
                     ca => ca.Category.BusinessTypesId,
                     bu => bu.BusinessTypesId,
                (ca, bu) => new
                {
                    ca.Product,
                    ca.Category,
                    BusinessTypeID = bu.BusinessTypesId
                })
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
            }
            if(Area == null && City == null)
            {
                search = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today &&d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active").Join(
                LQHVContext.Categories,
                p => p.CategoryId,
                c => c.CategoryId,
               (p, c) => new
               {
                   Product = p,
                   Category = c
               })
               .Join(LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == Type),
                     ca => ca.Category.BusinessTypesId,
                     bu => bu.BusinessTypesId,
                (ca, bu) => new
                {
                    ca.Product,
                    ca.Category,
                    BusinessTypeID = bu.BusinessTypesId
                })
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
            }
            else
            {
                search = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.ProductAddress.Contains(Area) && d.ProductAddress.Contains(City) && d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active").Join(
                LQHVContext.Categories,
                p => p.CategoryId,
                c => c.CategoryId,
               (p, c) => new
               {
                   Product = p,
                   Category = c
               })
               .Join(LQHVContext.BusinessTypes.Where(s => s.BusinessTypesId == Type),
                     ca => ca.Category.BusinessTypesId,
                     bu => bu.BusinessTypesId,
                (ca, bu) => new
                {
                    ca.Product,
                    ca.Category,
                    BusinessTypeID = bu.BusinessTypesId
                })
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
            }

            return View("Search", search);
        }
    }
}
