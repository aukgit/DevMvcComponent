namespace DevMvcComponent.Mail {
    /// <summary>
    /// </summary>
    public class GmailServer : MailServer {
        /// <summary>
        /// </summary>
        public GmailServer() {
            HostSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public GmailServer(string email, string password) : base(email, password) {
            HostSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public GmailServer(string email, string password, string host, int port)
            : base(email, password) {
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