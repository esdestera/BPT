using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ProfileViewModel
    {

        [Display(Name = "Email address")] public string EmailAddress { get; set; }

        [Display(Name = "First name")] public string FirstName { get; set; }

        [Display(Name = "Last name")] public string LastName { get; set; }

        [Display(Name = "Password")] public string Password { get; set; }

        [Display(Name = "Username")] public string UserName { get; set; }

        [Display(Name = "PhoneNumber")] public string PhoneNumber { get; set; }
    }

    public class NewAccountViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "IBAN required")]
        [Display(Name = "IBAN")] public string IBAN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency required")]
        [Display(Name = "Currency")] public string Currency { get; set; }

        [Display(Name = "Card limit")] public double CardLimit { get; set; }

    }
}