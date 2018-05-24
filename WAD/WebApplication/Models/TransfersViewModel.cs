using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TransfersViewModel
    {
        [Required]
        [Display (Name = "To account")]
        public string DestIban { get; set; }

        [Required]
        [Display(Name = "From account")]
        public string SenderIban { get; set; }

        [Display(Name = "Receiver name")]
        public string DestName { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        [Display(Name = "Save this transfer to my transfers!")]
        public bool SaveToMyTransfers { get; set; }

        [Display(Name = "Transaction name")]
        public string TransactionName { get; set; }

    }
}