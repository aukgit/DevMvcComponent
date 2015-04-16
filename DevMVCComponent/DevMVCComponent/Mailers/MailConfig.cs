using DevMVCComponent.Enums;
using DevMVCComponent.Miscellaneous.Extensions;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace DevMVCComponent.Mailers {
    public abstract class MailConfig : SmtpClient {

        /// <summary>
        /// By default:
        /// UseDefaultCredentials = false;
        /// EnableSsl = true;
        /// DeliveryMethod = SmtpDeliveryMethod.Network;
        /// Timeout = 10000;
        /// </summary>
        public MailConfig() {
            DefaultConfigarationSetup();
        }

        public MailConfig(string email, string password) {
            DefaultConfigarationSetup();
            SenderEmail = email;
            SenderEmailPassword = password;
            _isCredentialConfigured = true;
        }

        private string _senderMail;
        private string _senderPassword;
        private bool _isCredentialConfigured;
        protected bool _isHostConfigured;
        private bool _async = true;

        /// <summary>
        /// Setup credentials automatic.
        /// </summary>
        public string SenderEmail {
            get {
                return _senderMail;
            }
            set {
                _senderMail = value;
                //this.Mail.From = new MailAddress(_senderMail);
                SetupCredentials();
            }
        }


        /// <summary>
        /// Setup credentials automatic.
        /// </summary>
        public string SenderEmailPassword {
            get {
                return _senderPassword;
            }
            set {
                _senderPassword = value;
                SetupCredentials();
            }
        }

        /// <summary>
        /// Default = true
        /// </summary>
        public bool SendAsynchronousEmails { get { return _async; } set { _async = value; } }

        /// <summary>
        /// Change Credentials
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void ChangeCredentials(string email, string password) {
            SenderEmail = email;
            SenderEmailPassword = password;
            _isCredentialConfigured = true;
        }

        public bool IsConfigured {
            get {
                if (_isCredentialConfigured && _isHostConfigured) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        /// <summary>
        /// Setup credentials automatic.
        /// </summary>
        private void SetupCredentials() {
            this.Credentials = new System.Net.NetworkCredential(SenderEmail, SenderEmailPassword);
        }

        private void DefaultConfigarationSetup() {
            this.UseDefaultCredentials = false;
            this.EnableSsl = true;
            this.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.Timeout = 100000;

        }

        /// <param name="senderMailAddress">Your mail address</param>
        private MailMessage MailSetup(string sender, string receiver) {
            MailMessage mail = new MailMessage(sender, receiver);
            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.IsBodyHtml = true;
            return mail;
        }

        private SmtpClient copySmtpClient() {
            var mailSender = new SmtpClient();
            mailSender.UseDefaultCredentials = this.UseDefaultCredentials;
            mailSender.EnableSsl = this.EnableSsl;
            mailSender.DeliveryMethod = this.DeliveryMethod;
            mailSender.Timeout = this.Timeout;
            mailSender.Credentials = this.Credentials;
            mailSender.Port = this.Port;
            mailSender.Host = this.Host;
            return mailSender;
        }


        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="to">Comma to seperate multiple email addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type">Regular, CC, BCC</param>
        public void QuickSend(string to, string subject, string body, MailingType type = MailingType.RegularMail, bool SearchForCommas = false) {
            if (IsConfigured && !to.IsEmpty()) {
                Thread t = new Thread(() => {
                var mail = MailSetup(SenderEmail, to);
                MailingAddressAttach(ref mail, to, type, SearchForCommas);
                mail.Subject = subject;
                mail.Body = body;
                try {
                    var mailer = copySmtpClient();
                    if (SendAsynchronousEmails) {
                        mailer.SendAsync(mail, "none");
                    } else {
                        mailer.Send(mail);
                    }
            
                } catch (Exception ex) {
                    Console.WriteLine("Mail Sending Error: " + ex.Message.ToString());
                    throw ex;
                }
                });
                t.Start();
            } else {
                throw new Exception("Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        public void QuickSend(string to, string[] CarbonCopyEmails, string subject, string body, MailingType type = MailingType.CarbonCopy) {
            if (IsConfigured && !to.IsEmpty()) {
                Thread t = new Thread(() => {
                    var mail = MailSetup(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, CarbonCopyEmails, type);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        var mailer = copySmtpClient();
                        if (SendAsynchronousEmails) {
                            mailer.SendAsync(mail, "none");
                        } else {
                            mailer.Send(mail);
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Mail Sending Error: " + ex.Message.ToString());
                        throw ex;
                    }
                });
                t.Start();
            } else {
                throw new Exception("Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="to">Comma to seperate multiple email addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="password"></param>
        /// <param name="type">Regular, CC, BCC</param>
        public void QuickSend(string to, string subject, string body, string from, string password, MailingType type = MailingType.RegularMail) {
            var emailBack = SenderEmail;
            var passwordBack = SenderEmailPassword;

            SenderEmail = from;
            SenderEmailPassword = password;
            SetupCredentials();
            QuickSend(to, subject, body);

            SenderEmail = emailBack;
            SenderEmailPassword = passwordBack;
            SetupCredentials();

        }

        public void SendMailWithAttachments(string to, string subject, string body, string fileName, MailingType type = MailingType.RegularMail, bool SearchForCommas =false) {
            if (IsConfigured && !to.IsEmpty()) {
                Thread t = new Thread(() => {
                    var mail = MailSetup(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, type, SearchForCommas);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        Attachment messageAttachment = new Attachment(fileName);
                        mail.Attachments.Add(messageAttachment);
                        var mailer = copySmtpClient();
                        if (SendAsynchronousEmails) {
                            new Thread(() => mailer.Send(mail)).Start();
                        } else {
                            mailer.Send(mail);
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Mail Sending Error: " + ex.Message.ToString());
                        throw ex;
                    }
                });
                t.Start();
            } else {
                throw new Exception("Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }
        private void MailingAddressAttach(ref MailMessage mail, string mailTo, MailingType type) {
            mail.To.Clear();

            if (type == MailingType.RegularMail) {
                mail.To.Add(new MailAddress(mailTo));
            } else if (type == MailingType.CarbonCopy) {
                mail.CC.Add(new MailAddress(mailTo));
            } else {
                mail.Bcc.Add(new MailAddress(mailTo));
            }

        }
        private void MailingAddressAttach(ref MailMessage mail, string mailTo, MailingType type, bool searchForComma) {
            mail.To.Clear();
            if (searchForComma && mailTo.IndexOf(",") > -1) {
                string[] mailingAddresses = mailTo.Split(',').ToArray();
                foreach (string address in mailingAddresses) {
                    if (type == MailingType.RegularMail) {
                        mail.To.Add(new MailAddress(address));
                    } else if (type == MailingType.CarbonCopy) {
                        mail.CC.Add(new MailAddress(address));
                    } else {
                        mail.Bcc.Add(new MailAddress(address));
                    }
                }
            } else {
                if (type == MailingType.RegularMail) {
                    mail.To.Add(new MailAddress(mailTo));
                } else if (type == MailingType.CarbonCopy) {
                    mail.CC.Add(new MailAddress(mailTo));
                } else {
                    mail.Bcc.Add(new MailAddress(mailTo));
                }
            }
        }

        private void MailingAddressAttach(ref MailMessage mail, string mailTo, string[] mailCC, MailingType type) {
            mail.To.Clear();
            mail.To.Add(new MailAddress(mailTo));

            foreach (string address in mailCC) {
                if (type == MailingType.RegularMail) {
                    mail.To.Add(new MailAddress(address));
                } else if (type == MailingType.CarbonCopy) {
                    mail.CC.Add(new MailAddress(address));
                } else {
                    mail.Bcc.Add(new MailAddress(address));
                }
            }
        }



        /// <summary>
        /// Specific host setup. Must ensure the boolean isHostConfigured = true.
        /// </summary>
        /// <param name="host">for example gmail: smtp.gmail.com</param>
        /// <param name="port">for example gmail enablessl port: 587</param>
        public abstract void HostSetup();




    }
}