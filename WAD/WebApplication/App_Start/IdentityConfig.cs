using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication.Models;

namespace WebApplication
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
            return Task.Factory.StartNew(() =>
            {
                SendEmail(message);
            });
        }

        void SendEmail(IdentityMessage message)
        {
            #region formatter
            string text = message.Body;
            string html =  message.Body;
            #endregion

            MailMessage msg = new MailMessage();
            var from = ConfigurationManager.AppSettings["Email"];
            var pass = ConfigurationManager.AppSettings["Password"];
            var to = string.Empty;
            if (message.Destination.Equals("admin@admin.com"))
            {
                to = from;
            }
            else
            {
                to = message.Destination;
            }
            
            msg.From = new MailAddress(from);
            msg.To.Add(new MailAddress(to));
            msg.Subject = message.Subject;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
           // msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(from, pass);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            try
            {

                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
       


public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(RoleStore<IdentityRole> rolesStore)
            :base(rolesStore)
        {

        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            
            var manager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            if (!manager.RoleExists("Admin"))
            {
                manager.Create(new IdentityRole("Admin"));
            }
            return manager;
        }
    }
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            var adminUser = manager.FindByName("admin@admin.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = "admin@admin.com", NameIdentifier= "admin@admin.com", Email = "admin@admin.com", EmailConfirmed = true };       
               manager.Create(adminUser, "@dm!n!str@t0r");
                            
            }
            if (!manager.IsInRole(adminUser.Id, "Admin"))
            {
                manager.AddToRole(adminUser.Id, "Admin");
            }
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        internal Task SendEmailFromAdmin(string id, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
