using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TransactionsHistoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult TransactionHistory()
        {
            TransactionHistoryViewModel model = new TransactionHistoryViewModel();
            model.Transactions = new List<TransactionInfoViewModel>();
            var user = db.Users.Where(m => m.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            if(user != null)
            {
                var transactions = new List<Transaction>();

                var accounts = db.Accounts.Where(m => m.UserId.Equals(user.Id)).ToList();

                foreach (var account in accounts)
                {
                    transactions.AddRange(db.Transactions.Where(m => m.ReceiverId == account.AccountId).ToList());
                    transactions.AddRange(db.Transactions.Where(m => m.SenderId == account.AccountId && account.UserId != user.Id).ToList());
                }

                
                foreach (var transaction in transactions)
                {
                    var transactionInfo = new TransactionInfoViewModel();
                    transactionInfo.Created = transaction.Created;
                    transactionInfo.Amount = transaction.Amount;
                    transactionInfo.Details = transaction.Details;
                    var sender = db.Accounts.Where(m => m.AccountId == transaction.SenderId).FirstOrDefault();
                    var receiver = db.Accounts.Where(m => m.AccountId == transaction.ReceiverId).FirstOrDefault();
                    transactionInfo.ReceiverIban = receiver.Iban;
                    transactionInfo.SenderIban = sender.Iban;
                    transactionInfo.SenderCurrency = sender.Currency;
                    transactionInfo.ReceiverCurrency = receiver.Currency;
                    if(sender.UserId == user.Id)
                    {
                        transactionInfo.Type = "out";
                    }
                    else
                    {
                        transactionInfo.Type = "in";
                    }

                    model.Transactions.Add(transactionInfo);

                }
                return View(model);

            }
            return RedirectToAction("InvalidUser", "Home");

        }
    }
}