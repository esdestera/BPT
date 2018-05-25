using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class BillViewModel
    {
        [Display(Name = "Name of the bill")]
        public string Name { get; set; }

        [Display(Name = "IBAN")]
        public string Iban { get; set; }

        [Display(Name = "From")]
        public string SenderIban { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        [Display(Name = "Save this transfer to my transfers!")]
        public bool SaveToMyTransfers { get; set; }
    }
}