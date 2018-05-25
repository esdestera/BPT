using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TaxController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tax
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Continue(BillViewModel model)
        {
            var tax = db.Taxes.Where(m => m.Name.Equals(model.Name)).FirstOrDefault();
            if (tax != null)
            {
                //var destUser = db.Accounts.Where(m => m.AccountId.Equals(bill.BillAccount.AccountId)).FirstOrDefault();
                // model.Iban = destUser.Iban;
                return View("TaxConfirmation", model);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }
    }
}