using System;

namespace WebApplication.Controllers
{
    public class TransactionHistoryViewModel
    {

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

      
        public DateTime Created { get; set; }
    }
}
