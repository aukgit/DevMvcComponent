
namespace DevMVCComponent.Mailers {
    public class CustomMailConfig : MailConfig {
        /// <summary>
        /// SSL true
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public CustomMailConfig(string email, string password, string host, int port)
            : base(email, password) {
            HostSetup();
            base.Port = port;
            base.Host = host;

        }
        public CustomMailConfig(string email, string password, string host, int port, bool isSSL)
            : base(email, password) {
            HostSetup();
            base.Port = port;
            base.Host = host;
            base.EnableSsl = isSSL;

        }

        public override void HostSetup() {
            base.Host = "smtp.gmail.com";
            base.Port = 587;
            base._isHostConfigured = true;
        }
    }
}