using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;

namespace DevMVCComponent.Mailers {
    public class Mailer : MailerBase {

        public const bool SEND_ASY = true;

        #region Quick Mails

        /// <summary>
        /// When task is started mail will be sent asynchronously 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="sub"></param>
        /// <param name="to"></param>
        /// <param name="isHtml"></param>
        /// <param name="send"></param>
        /// <param name="NotifyDeveloper"></param>
        /// <param name="NotifyAdmin"></param>
        /// <returns></returns>
        public Task QuickMail(string body, string sub, string to = "", bool isHtml = true, bool send = true, bool NotifyDeveloper = false, bool NotifyAdmin = false) {
            Task t = new Task(() => {
                var mail = new MailMessage();
                mail.Subject = sub;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;
                if (send) {
                    if (to != "" && to != null) {
                        mail.To.Add(to);
                    }
                    if (NotifyAdmin && Config.AdminEmail != null) {
                        mail.To.Add(Config.AdminEmail);
                    }
                    if (NotifyDeveloper && Config.DeveloperEmail != null) {
                        mail.To.Add(Config.DeveloperEmail);
                    }
                }

                mail.SendAsync();
            });
            if (send) {
                t.Start();
            }
            return t;
        }

        #endregion

        /// <summary>
        /// Add alternative view and mail body encoding.
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public MailMessage SetMailDefaults(MailMessage mail) {
            mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            AlternateView altView = AlternateView.CreateAlternateViewFromString("Please check your spam box for " + Config.ApplicationName + " email.", null, "text/plain");
            mail.AlternateViews.Add(altView);
            return mail;
        }

        /// <summary>
        /// Calls SetMailDefaults first.
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        public void Send(MailMessage mail, string subject = "", string to = "") {
            mail = SetMailDefaults(mail);
            if (subject != null && subject == "") {
                mail.Subject = subject;
            }

            if (to != null && to != "") {
                mail.To.Add(to);
            }

            try {
                new Thread(() => {
                    if (SEND_ASY)
                        mail.SendAsync();
                    else
                        mail.Send();
                }).Start();
            } catch (Exception) {

            }
        }
    }


}