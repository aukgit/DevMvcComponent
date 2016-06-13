namespace DevMvcComponent.Mail {
    /// <summary>
    ///     Create a new gmail server to send emails from smtp.gmail.com or new host.
    /// </summary>
    public class GmailServer : MailServer {
        /// <summary>
        ///     Host = "smtp.gmail.com";
        ///     Port = 587;
        /// </summary>
        /// <param name="displayName">User's display name.</param>
        public GmailServer(string displayName) : base(displayName) {
            HostSetup();
        }

        /// <summary>
        ///     Get default configured gmail server :
        ///     Host = "smtp.gmail.com";
        ///     Port = 587;
        /// </summary>
        /// <param name="displayName">User's display name.</param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        public GmailServer(string displayName, string emailAddress, string password)
            : base(displayName,emailAddress, password) {
            HostSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="displayName">User's display name.</param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public GmailServer(string displayName, string emailAddress, string password, string host, int port)
            : base(displayName, emailAddress, password) {
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