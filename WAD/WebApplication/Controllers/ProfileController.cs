using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        /// <summary>
        /// Gets User manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Profile()
        {
            var currentUser = db.Users.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            var model = new UserViewModel();
            model.Email = currentUser.Email;
            model.UserName = currentUser.UserName;
            model.UserId = currentUser.Id;
            model.NameIdentifier = currentUser.NameIdentifier;
            return View(model);
        }

        [HttpGet]
        public ActionResult Accounts()
        {
            AccountsViewModel acc = new AccountsViewModel();
            var user = db.Users.Where(m => m.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            acc.Accounts = db.Accounts.Where(m => m.UserId.Equals(user.Id)).ToList();
            acc.CreateAccountVM = new CreateAccountViewModel();

            return View(acc);
        }

        // GET: Accounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser account = db.Users.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        [HttpGet]
        public ActionResult Edit()
        {
            var currentUser = db.Users.Where(m => m.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var model = new UserViewModel();
            model.Email = currentUser.Email;
            model.UserName = currentUser.UserName;
            model.UserId = currentUser.Id;
            model.NameIdentifier = currentUser.NameIdentifier;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                user.NameIdentifier = model.NameIdentifier;
                model.UserId = user.Id;
                db.SaveChanges();
                return RedirectToAction("Profile", model);
            }
            return View(model);
        }


        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("SetBalance?accountId={accountId}")]
        [AllowAnonymous]
        public ActionResult SetBalance(int accountId)
        {
            var account = db.Accounts.Where(m=>m.AccountId == accountId).FirstOrDefault();
            if (account != null)
            {
                return View("SetBalance", "Home", account);
            }
            else
            {
                return RedirectToAction("InvalidUser", "Accounts");
            }

        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CreateAccount(CreateAccountViewModel model)
        {
            var currentUser = db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            var admin = db.Users.Where(m => m.NameIdentifier.Equals("admin@admin.com")).FirstOrDefault();
            var account = new Account();
            if (ModelState.IsValid)
            {
                account.UserId = currentUser.Id;
                account.CardLimit = model.CardLimit;
                account.Iban = model.Iban;
                account.Currency = model.Currency;

                var callbackUrl = Url.Action("SetBalance", "Admin", new { userId = currentUser.Id, iban = account.Iban }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(admin.Id, "Set balance", "An acccount has been created for the user " + currentUser.NameIdentifier + "Set balance from <a href=\"" + callbackUrl + "\">here</a>");
                db.Accounts.Add(account);
                db.SaveChanges();

                return RedirectToAction("Accounts");
            }

            return RedirectToAction("Error", "Profile");
        }
      }
}