#region using block

using System.Reflection;
using DevMvcComponent.Error;
using DevMvcComponent.Mail;
using DevMvcComponent.Processor;

#endregion

namespace DevMvcComponent {
    /// <summary>
    ///     It's direct singleton pattern class in C#.
    ///     Must setup this starter class and Config class.
    ///     Note: In java you need to follow traditional singleton pattern ,
    ///     however in C# you don't require that Instance variable because it already has static keyword.
    /// </summary>
    public static class Mvc {
        /// <summary>
        ///     Handles all kinds of errors and then finally sends an email to the client.
        /// </summary>
        public static Handler Error;

        /// <summary>
        ///     Cookies are client specific.
        /// </summary>
        public static CookieProcessor Cookies;

        /// <summary>
        ///     Caches are application specific.
        /// </summary>
        public static CacheProcessor Caches;

        /// <summary>
        ///     Must re-setup with appropriate developer credentials.
        /// </summary>
        public static MailServer Mailer;

        /// <summary>
        ///     Setup the component plugin.
        ///     Please make sure that your executing directory is writable if not then please add a folder "DataCache"
        /// </summary>
        /// <param name="applicationName">Name of your application or software.</param>
        /// <param name="developerEmail">Comma separated developers emails</param>
        /// <param name="assembly">Usually set to "System.Reflection.Assembly.GetExecutingAssembly()"</param>
        /// <param name="mailer">
        ///     Get your own custom mailer or GmailConfig or setup CustomConfig.
        ///     new DevMVCComponent.Mailers.GmailConfig("senderEmail","Password")
        /// </param>
        public static void Setup(string applicationName, string developerEmail, Assembly assembly, MailServer mailer) {
            Config.ApplicationName = applicationName;
            Config.DeveloperEmails = developerEmail.Split(',');
            //Configure this with add a sender email.
            Mailer = mailer; //
            InitalizeDefaults(assembly);
        }

        private static void InitalizeDefaults(Assembly assembly) {
            Config.Assembly = assembly;
            Error = new Handler();
            Cookies = new CookieProcessor();
            Caches = new CacheProcessor();
            Config.GetApplicationNameHtml(true);
        }

        /// <summary>
        ///     Setup the component plugin.
        ///     Please make sure that your executing directory is writable if not then please add a folder "DataCache"
        ///     ** Warning : By this instantiation you can't handle exception by email or send quick emails through
        ///     Starter.Mailer.SendQuick(..) **
        /// </summary>
        /// <param name="assembly">Usually set to "System.Reflection.Assembly.GetExecutingAssembly()"</param>
        public static void Setup(Assembly assembly) {
            Config.ApplicationName = "";
            Config.DeveloperEmails = null;
            //Configure this with add a sender email.
            InitalizeDefaults(assembly);
        }

        /// <summary>
        ///     Setup the component plugin.
        ///     Please make sure that your executing directory is writable if not then please add a folder "DataCache"
        /// </summary>
        /// <param name="applicationName">Name of your application or software.</param>
        /// <param name="developerEmail">Developer email</param>
        /// <param name="assembly">Usually set to "System.Reflection.Assembly.GetExecutingAssembly()"</param>
        /// <param name="senderEmail">Smtp sender email address.</param>
        /// <param name="senderDisplayName">Email address's display name.</param>
        /// <param name="senderPassword">Smtp sender password.</param>
        /// <param name="hostName">host name, i.e. smtp.gmail.com</param>
        /// <param name="senderPort">port number</param>
        /// <param name="isSsl"></param>
        public static void Setup(
            string applicationName, 
            string developerEmail, 
            Assembly assembly, 
            string senderEmail, 
            string senderDisplayName,
            string senderPassword, 
            string hostName, 
            int senderPort, 
            bool isSsl) {
            //Configure this with add a sender email.
            Mailer = new CustomMailServer(senderDisplayName,senderEmail, senderPassword, hostName, senderPort, isSsl); //
            Setup(applicationName, developerEmail, assembly, Mailer);
        }
    }
}