using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class TransactionsHistoryController : Controller
    {
        public ActionResult TransactionHistory(TransactionHistoryViewModel model)
        {
            return View();
        }
    }
}