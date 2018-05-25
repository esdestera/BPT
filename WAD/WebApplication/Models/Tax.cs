using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Tax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string TaxIban { get; set; }

        public Tax()
        {
        }

        public Tax(string name, string iban)
        {
            this.Name = name;
            this.TaxIban = iban;
        }
    }
}