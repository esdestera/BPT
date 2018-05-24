using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class MyTransfersViewModel
    {

        public ICollection<TransactionInfoViewModel> Transactions { get; set; }
    }
}