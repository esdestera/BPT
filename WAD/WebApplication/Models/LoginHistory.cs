using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    /// <summary>
    /// Defines the login history class.
    /// </summary>
    public class LoginHistory
    {
        /// <summary>
        /// Initialize a new instance of the Login history class.
        /// </summary>
        public LoginHistory()
        {

        }

        /// <summary>
        /// Initialize a new instance of the login history. class.
        /// </summary>
        /// <param name="loginTime"></param>
        /// <param name="ip"></param>
        /// <param name="userId"></param>
        public LoginHistory(DateTime loginTime, string ip, string userId)
        {
            LoginTime = loginTime;
            Ip = ip;
            UserId = userId;
        }

        /// <summary>
        /// Gets the login history id in the database.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoginHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the login time.
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// Gets or sets the logout time.
        /// </summary>
        public DateTime? LogoutTime { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [StringLength(128)]
        public string UserId { get; set; }
    }
}