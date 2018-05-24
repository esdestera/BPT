using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TransfersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult ToMyAccounts()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult ToOthers()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyTransfers()
        {
            MyTransfersViewModel model = new MyTransfersViewModel();
            model.Transactions = new List<TransactionInfoViewModel>();
            var currentUser = db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault();
            if(currentUser != null)
            {
                var accounts = db.Accounts.Where(m => m.UserId == currentUser.Id).ToList();
                var trans = new List<Transaction>();
                var transactions = new List<Transaction>();
                foreach (var account in accounts)
                {
                    trans = db.Transactions.Where(m => m.SenderId == account.AccountId).ToList();
                    if (trans != null)
                    {
                        foreach (var transaction in trans)
                        {
                            if (transaction.SaveToMyTransfers == true)
                            {
                                var transactionInfo = new TransactionInfoViewModel();
                                transactionInfo.TransactionName = transaction.TransactionName;
                                transactionInfo.SenderIban = account.Iban;
                                var receiver = db.Accounts.Where(m => m.AccountId == transaction.ReceiverId).FirstOrDefault();
                                transactionInfo.ReceiverIban = receiver.Iban;
                                transactionInfo.ReceiverCurrency = receiver.Currency;
                                transactionInfo.SenderCurrency = account.Currency;
                                transactionInfo.TransactionId = transaction.TransactionId;
                                model.Transactions.Add(transactionInfo);
                            }
                        }
                    }

                }

                return View(model);
            }

            return RedirectToAction("InvalidUser", "Account");
            
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContinueTransferToMe(TransfersViewModel model)
        {
            var account = db.Accounts.Where(m => m.Iban.Equals(model.DestIban)).FirstOrDefault();
            if(account != null)
            {
                var destUser = db.Users.Where(m => m.Id.Equals(account.UserId)).FirstOrDefault();
                model.DestName = destUser.NameIdentifier;
                return View("TransferToMyAccountConfirmation", model); 
            }
            else
            {
                return RedirectToAction("ToMyAccounts", "Transfers");
            }
        }


        [HttpGet]
        public ActionResult FilledTransfer(TransactionInfoViewModel model)
        {
            TransfersViewModel filled = new TransfersViewModel();
            var transaction = db.Transactions.Where(m => m.TransactionId == model.TransactionId).FirstOrDefault();
           if (transaction != null)
            {
                var sender = db.Accounts.Where(m => m.AccountId.Equals(transaction.SenderId)).FirstOrDefault();
                var receiver = db.Accounts.Where(m => m.AccountId.Equals(transaction.ReceiverId)).FirstOrDefault();
                var destUser = db.Users.Where(m => m.Id.Equals(receiver.UserId)).FirstOrDefault();
                filled.DestIban = receiver.Iban;
                filled.SenderIban = sender.Iban;
                filled.Details = transaction.Details;
                filled.DestName = destUser.NameIdentifier;
                filled.Amount = 0;
                return View("FilledTransfer", filled);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContinueTransferToOthers(TransfersViewModel model)
        {
            var account = db.Accounts.Where(m => m.Iban.Equals(model.DestIban)).FirstOrDefault();
            if (account != null)
            {
                var destUser = db.Users.Where(m => m.Id.Equals(account.UserId)).FirstOrDefault();
                if(destUser != null)
                {
                    model.DestName = destUser.NameIdentifier;
                    return View("TransferToOthersConfirmation", model);
                }
                else
                {
                    return View("InvalidUser", "Home");
                }
            }
            else
            {
                return RedirectToAction("InvalidDestAccount", "Transfers");
            }
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransfersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountDest = db.Accounts.Where(m => m.Iban.Equals(model.DestIban)).FirstOrDefault();
                var accountSrc = db.Accounts.Where(m => m.Iban.Equals(model.SenderIban)).FirstOrDefault();
                if(accountDest != null && accountSrc != null)
                {
                    if(accountSrc.Balance > (double)model.Amount)
                    {
                        if(accountSrc.CardLimit < (double)(accountSrc.Balance - (double)model.Amount))
                        {
                            var transaction = new Transaction((double)model.Amount, "out", accountSrc.AccountId, accountDest.AccountId, model.SaveToMyTransfers, model.Details, model.TransactionName);
                            accountSrc.Transactions.Add(transaction);
                            accountDest.Balance += ConvertToAccountCurrency(accountSrc.Currency, accountDest.Currency, (double)model.Amount);
                            accountSrc.Balance -= (double)model.Amount;
                            if (!string.IsNullOrEmpty(model.TransactionName))
                            {
                                model.SaveToMyTransfers = true;
                                transaction.SaveToMyTransfers = true;
                            }
                            transaction.Created = DateTime.Now;
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                            return RedirectToAction("Home", "Home");
                        }
                        else
                        {
                            return View("Error", model);
                        }
                    }
                    else
                    {
                        return View("Error", model);
                    }
                }
                else
                {
                    return View("Error");
                }
               
            }

            return View("Error");
        }

        private double ConvertToAccountCurrency(string senderCurrency, string receiverCurrency, double amount)
        {
            var result = 0.0;
            if (senderCurrency.Equals(receiverCurrency))
            {
                return (double)amount;
            }

            switch (senderCurrency)
            {
                case "EUR":
                    switch (receiverCurrency)
                    {
                        case "USD":
                            result = Double.Parse(ConfigurationManager.AppSettings["EURTOUSD"]);
                            return amount * result;
                        case "RON":
                            result = Double.Parse(ConfigurationManager.AppSettings["EURTOLEI"]);
                            return amount * result;
                    }
                    break;
                case "USD":
                    switch (receiverCurrency)
                    {
                        case "EUR":
                            result = Double.Parse(ConfigurationManager.AppSettings["USDTOEUR"]);
                            return amount * result;
                        case "RON":
                            result = Double.Parse(ConfigurationManager.AppSettings["USDTOLEI"]);
                            return amount * result;
                    }
                    break;
                case "RON":
                    switch (receiverCurrency)
                    {
                        case "EUR":
                            result = Double.Parse(ConfigurationManager.AppSettings["LEITOEUR"]);
                            return amount * result;
                        case "USD":
                            result = Double.Parse(ConfigurationManager.AppSettings["LEITOUSD"]);
                            return amount * result;
                    }
                    break;

            }

            return result;
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,Amount,Type")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}