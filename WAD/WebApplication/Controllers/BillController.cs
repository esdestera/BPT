using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class BillController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Bill
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Continue(BillViewModel model)
        {
            var bill = db.Bills.Where(m => m.Name.Equals(model.Name)).FirstOrDefault();
            if (bill != null)
            {
                //var destUser = db.Accounts.Where(m => m.AccountId.Equals(bill.BillAccount.AccountId)).FirstOrDefault();
                // model.Iban = destUser.Iban;
                return View("BillConfirmation", model);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }
    }
}