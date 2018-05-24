using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Defines the home controller class.
    /// </summary>
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        /// <summary>
        /// Initializes a new instance of the Home controller class.
        /// </summary>
        public HomeController()
        {
        }
              
        /// <summary>
        /// Initializes a new instance of the Home controller class.
        /// </summary>
        /// <param name="userManager">Application user manager.</param>
        public HomeController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Sets the home page based on therole of the user.
        /// </summary>
        /// <returns>The action result to the Home view or to the AdminIndex view.</returns>
        public ActionResult Home()
        {
            var db = new ApplicationDbContext();
            var currentUser = db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            var customUser = db.CustomUsers.Where(m => m.UserId == currentUser.Id).FirstOrDefault();
            
            bool isAdmin = HttpContext.User.IsInRole("Admin");
            ActionResult retValue = View();
            if (isAdmin)
            {
                retValue = RedirectToAction("Index", "Admin");
            }

            return retValue;
        }

        /// <summary>
        /// Users info page.
        /// </summary>
        /// <returns>Action result to the users info page.</returns>
        public ActionResult UsersInfo()
        {
            return View();
        }

        /// <summary>
        /// About page.
        /// </summary>
        /// <returns>The about page.</returns>
        public ActionResult About()
        {
            return View();
        }


        /// <summary>
        /// Deactivates the user with user id id.
        /// </summary>
        /// <param name="id">The id of the user to be deactivated.</param>
        /// <returns>A view to assure that the admin wants to deactivate the user.</returns>
        public ActionResult Deactivate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Deactivates the user with user id id.
        /// </summary>
        /// <param name="id">The id of the user to be deactivated.</param>
        /// <returns>A view to assure that the admin wants to deactivate the user.</returns>
        public ActionResult UpdateBalance(Account account, double amount)
        {
            if (account == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(account.UserId);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View();

        }

        /// <summary>
        /// Sets activate property from the database to true.
        /// </summary>
        /// <param name="id">The id of the user to be deactivated.</param>
        /// <returns>A view to the admin index page.</returns>
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmed(string id)
        {
            var user = db.Users.Find(id);
            user.Active = true;
            db.SaveChanges();
            return View("AdminIndex");
        }


        /// <summary>
        /// Activates the user with user id id.
        /// </summary>
        /// <param name="id">The id of the user to be activated.</param>
        /// <returns>A view to assure that the admin wants to activate the user.</returns>
        public ActionResult Activate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        /// <summary>
        /// Sets activate property from the database to true.
        /// </summary>
        /// <param name="id">The id of the user to be activated.</param>
        /// <returns>A view to the admin index page.</returns>
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateConfirmed(string id)
        {
            var user = db.Users.Find(id);
            user.Active = false;
            db.SaveChanges();
            return View("AdminIndex");
        }
    }
}