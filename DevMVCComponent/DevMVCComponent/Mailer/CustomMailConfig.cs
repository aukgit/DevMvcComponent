namespace DevMVCComponent.Mailer {
    public class CustomMailConfig : MailConfig {
        /// <summary>
        ///     SSL true
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public CustomMailConfig(string email, string password, string host, int port)
            : base(email, password) {
            HostSetup();
            Port = port;
            Host = host;
        }

        public CustomMailConfig(string email, string password, string host, int port, bool isSsl)
            : base(email, password) {
            HostSetup();
            Port = port;
            Host = host;
            EnableSsl = isSsl;
        }

        public override void HostSetup() {
            Host = "smtp.gmail.com";
            Port = 587;
            IsHostConfigured = true;
        }
    }
}