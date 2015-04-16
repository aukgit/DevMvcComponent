
namespace DevMVCComponent.Mailers {
    public class GmailConfig : MailConfig {
        public GmailConfig():base() {
            HostSetup();            
        }
        public GmailConfig(string email, string password): base(email,password) {
            HostSetup();
        }
        public GmailConfig(string email, string password, string host, int port)
            : base(email, password) {
            HostSetup();
            this.Port = port;
            this.Host = host;

        }

        public override void HostSetup() {
            this.Host = "smtp.gmail.com";
            this.Port = 587;
            this._isHostConfigured = true;
        }
    }
}