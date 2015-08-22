#region using block

using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using DevMvcComponent.Enums;
using DevMvcComponent.Extensions;

#endregion

namespace DevMvcComponent.Mail {
    /// <summary>
    ///     Must configure this to your smtpclient
    /// </summary>
    public abstract class MailServer : SmtpClient {
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
        protected MailServer() {
            DefaultConfigarationSetup();
        }

        /// <summary>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public MailServer(string email, string password) {
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

        /// <summary>
        /// Get a new mail message.
        /// </summary>
        /// <param name="sender">Your mail address</param>
        /// <param name="receiver"></param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">email body</param>
        /// <param name="isHtmlBody">By default : true</param>
        /// <param name="bodyEncoding">By default : Encoding.UTF8</param>
        /// <returns></returns>
        public MailMessage GetNewMailMessage(string sender, string receiver, string subject = "", string body = "", bool isHtmlBody = true, Encoding bodyEncoding = null) {
            bodyEncoding = bodyEncoding ?? Encoding.UTF8;
            var mail = new MailMessage(sender, receiver) {
                BodyEncoding = bodyEncoding,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = isHtmlBody,
                Body = body
            };
            return mail;
        }

        /// <summary>
        /// Copy current smtp mailer to an new instance.
        /// </summary>
        /// <returns></returns>
        public SmtpClient CloneSmtpClient() {
            return CloneSmtpClient(this);
        }
        /// <summary>
        /// Copy any smtp mailer to an new instance.
        /// </summary>
        /// <returns></returns>
        public SmtpClient CloneSmtpClient(SmtpClient smpt) {
            var mailSender = new SmtpClient();
            mailSender.UseDefaultCredentials = smpt.UseDefaultCredentials;
            mailSender.EnableSsl = smpt.EnableSsl;
            mailSender.DeliveryMethod = smpt.DeliveryMethod;
            mailSender.Timeout = smpt.Timeout;
            mailSender.Credentials = smpt.Credentials;
            mailSender.Port = smpt.Port;
            mailSender.Host = smpt.Host;
            return mailSender;
        }

        /// <summary>
        ///     Send quick mail synchronously or  asynchronously.
        /// </summary>
        /// <param name="to">Comma to separate multiple email addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type">Regular, CC, BCC</param>
        /// <param name="searchForCommas"></param>
        public void QuickSend(string to, string subject, string body, MailingType type = MailingType.RegularMail,
            bool searchForCommas = false) {
            if (IsConfigured && !to.IsEmpty()) {
                var t = new Thread(() => {
                    var mail = GetNewMailMessage(SenderEmail, to, subject,body);
                    MailingAddressAttach(ref mail, to, type, searchForCommas);
       
                    try {
                        var mailer = CloneSmtpClient();
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
                    var mail = GetNewMailMessage(SenderEmail, to, subject, body);
                    MailingAddressAttach(ref mail, to, carbonCopyEmails, type);
                    try {
                        var mailer = CloneSmtpClient();
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
                    var mail = GetNewMailMessage(SenderEmail, to);
                    MailingAddressAttach(ref mail, to, type, searchForCommas);
                    mail.Subject = subject;
                    mail.Body = body;
                    try {
                        var messageAttachment = new Attachment(fileName);
                        mail.Attachments.Add(messageAttachment);
                        var mailer = CloneSmtpClient();
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