using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    /// <summary>
    /// Defines the account model.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Initializes a new instance of the Account class.
        /// </summary>
        public Account()
        {
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Initializes a new instance of the Account class.
        /// 
        /// </summary>
        /// <param name="iban">The account's iban.</param>
        /// <param name="currency">The currency of the account.</param>
        public Account(string iban, string currency)
        {
            Iban = iban;
            Balance = 0;
            CardLimit = 0;
            Currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the Account class.
        /// </summary>
        /// <param name="iban">The account iban.</param>
        /// <param name="balance">The account balance.</param>
        /// <param name="cardLimit">The account limit.</param>
        /// <param name="currency">The account currency.</param>
        public Account(string iban, double balance, double cardLimit, string currency)
        {
            Iban = iban;
            Balance = balance;
            CardLimit = cardLimit;
            Currency = currency;
            Transactions = new List<Transaction>();
        }
        /// <summary>
        /// The account id in the databse.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        
        /// <summary>
        /// Gets or sets the iban propery.
        /// </summary>
        public string Iban { get; set; }

        /// <summary>
        /// Gets or sets the balance property.
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Gets or sets the card limit.
        /// </summary>
        public double CardLimit { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the transaction collection.
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        
    }
}