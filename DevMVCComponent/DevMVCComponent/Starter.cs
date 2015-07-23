#region using block

using System.Reflection;
using DevMvcComponent.Error;
using DevMvcComponent.Mailer;
using DevMvcComponent.Processor;

#endregion

namespace DevMvcComponent {
    /// <summary>
    ///     Must setup this starter class and Config class.
    /// </summary>
    public static class Starter {
        /// <summary>
        ///     Handles all kinds of errors and then finally sends an email to the client.
        /// </summary>
        public static Handler Error;

        /// <summary>
        /// </summary>
        public static CookieProcessor Cookies;

        /// <summary>
        /// </summary>
        public static CacheProcessor Caches;

        /// <summary>
        ///     Must re-setup with appropriate developer credentials.
        /// </summary>
        public static MailConfig Mailer;

        /// <summary>
        ///     Setup the component plugin.
        ///     Please make sure that your executing directory is writable if not then please add a folder "DataCache"
        /// </summary>
        /// <param name="applicationName">Name of your application or software.</param>
        /// <param name="developerEmail">Developer email</param>
        /// <param name="assembly">Usually set to "System.Reflection.Assembly.GetExecutingAssembly()"</param>
        /// <param name="mailer">
        ///     Get your own custom mailer or GmailConfig or setup CustomConfig.
        ///     new DevMVCComponent.Mailers.GmailConfig("senderEmail","Password")
        ///     By default email sender is async , to make it sync you can use property on MailConfig.SendAsynchronousEmails = false;
        /// </param>
        public static void Setup(string applicationName, string developerEmail, Assembly assembly, MailConfig mailer) {
            Config.ApplicationName = applicationName;
            Config.DeveloperEmail = developerEmail;
            Config.Assembly = assembly;
            //Configure this with add a sender email.
            Mailer = mailer; //
            InitalizeDefaults();
        }

        private static void InitalizeDefaults() {
            Error = new Handler();
            Cookies = new CookieProcessor();
            Caches = new CacheProcessor();
        }

        /// <summary>
        ///     Setup the component plugin.
        ///     Please make sure that your executing directory is writable if not then please add a folder "DataCache"
        /// </summary>
        /// <param name="applicationName">Name of your application or software.</param>
        /// <param name="developerEmail">Developer email</param>
        /// <param name="assembly">Usually set to "System.Reflection.Assembly.GetExecutingAssembly()"</param>
        /// <param name="senderEmail">Smtp sender email address.</param>
        /// <param name="senderPassword">Smtp sender password.</param>
        /// <param name="hostName">host name, i.e. smtp.gmail.com</param>
        /// <param name="senderPort">port number</param>
        /// <param name="isSSL"></param>
        public static void Setup(string applicationName, string developerEmail, Assembly assembly, string senderEmail,
            string senderPassword, string hostName, int senderPort, bool isSSL) {
           
            //Configure this with add a sender email.
            Mailer = new CustomMailConfig(senderEmail, senderPassword, hostName, senderPort, isSSL); //

            Setup(applicationName, developerEmail, assembly, Mailer);
        }
    }
}