using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    /// <summary>
    /// Defines the Customer user class.
    /// </summary>
    public class CustomUser
    {
        /// <summary>
        /// Initialize a new instance of the CustomUser class.
        /// </summary>
        public CustomUser()
        {
        }

        /// <summary>
        /// Initialize a new instance of the CustomUser class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public CustomUser(string userId)
        {
            UserId = userId;
            Accounts = new List<Account>();
        }

        /// <summary>
        /// Gets or sets the CUstomer user idin the database.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomUserId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the accounts collection.
        /// </summary>
        public ICollection<Account> Accounts { get; set; }
    }
}