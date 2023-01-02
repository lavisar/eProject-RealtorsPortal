using Eproject_RealtorsPortal.Data;
using Eproject_RealtorsPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eproject_RealtorsPortal.Controllers
{
    public class ContactController : Controller
    {
        LQHVContext LQHVContext = new LQHVContext();
        List<Contact> indexList;
        Contact contact;
        Contact ForDeleteContact;
               
        public IActionResult listContact()
        {
            indexList = LQHVContext.Contacts.ToList();
            return View("listContact", indexList);
        }


        [HttpGet]
        public IActionResult Delete(long ID)
        {
            ForDeleteContact = LQHVContext.Contacts.Where(p => p.ContactsId == ID).FirstOrDefault();
            if (ForDeleteContact != null)
            {
                LQHVContext.Contacts.Remove(ForDeleteContact);
                if (LQHVContext.SaveChanges() > 0)
                {
                    TempData["msg"] = "Delete successfully";
                    return RedirectToAction("listContact", "Contact");
                }
            }

            List<Contact> indexList = LQHVContext.Contacts.ToList();
            TempData["msg"] = "Delete failed";
            return View("listContact", indexList);
        }

    }
}
