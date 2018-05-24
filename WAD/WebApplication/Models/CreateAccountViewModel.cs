using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class CreateAccountViewModel
    {
        public string Iban { get; set; }
        public string Currency { get; set; }
        public double CardLimit { get; set; }

        public string UserId { get; set; }
    }
}