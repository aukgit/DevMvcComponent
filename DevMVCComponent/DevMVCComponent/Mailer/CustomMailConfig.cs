namespace DevMVCComponent.Mailer {
    /// <summary>
    /// Any mail server generic config methods.
    /// There is no need to inherit MailConfig all the time.
    /// Create this class with contructor parameters and
    /// then initialize it in the starter.
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="isSsl"></param>
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