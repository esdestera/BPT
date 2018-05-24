using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class UserViewModel
    {
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Name identifier")]
        public string NameIdentifier { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "User id")]
        public string UserId { get; set; }
    }
}