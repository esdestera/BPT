using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class BalanceViewModel
    {
        [Display(Name = "User id")]
        public string UserId { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Balance")]
        public double Balance { get; set; }

        public string Iban { get; set; }
    }
}