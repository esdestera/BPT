using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult SetBalance(string userId, string iban)
        {
            BalanceViewModel model = new BalanceViewModel();
            var user = db.Users.Where(m => m.Id.Equals(userId)).FirstOrDefault();
            
            if(user != null)
            {
                var account = db.Accounts.Where(m => m.Iban.Equals(iban)).FirstOrDefault();
                model.Balance = 0;
                model.UserName = user.NameIdentifier;
                model.Iban = account.Iban;
                model.UserId = user.Id;
                return View(model);
            }
            return RedirectToAction("Index");
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBalance(BalanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.Id.Equals(model.UserId)).FirstOrDefault();
                var account = db.Accounts.Where(m => m.UserId.Equals(user.Id) && m.Iban.Equals(model.Iban)).FirstOrDefault();
                account.Balance = model.Balance;
                db.SaveChanges();
                return RedirectToAction("Index", model);
            }

            return RedirectToAction("InvalidModel", model);
        }


        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
