#region using block

using System;
using System.Collections.Generic;
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
        private bool _isCredentialConfigured;
        private string _senderMail;
        private string _senderPassword;
        /// <summary>
        /// Display name for the email.
        /// </summary>
        public string DisplayName { get; private set; }
        protected bool IsHostConfigured;

        /// <summary>
        ///     Make sure your mail has less protection, IMAP, and pop3 set to enabled.
        ///     By default:
        ///     UseDefaultCredentials = false;
        ///     EnableSsl = true;
        ///     DeliveryMethod = SmtpDeliveryMethod.Network;
        ///     Timeout = 10000;
        /// </summary>
        /// <param name="displayName">Display user's name</param>
        protected MailServer(string displayName) {
            DisplayName = displayName;
            DefaultConfigarationSetup();
        }

        /// <summary>
        ///     Make sure your mail has less protection, IMAP, and pop3 set to enabled.
        /// </summary>
        /// <param name="displayName">Display user's name</param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        protected MailServer(string displayName, string emailAddress, string password) {
            DefaultConfigarationSetup();
            SenderEmailAddress = emailAddress;
            SenderEmailPassword = password;
            DisplayName = displayName;
            _isCredentialConfigured = true;
        }

        /// <summary>
        ///     Setup credentials automatic.
        /// </summary>
        public string SenderEmailAddress {
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
        /// Get a new mail sending wrapper.
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        public MailSendingWrapper GetMailSendingWrapper(MailMessage mailMessage) {
            return new MailSendingWrapper(this, mailMessage);
        }

        #region Mail message

        /// <summary>
        ///     Get a new mail message.
        ///     Use SmtpClient to send that mail message.
        /// </summary>
        /// <param name="sender">Your mail address</param>
        /// <param name="displayName"></param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">emailAddress body</param>
        /// <param name="isHtmlBody">By default : true</param>
        /// <param name="bodyEncoding">By default : Encoding.UTF8</param>
        /// <param name="deliveryNotification"></param>
        /// <returns></returns>
        public MailMessage GetNewMailMessage(
            string subject = "",
            string body = "",
            bool isHtmlBody = true,
            Encoding bodyEncoding = null,
            string sender = null,
            string displayName = null,
            DeliveryNotificationOptions deliveryNotification = DeliveryNotificationOptions.OnFailure) {
            bodyEncoding = bodyEncoding ?? Encoding.UTF8;
            var mail = new MailMessage {
                BodyEncoding = bodyEncoding,
                DeliveryNotificationOptions = deliveryNotification,
                IsBodyHtml = isHtmlBody,
                Body = body,
                Subject = subject
            };
            sender = sender ?? _senderMail;
            displayName = displayName ?? DisplayName;
            var mailAddress = new MailAddress(sender, displayName);

            mail.Sender = mailAddress;
            mail.From = mailAddress;
            //mail.ReplyToList.Add(mailAddress);
            return mail;
        }

        #endregion

        #region Original Attachment Method

        /// <summary>
        ///     Send emailAddress with attachments
        /// </summary>
        /// <param name="mailingTos"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ccMails"></param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="isAsync"></param>
        /// <param name="isHtml">Is the body is Html or only text.</param>
        /// <param name="searchForComma"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void SendWithAttachments(
            string[] mailingTos,
            string subject,
            string body,
            string[] ccMails,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool isAsync = true,
            bool isHtml = true,
            bool searchForComma = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            var mailSendingWrapper = GetMailSendingWrapper(mailingTos, subject, body, ccMails, attachments, type, searchForComma, isHtml);
            SendMail(mailSendingWrapper, isAsync);

        }

        #endregion

        /// <summary>
        ///     Send emailAddress with attachments
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="isAsync"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void SendWithAttachments(
            MailMessage mailMessage,
            bool isAsync = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            if (IsConfigured) {
                var mailSendingWrapper = GetMailSendingWrapper(mailMessage);
                SendMail(mailSendingWrapper, isAsync, userToken, sendCompletedEventHandler);
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        /// <summary>
        ///     Get a mail message instead of sending the direct email.
        /// </summary>
        /// <param name="mailingTos">Csv email address</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ccMails">cc email addresses as csv</param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="isHtml"></param>
        /// <exception cref="Exception">If mailer is not configured properly then : "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared."</exception>
        public MailSendingWrapper GetMailSendingWrapper(
            string mailingTos,
            string subject,
            string body,
            string ccMails,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = true,
            bool isHtml = true) {
            var sendingToEmails = GetEmailAddressList(mailingTos, searchCommas);
            var ccToEmails = GetEmailAddressList(ccMails, searchCommas);
            return GetMailSendingWrapper(sendingToEmails, subject, body, ccToEmails, attachments, type, searchCommas, isHtml);
        }


        /// <summary>
        ///     Get a mail message instead of sending the direct email.
        /// </summary>
        /// <param name="mailingTos">Csv email address</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ccMails">cc email addresses as csv</param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="isHtml"></param>
        /// <exception cref="Exception">If mailer is not configured properly then : "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared."</exception>
        public MailSendingWrapper GetMailSendingWrapper(
            string[] mailingTos,
            string subject,
            string body,
            string[] ccMails,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = true,
            bool isHtml = true) {
            var mail = GetMailMessage(mailingTos, subject, body, ccMails, attachments, type, searchCommas, isHtml);
            var server = CloneSmtpClient();
            return new MailSendingWrapper(server, mail);
        }

        /// <summary>
        ///     Get a mail message instead of sending the direct email.
        /// </summary>
        /// <param name="mailingTos">Csv email address</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ccMails">cc email addresses as csv</param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="isHtml"></param>
        /// <exception cref="Exception">If mailer is not configured properly then : "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared."</exception>
        public MailMessage GetMailMessage(
            string[] mailingTos,
            string subject,
            string body,
            string[] ccMails,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = false,
            bool isHtml = true) {
            if (IsConfigured && !mailingTos.IsEmpty()) {
                var mail = GetNewMailMessage(subject, body, isHtml);
                MailingAddressAttach(ref mail, mailingTos, ccMails, type);
                if (attachments != null) {
                    foreach (var attachment in attachments) {
                        mail.Attachments.Add(attachment);
                    }
                }
                return mail;
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        /// <summary>
        ///     Get a mail message instead of sending the direct email.
        /// </summary>
        /// <param name="mailingTos">Csv email address</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ccMails">cc email addresses as csv</param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="isHtml"></param>
        /// <exception cref="Exception">If mailer is not configured properly then : "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared."</exception>
        public MailMessage GetMailMessage(
            string mailingTos,
            string subject,
            string body,
            string ccMails,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = true,
            bool isHtml = true) {
            var sendingToEmails = GetEmailAddressList(mailingTos, searchCommas);
            var ccToEmails = GetEmailAddressList(mailingTos, searchCommas);
            return GetMailMessage(sendingToEmails, subject, body, ccToEmails, attachments, type, searchCommas, isHtml);
        }

        /// <summary>
        ///     Send emailAddress with attachments
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="isAsync"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void QuickSend(
            MailMessage mailMessage,
            bool isAsync = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            if (IsConfigured) {
                var server = CloneSmtpClient();
                var mailSendingWrapper = new MailSendingWrapper(server, mailMessage);
                SendMail(mailSendingWrapper, isAsync, userToken, sendCompletedEventHandler);
            } else {
                throw new Exception(
                    "Mailer is not configured correctly. Please check credentials , host config and mailing address maybe empty or not declared.");
            }
        }

        #region Send emails

        /// <summary>
        ///     Final method for sending emails to the user.
        /// </summary>
        /// <param name="mailWrapper"></param>
        /// <param name="async"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <returns></returns>
        public void SendMail(
            MailSendingWrapper mailWrapper,
            bool async = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            var server = mailWrapper.MailServer;
            var message = mailWrapper.MailMessage;
            if (server != null && message != null) {

                if (async) {
                    //userToken = userToken ?? "None";
                    new Thread(() => {
                        server.Send(message);
                    }).Start();
                } else {
                    server.Send(message);
                }
            }
        }

        #endregion


        #region Configuration and setup HostSetup

        /// <summary>
        ///     Specific host setup. Must ensure the boolean isHostConfigured = true.
        ///     Make sure your mail has less protection, IMAP, and pop3 set to enabled.
        /// </summary>
        public abstract void HostSetup();
        /// <summary>
        /// </summary>
        public bool IsConfigured {
            get { return _isCredentialConfigured && IsHostConfigured; }
        }

        /// <summary>
        ///     Change Credentials
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void ChangeCredentials(string email, string password) {
            SenderEmailAddress = email;
            SenderEmailPassword = password;
            _isCredentialConfigured = true;
        }

        /// <summary>
        ///     Setup credentials automatic.
        /// </summary>
        private void SetupCredentials() {
            Credentials = new NetworkCredential(SenderEmailAddress, SenderEmailPassword);
        }

        private void DefaultConfigarationSetup() {
            UseDefaultCredentials = false;
            EnableSsl = true;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            Timeout = 100000;
        }

        #endregion

        #region Cloning Smtp

        /// <summary>
        ///     Copy current smtp mailer to an new instance.
        /// </summary>
        /// <returns></returns>
        public SmtpClient CloneSmtpClient() {
            return CloneSmtpClient(this);
        }

        /// <summary>
        ///     Copy any smtp mailer to an new instance.
        /// </summary>
        /// <returns>Returns a deep copy instance of the given object.</returns>
        public SmtpClient CloneSmtpClient(SmtpClient smtp) {
            var mailSender = new SmtpClient();
            mailSender.UseDefaultCredentials = smtp.UseDefaultCredentials;
            mailSender.EnableSsl = smtp.EnableSsl;
            mailSender.DeliveryMethod = smtp.DeliveryMethod;
            mailSender.Timeout = smtp.Timeout;
            mailSender.Credentials = smtp.Credentials;
            mailSender.Port = smtp.Port;
            mailSender.Host = smtp.Host;
            return mailSender;
        }

        #endregion

        #region Quick sending emailAddress methods with Attachments

        /// <summary>
        ///     Quickly send an emailAddress.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type"></param>
        /// <param name="attachments"></param>
        /// <param name="isAsync"></param>
        /// <param name="isHtml"></param>
        /// <param name="searchCommas"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void QuickSend(
            string[] to,
            string subject,
            string body,
            MailingType type = MailingType.RegularMail,
            List<Attachment> attachments = null,
            bool isAsync = true,
            bool isHtml = true,
            bool searchCommas= true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            SendWithAttachments(to, subject, body, null, attachments, type, isAsync, isHtml,searchCommas, userToken, sendCompletedEventHandler);
        }

        /// <summary>
        ///     Quickly send an emailAddress.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="attachments"></param>
        /// <param name="isAsync"></param>
        /// <param name="isHtml"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void QuickSend(
            string to,
            string subject,
            string body,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = true,
            List<Attachment> attachments = null,
            bool isAsync = true,
            bool isHtml = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            SendWithAttachments(to, subject, body, attachments, type, searchCommas, isAsync, isHtml, userToken, sendCompletedEventHandler);
        }

        /// <summary>
        ///     Sends mail asynchronously.
        /// </summary>
        /// <param name="to">Comma to seperate multiple emailAddress addresses.</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="password"></param>
        /// <param name="type">Regular, CC, BCC</param>
        public void QuickSend(string to, string subject, string body, string from, string password,
            MailingType type = MailingType.RegularMail) {
            var emailBack = SenderEmailAddress;
            var passwordBack = SenderEmailPassword;

            SenderEmailAddress = from;
            SenderEmailPassword = password;
            SetupCredentials();
            QuickSend(to, subject, body);

            SenderEmailAddress = emailBack;
            SenderEmailPassword = passwordBack;
            SetupCredentials();
        }

        /// <summary>
        ///     Send emailAddress with attachments
        /// </summary>
        /// <param name="mailingTos">Csv email address</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <param name="type"></param>
        /// <param name="searchCommas"></param>
        /// <param name="isAsync"></param>
        /// <param name="isHtml"></param>
        /// <param name="userToken"></param>
        /// <param name="sendCompletedEventHandler"></param>
        /// <exception cref="Exception"></exception>
        public void SendWithAttachments(
            string mailingTos,
            string subject,
            string body,
            List<Attachment> attachments = null,
            MailingType type = MailingType.RegularMail,
            bool searchCommas = true,
            bool isAsync = true,
            bool isHtml = true,
            object userToken = null,
            SendCompletedEventHandler sendCompletedEventHandler = null) {
            var sendingToEmails = GetEmailAddressList(mailingTos, searchCommas);
            SendWithAttachments(sendingToEmails, subject, body, null, attachments, type, isAsync, isHtml, searchCommas, userToken, sendCompletedEventHandler);
        }

        #endregion

        #region Mail address attachments
        /// <summary>
        /// Get array of email addresses from csv of email addresses.
        /// If searchComma = false then return only one email address as an array.
        /// </summary>
        /// <param name="mailingTos"></param>
        /// <param name="searchComma">If searchComma = false then return only one email address as an array.</param>
        /// <returns>If mailingTos is empty then returns null or else array of email addresses from csv or if no comma then only one email address in the array.</returns>
        private string[] GetEmailAddressList(string mailingTos, bool searchComma = false) {
            var isEmpty = mailingTos.IsEmpty();
            if (searchComma && !isEmpty && mailingTos.IndexOf(",", StringComparison.Ordinal) > -1) {
                return mailingTos.Split(',').ToArray();
            } else if (!isEmpty) {
                return new[] { mailingTos };
            }
            return null;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail">MailMessage object to attach the email address to it.</param>
        /// <param name="mailTos">Expecting at least one email address in the array. On null throw exception.</param>
        /// <param name="mailCc">if null then no action or else add to the cc list.</param>
        /// <param name="type"></param>
        private void MailingAddressAttach(ref MailMessage mail, string[] mailTos, string[] mailCc, MailingType type) {
            mail.To.Clear();
            foreach (var mailTo in mailTos) {
                var mailAddress = new MailAddress(mailTo);
                mail.ReplyToList.Add(mailAddress);
                mail.To.Add(mailAddress);
            }
            if (mailCc == null) {
                return;
            }
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

        #endregion
    }
}