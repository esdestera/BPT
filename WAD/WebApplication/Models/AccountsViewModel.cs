using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class AccountsViewModel
    {
        public ICollection<Account> Accounts { get; set; }

        public CreateAccountViewModel CreateAccountVM { get; set; }
    }
}