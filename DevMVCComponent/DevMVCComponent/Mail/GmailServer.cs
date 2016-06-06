namespace DevMvcComponent.Mail {
    /// <summary>
    ///     Create a new gmail server to send emails from smtp.gmail.com or new host.
    /// </summary>
    public class GmailServer : MailServer {
        /// <summary>
        ///     Host = "smtp.gmail.com";
        ///     Port = 587;
        /// </summary>
        public GmailServer() {
            HostSetup();
        }

        /// <summary>
        ///     Get default configured gmail server :
        ///     Host = "smtp.gmail.com";
        ///     Port = 587;
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        public GmailServer(string emailAddress, string password)
            : base(emailAddress, password) {
            HostSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public GmailServer(string emailAddress, string password, string host, int port)
            : base(emailAddress, password) {
            HostSetup();
            Port = port;
            Host = host;
        }

        public override void HostSetup() {
            Host = "smtp.gmail.com";
            Port = 587;
            IsHostConfigured = true;
        }
    }
}