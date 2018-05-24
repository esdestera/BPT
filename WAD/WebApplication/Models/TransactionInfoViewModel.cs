using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class TransactionInfoViewModel
    {
        /// <summary>
        /// Gets or sets the amount of transaction.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the amount of transaction.
        /// </summary>
        public int TransactionId { get; set; }
        /// <summary>
        /// Gets or sets the type of transaction. 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        public string SenderIban { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        public string ReceiverIban { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        public string SenderCurrency { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        public string ReceiverCurrency { get; set; }


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