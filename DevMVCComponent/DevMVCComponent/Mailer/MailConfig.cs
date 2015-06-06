#region using block

using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using DevMVCComponent.Enums;
using DevMVCComponent.Miscellaneous.Extensions;

#endregion

namespace DevMVCComponent.Mailer {
    /// <summary>
    /// Must configure this to your smtpclient
    /// </summary>
    public abstract class MailConfig : SmtpClient {
        private bool _async = true;
        private bool _isCredentialConfigured;
        private string _senderMail;
        private string _senderPassword;
        protected bool IsHostConfigured;

        /// <summary>
        ///     By default:
        ///     UseDefaultCredentials = false;
        ///     EnableSsl = true;
        ///     DeliveryMethod = SmtpDeliveryMethod.Network;
        ///     Timeout = 10000;
        /// </summary>
        protected MailConfig() {
            DefaultConfigarationSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public MailConfig(string email, string password) {
            DefaultConfigarationSetup();
            SenderEmail = email;
            SenderEmailPassword = password;
            _isCredentialConfigured = true;
        }

        /// <summary>
        ///     Setup credentials automatic.
        /// </summary>
        public string SenderEmail {
            get { return _senderMail; }
            set {
                _senderMail = value;
                //this.Mail.From = new MailAddress(_senderMail);
                SetupCredentials();
            }
        }

        /// <summary>
        ///     Setup credentials automatic.
        /// </summary>
        public string SenderEmailPassword {
            get { return _senderPassword; }
            set {
                _senderPassword = value;
                SetupCredentials();
            }
        }

        /// <summary>
        ///     Default = true
        /// </summary>
        public bool SendAsynchronousEmails {
            get { return _async; }
            set { _async = value; }
        }

        /// <summary>
        /// </summary>
        public bool IsConfigured {
            get {
                if (_isCredentialConfigured && IsHostConfigured) {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Change Credentials
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void ChangeCredentials(string email, string password) {
            SenderEmail = email;
            SenderEmailPassword = password;
            _isCredentialConfigured = true;
        }

        /// <summary>
        ///     Setup credentials automatic.
        /// </summary>
        private void SetupCredentials() {
            Credentials = new NetworkCredential(SenderEmail, SenderEmailPassword);
        }

        private void DefaultConfigarationSetup() {
            UseDefaultCredentials = false;
            EnableSsl = true;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            Timeout = 100000;
        }

        /// <param name="sender">Your mail address</param>
        /// <param name="receiver"></param>
        private MailMessage MailSetup(string sender, string receiver) {
            var mail = new MailMessage(sender, receiver);
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.IsBodyHtml = true;
            return mail;
        }

        private SmtpClient CopySmtpClient() {
            var mailSender = new SmtpClient();
            mailSender.UseDefaultCredentials = UseDefaultCredentials;
            mailSender.EnableSsl = EnableSsl;
            mailSender.DeliveryMethod = DeliveryMethod;
            mailSender.Timeout = Timeout;
            mailSender.Credentials = Credentials;
            mailSender.Port = Port;
            mailSender.Host = Host;
            return mailSender;
        }

        /// <summary>
        ///     Sends mail asynchronously.
        /// </summary>
        /// <param name="to">Comma to seperate multiple email addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type">Regular, CC, BCC</param>
        /// <param name="searchForCommas"></param>
        public void QuickSend(string to, string subject, string body, MailingType type = MailingType.RegularMail,
            bool searchForCommas = false) {
            if (IsConfigured && !to.IsEmpty()) {
                var t = new Thread(() => {
                    var mail = MailSetup(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, type, searchForCommas);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        var mailer = CopySmtpClient();
                        if (SendAsynchronousEmails) {
                            mailer.SendAsync(mail, "none");
                        } else {
                            mailer.Send(mail);
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Mail Sending Error: " + ex.Message);
                        throw ex;
                    }
                });
                t.Start();
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        /// <summary>
        ///     Quickly send an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="carbonCopyEmails"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type"></param>
        /// <exception cref="Exception"></exception>
        public void QuickSend(string to, string[] carbonCopyEmails, string subject, string body,
            MailingType type = MailingType.CarbonCopy) {
            if (IsConfigured && !to.IsEmpty()) {
                var t = new Thread(() => {
                    var mail = MailSetup(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, carbonCopyEmails, type);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        var mailer = CopySmtpClient();
                        if (SendAsynchronousEmails) {
                            mailer.SendAsync(mail, "none");
                        } else {
                            mailer.Send(mail);
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Mail Sending Error: " + ex.Message);
                        throw ex;
                    }
                });
                t.Start();
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        /// <summary>
        ///     Sends mail asynchronously.
        /// </summary>
        /// <param name="to">Comma to seperate multiple email addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="password"></param>
        /// <param name="type">Regular, CC, BCC</param>
        public void QuickSend(string to, string subject, string body, string from, string password,
            MailingType type = MailingType.RegularMail) {
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

        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="searchForCommas"></param>
        /// <exception cref="Exception"></exception>
        public void SendMailWithAttachments(string to, string subject, string body, string fileName,
            MailingType type = MailingType.RegularMail, bool searchForCommas = false) {
            if (IsConfigured && !to.IsEmpty()) {
                var t = new Thread(() => {
                    var mail = MailSetup(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, type, searchForCommas);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        var messageAttachment = new Attachment(fileName);
                        mail.Attachments.Add(messageAttachment);
                        var mailer = CopySmtpClient();
                        if (SendAsynchronousEmails) {
                            new Thread(() => mailer.Send(mail)).Start();
                        } else {
                            mailer.Send(mail);
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Mail Sending Error: " + ex.Message);
                        throw ex;
                    }
                });
                t.Start();
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
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
                var mailingAddresses = mailTo.Split(',').ToArray();
                foreach (var address in mailingAddresses) {
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

        private void MailingAddressAttach(ref MailMessage mail, string mailTo, string[] mailCc, MailingType type) {
            mail.To.Clear();
            mail.To.Add(new MailAddress(mailTo));

            foreach (var address in mailCc) {
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
        ///     Specific host setup. Must ensure the boolean isHostConfigured = true.
        /// </summary>
        /// <param name="host">for example gmail: smtp.gmail.com</param>
        /// <param name="port">for example gmail enablessl port: 587</param>
        public abstract void HostSetup();
    }
}