using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        Eproject_RealtorsPortal.Data.LQHVContext dbContext = new LQHVContext();

        public IActionResult ViewCountry()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            List<Country> countries = dbContext.Countries.ToList();
            return View(countries);

        }

        public IActionResult AddCountry()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddCountry(Country model)
        {
            Country countries = dbContext.Countries.Where(s => s.CountriesName == model.CountriesName).FirstOrDefault();
            if (countries != null)
            {
                ViewBag.msg = "Name already exist!.";
                return View(model);
            }
            var country = new Country()
            {
                CountriesName = model.CountriesName
            };
            dbContext.Countries.Add(country);
            if (dbContext.SaveChanges() >= 1)
            {
                ViewBag.sg = "successfully added new.";
                return View(model);
            }
            return View(model);
        }

        public IActionResult DeleteCountry(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Country countries = dbContext.Countries.Where(c => c.CountriesId == id).FirstOrDefault();
            if (countries != null)
            {
                dbContext.Countries.Remove(countries);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewCountry", "Location");
                }
            }
            return View(countries);
        }

        public IActionResult EditCountry(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Country countries = dbContext.Countries.Where(c => c.CountriesId == id).FirstOrDefault();
            return View(countries);
        }

        [HttpPost]
        public IActionResult EditCountry(Country model)
        {
            Country countries = dbContext.Countries.Where(c => c.CountriesId == model.CountriesId).FirstOrDefault();
            var check = dbContext.Countries.Where(s => s.CountriesName == model.CountriesName && model.CountriesName != countries.CountriesName).FirstOrDefault();

            if (countries != null)
            {
                if (check != null)
                {
                    ViewBag.msg = "Name already exist!";
                    return View(model);
                }
                if (model.CountriesName != null)
                {
                    countries.CountriesName = model.CountriesName;

                    dbContext.Countries.Update(countries);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Update successful.";
                        return View(model);
                    }
                    ViewBag.msg = "Update failed!";
                    return View(model);
                }
            }
            return View(model);
        }


        // ++++ Region ++++ //
        public IActionResult ViewRegion()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            List<Region> regions = dbContext.Regions.ToList();
            return View(regions);

        }

        public IActionResult AddRegion()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddRegion(Region model)
        {
            Region region = dbContext.Regions.Where(s => s.CountriesId == model.CountriesId).FirstOrDefault();
            var re = dbContext.Regions.Where(s => s.RegionsName == model.RegionsName && model.RegionsName != region.RegionsName).FirstOrDefault();
            if (re != null)
            {
                ViewBag.msg = "Name already exist!.";
                return View(model);
            }
            var regions = new Region()
            {
                CountriesId = model.CountriesId,
                RegionsName = model.RegionsName
            };
            dbContext.Regions.Add(regions);
            if (dbContext.SaveChanges() >= 1)
            {
                ViewBag.sg = "successfully added new.";
                return View(model);
            }
            return View(model);
        }

        public IActionResult DeleteRegion(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Region region = dbContext.Regions.Where(c => c.RegionsId == id).FirstOrDefault();
            if (region != null)
            {
                dbContext.Regions.Remove(region);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewRegion", "Location");
                }
            }
            return View(region);
        }

        public IActionResult EditRegion(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Region regions = dbContext.Regions.Where(c => c.RegionsId == id).FirstOrDefault();
            return View(regions);
        }

        [HttpPost]
        public IActionResult EditRegion(Region model)
        {
            Region regions = dbContext.Regions.Where(c => c.RegionsId == model.RegionsId).FirstOrDefault();
            var check = dbContext.Regions.Where(s => s.RegionsName == model.RegionsName && model.RegionsName != regions.RegionsName).FirstOrDefault();

            if (regions != null)
            {
                if (check != null)
                {
                    ViewBag.msg = "Name already exist!";
                    return View(model);
                }
                if (model.RegionsName != null)
                {
                    regions.RegionsName = model.RegionsName;

                    dbContext.Regions.Update(regions);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Update successful.";
                        return View(model);
                    }
                    ViewBag.msg = "Update failed!";
                    return View(model);
                }
            }
            return View(model);
        }


        // ++++ City ++++ //
        public IActionResult ViewCity()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            List<City> cities = dbContext.Cities.ToList();
            return View(cities);

        }

        public IActionResult AddCity()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddCity(City model)
        {
            List<City> city = dbContext.Cities.Where(s => s.RegionsId == model.RegionsId).ToList();
            foreach (var item in city)
            {
                if (item.CitiesName == model.CitiesName)
                {
                    ViewBag.msg = "Name already exist!.";
                    return View(model);
                }
                var cities = new City()
                {
                    RegionsId = model.RegionsId,
                    CitiesName = model.CitiesName
                };
                dbContext.Cities.Add(cities);
                if (dbContext.SaveChanges() >= 1)
                {
                    ViewBag.sg = "successfully added new.";
                    return View(model);
                }
                return View(model);
            }
            ViewBag.sg = "Add failed!";
            return View(model);
        }

        public IActionResult DeleteCity(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            City cities = dbContext.Cities.Where(c => c.CitiesId == id).FirstOrDefault();
            if (cities != null)
            {
                dbContext.Cities.Remove(cities);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewCity", "Location");
                }
            }
            return View();
        }

        public IActionResult EditCity(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            City cities = dbContext.Cities.Where(c => c.CitiesId == id).FirstOrDefault();
            return View(cities);
        }

        [HttpPost]
        public IActionResult EditCity(City model)
        {
            City cities = dbContext.Cities.Where(c => c.CitiesId == model.CitiesId).FirstOrDefault();
            var check = dbContext.Cities.Where(s => s.CitiesName == model.CitiesName && model.CitiesName != cities.CitiesName).FirstOrDefault();

            if (cities != null)
            {
                if (check != null)
                {
                    ViewBag.msg = "Name already exist!";
                    return View(model);
                }
                if (model.CitiesName != null)
                {
                    cities.CitiesName = model.CitiesName;

                    dbContext.Cities.Update(cities);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Update successful.";
                        return View(model);
                    }
                    ViewBag.msg = "Update failed!";
                    return View(model);
                }
            }
            ViewBag.msg = "Update failed!";
            return View(model);
        }



        // ++++ Areas ++++ //
        public IActionResult ViewArea()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }
            List<Area> area = dbContext.Areas.ToList();
            return View(area);

        }

        public IActionResult AddArea()
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddArea(Area model)
        {
            Area area = dbContext.Areas.Where(s => s.CitiesId == model.CitiesId).FirstOrDefault();
            var ar = dbContext.Areas.Where(s => s.AreasName == model.AreasName && model.AreasName != area.AreasName).FirstOrDefault();
            if (ar != null)
            {
                ViewBag.msg = "Name already exist!.";
                return View(model);
            }
            var areas = new Area()
            {
                CitiesId = model.CitiesId,
                AreasName = model.AreasName
            };
            dbContext.Areas.Add(areas);
            if (dbContext.SaveChanges() >= 1)
            {
                ViewBag.sg = "successfully added new.";
                return View(model);
            }
            return View(model);
        }

        public IActionResult DeleteArea(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Area area = dbContext.Areas.Where(c => c.AreasId == id).FirstOrDefault();
            if (area != null)
            {
                dbContext.Areas.Remove(area);
                if (dbContext.SaveChanges() >= 1)
                {
                    return RedirectToAction("ViewRegion", "Location");
                }
            }
            return View(area);
        }

        public IActionResult EditArea(long id)
        {
            if (HttpContext.Session.GetString("adminId") == null)
            {
                return RedirectToAction("LoginAdmin", "Home");
            }

            Area area = dbContext.Areas.Where(c => c.AreasId == id).FirstOrDefault();
            return View(area);
        }

        [HttpPost]
        public IActionResult EditArea(Area model)
        {
            Area area = dbContext.Areas.Where(c => c.AreasId == model.AreasId).FirstOrDefault();
            var check = dbContext.Areas.Where(s => s.AreasName == model.AreasName && model.AreasName != area.AreasName).FirstOrDefault();

            if (area != null)
            {
                if (check != null)
                {
                    ViewBag.msg = "Name already exist!";
                    return View(model);
                }
                if (model.AreasName != null)
                {
                    area.AreasName = model.AreasName;

                    dbContext.Areas.Update(area);
                    if (dbContext.SaveChanges() >= 1)
                    {
                        ViewBag.sg = "Update successful.";
                        return View(model);
                    }
                    ViewBag.msg = "Update failed!";
                    return View(model);
                }
            }
            return View(model);
        }

    }
}
