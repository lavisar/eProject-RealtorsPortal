using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Web;
namespace Eproject_RealtorsPortal.Controllers
{
    public class AdsController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<ProductBox> rent, sell, allOwnAds;
        //List<Product> products;
        ProductDetail product;
        Product products;
        List<Package> package;
        List<Category> category;
        List<Area> areas;
        List<City> city;
        List<Region> region;
        List<Country> country;
        Image image;
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
            region = LQHVContext.Regions.ToList();
            country = LQHVContext.Countries.ToList();
            return View(new ProductAdd { Package = package,Categories = category, Countries = country, Areas = areas, Cities = city });
        }
        [HttpPost]
        public IActionResult CreateAds(ProductAdd model)
        {
            if (HttpContext.Request.Method == "POST")
            {
                // Get the uploaded files
                IFormFileCollection files = HttpContext.Request.Form.Files;

                // Iterate through the files and save each one to a file and the database

                    IFormFile file = files[0];
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Ads", fileName);

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
                    image = new Image
                    {
                        ImagePath = ManyImage.FileName,
                        ProductId = 1

                    };
                    // Insert the image into the database
            }
            products = new Product {
                ProductAddress = model.ProductAddress + ", " + model.AreaName + ", " + model.CityName + ", " + model.CityName,
                ProductArea = model.ProductArea,
                PackagesId = model.PackagesId,
                ProductDesc = model.ProductDesc,
                PhoneNumber = model.PhoneNumber,
                ProductImage = image.ImagePath,
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
                HttpContext.Session.SetString("PayType", "ads");
                //HttpContext.Session.SetString("PackagePrice", products.Packages.PackagesPrice.ToString());
                if (HttpContext.Request.Method == "POST")
                {
                    // Get the uploaded files
                    IFormFileCollection files = HttpContext.Request.Form.Files;

                    // Iterate through the files and save each one to a file and the database
                    for (int i = 1; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Ads", fileName);

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
                            ProductId = 1

                        };
                        // Insert the image into the database
                        LQHVContext.Images.Add(image);
                        LQHVContext.SaveChanges();
                    }
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
        public IActionResult SearchRent(string Area, string City, long StartPrice, long EndPrice)
        {
            rent = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.ProductAddress.Contains(Area) && d.ProductAddress.Contains(City) && d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active")
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
            return View("Search",rent);
        }
        public IActionResult SearchSell(string Area, string City, long StartPrice, long EndPrice)
        {
            sell = LQHVContext.Products.Where(d => d.StartDate <= DateTime.Today && d.EndDate > DateTime.Today && d.ProductAddress.Contains(Area) && d.ProductAddress.Contains(City) && d.ProductPrice >= StartPrice && d.ProductPrice <= EndPrice && d.Status == "active")
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
            return View("Search", sell);
        }


        public IActionResult listAds()
        {
            var indexList = LQHVContext.Products.ToList();
            return View("listAds", indexList);
        }
        public IActionResult DeleteList(long id)
        {
            var ForDelete = LQHVContext.Products.Where(p => p.ProductId == id).FirstOrDefault();
            if (ForDelete != null)
            {
                LQHVContext.Products.Remove(ForDelete);
                if (LQHVContext.SaveChanges() > 0)
                {
                    return RedirectToAction("listAds", "Ads");
                }
            }

            List<Product> list = LQHVContext.Products.ToList();
            return View("listAds", list);
        }
        public IActionResult detailForList(long ID)
        {
            //Link qua trang details dựa theo ID
            var detail = LQHVContext.Products.Where(s => s.ProductId == ID).FirstOrDefault();
            return View("detailForList", detail);
        }


    }
}
