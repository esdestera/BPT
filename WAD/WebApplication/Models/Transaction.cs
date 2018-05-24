using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    /// <summary>
    /// Defines the Transaction class.
    /// </summary>
    public class Transaction
    {

        public Transaction()
        {
           
        }

        public Transaction(double amount, string type, int sender, int receiver, bool saveToMyTransfers)
        {
            Amount = amount;
            Type = type;
            SenderId = sender;
            ReceiverId = receiver;
            SaveToMyTransfers = saveToMyTransfers;
        }

        public Transaction(double amount, string type, int sender, int receiver, bool saveToMyTransfers, string details, string transactionName)
        {
            Amount = amount;
            Type = type;
            SenderId = sender;
            ReceiverId = receiver;
            SaveToMyTransfers = saveToMyTransfers;
            Details = details;
            TransactionName = transactionName;
        }
        /// <summary>
        /// Gets the transaction id in the database.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        
        /// <summary>
        /// Gets or sets the amount of transaction.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the type of transaction. 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// gets or sets the save to my transfers property.
        /// </summary>
        public bool SaveToMyTransfers { get; set; }

        /// <summary>
        /// gets or sets the save to my transfers property.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// gets or sets the transaction name.
        /// </summary>
        public string TransactionName { get; set; }

        public DateTime Created { get; set; }
    }

}