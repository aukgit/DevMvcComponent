using System.Net.Mail;

namespace DevMvcComponent.Mail {
    /// <summary>
    ///     Mail sending wrapper.
    /// </summary>
    public class MailSendingWrapper {
        /// <summary>
        /// </summary>
        public MailSendingWrapper() {}

        /// <summary>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="message"></param>
        public MailSendingWrapper(SmtpClient server, MailMessage message) {
            MailServer = server;
            MailMessage = message;
        }

        /// <summary>
        ///     Mail message, contains mail body , subject and etc.
        /// </summary>
        public MailMessage MailMessage { get; set; }

        /// <summary>
        ///     A type of mail sender or mail server.
        ///     It will send the email.
        ///     It also contains the credentials.
        /// </summary>
        public SmtpClient MailServer { get; set; }
    }
}