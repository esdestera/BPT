using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class TransactionsHistoryController : Controller
    {
        public ActionResult TransactionHistory()
        {
            return View();
        }
    }
}