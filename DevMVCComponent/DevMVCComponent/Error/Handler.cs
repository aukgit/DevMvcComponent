#region using block

using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DevMvcComponent.EntityConversion;
using DevMvcComponent.Mail;
using DevMvcComponent.Miscellaneous;

#endregion

namespace DevMvcComponent.Error {
    /// <summary>
    /// Error handler
    /// </summary>
    public class Handler {
        /// <summary>
        /// </summary>
        public Handler() {
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        public Handler(Exception ex) {
            ByEmail(ex, "No Name");
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        public Handler(Exception ex, string method) {
            ByEmail(ex, method);
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        public Handler(Exception ex, string method, string subject) {
            ByEmail(ex, method, subject, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        /// <param name="entity"></param>
        public Handler(Exception ex, string method, object entity) {
            ByEmail(ex, method, "", entity);
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>
        public Handler(Exception ex, string method, string subject, object entity) {
            ByEmail(ex, method, subject, entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        public void HandleBy(Exception ex) {
            ByEmail(ex, "No Name");
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exception"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        public void HandleBy(Exception exception, string method) {
            ByEmail(exception, method);
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        public void HandleBy(Exception exception, string method, string subject) {
            ByEmail(exception, method, subject, null);
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>
        public void HandleBy(Exception exception, string method, object entity) {
            ByEmail(exception, method, "", entity);
        }

        /// <summary>
        ///     Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>
        public void HandleBy(Exception exception, string method, string subject, object entity) {
            ByEmail(exception, method, subject, entity);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="methodName"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public string GetEntityValidationHtml(DbEntityValidationException e, string methodName,
            string optional = "") {
            var showError = String.Format("(Failed)Method: {0}\n" +
                                          "<br/>Exception :{1}\n" +
                                          "<br/><b>Stack Trace :{2}</b>\n" +
                                          "<br/>Optional:{3}\n", methodName, e, e.StackTrace, optional);
            //Trace.TraceError(showError);
            //Console.WriteLine(showError);
            showError += "<br/>DBEntity Errors:-><br/> <div style='color:red;font-weight:bolder;'>";
            foreach (var eve in e.EntityValidationErrors) {
                showError += String.Format("EntityType: {0}<br/>" +
                                           "State :{1}<br/>", eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors) {
                    //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    showError += String.Format("->Property: {0}<br/>" +
                                               "->Error:{1}<br/>", ve.PropertyName, ve.ErrorMessage);
                }
            }
            showError += "</div><br/>";
            return showError;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="methodName"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public string GetErrorMsgHtml(Exception e, string methodName, string optional = "") {
            var inner = "";
            if (e is DbEntityValidationException) {
                return GetEntityValidationHtml((DbEntityValidationException)e, methodName, optional);
            }
            if (e.InnerException != null) {
                inner = e.InnerException.ToString();
            }
            var showError = String.Format("(Failed)Method: {0}<br>" +
                                          "Exception :{2}<br>" +
                                          "<h3 style='color:red;font-weight:bolder;'>Message:{1}</h3><br/>" +
                                          "Source:{3}<br>" +
                                          "Inner Exception:{4}<br>" +
                                          "Stack Trace:{5}<br>" +
                                          "Optional:{6}<br>", methodName, e, e.Message, e.Source, inner,
                e.StackTrace, optional);

            return showError;
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <param name="entitySingleObject"></param>
        public void GenerateErrorBody(Exception ex, ref string subject, ref string body, string method = "",
            object entitySingleObject = null) {
            var isUserExist = HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated;

            StringBuilder sb = new StringBuilder(30);
            if (body == null) {
                body = "";
            }

            sb.Append(GetErrorMsgHtml(ex, method));

            if (string.IsNullOrEmpty(subject)) {
                subject = string.Format("[{0}] [Error] on [{1}] method at {2}", Config.ApplicationName, method,
                    DateTime.UtcNow);
            }
            if (isUserExist) {
                sb.Append("<hr />");
                var loggedUserStyle = HtmlHelper.GetCommonStyles("Green", "White", "0 0 5px 0", "8px", "3px", "bolder");
                sb.Append(HtmlHelper.GetTag("div", "Logged in user : " + HttpContext.Current.User.Identity.Name, loggedUserStyle));
            }
            if (entitySingleObject != null) {
                sb.Append("<hr/>");
                sb.Append(HtmlHelper.GetTag("h3", "Entity Title : " + entitySingleObject.ToString()));
                try {
                    var entityString = EntityToString.GetHtmlOfSingleClassAsTable(entitySingleObject);
                    sb.Append(entityString);
                } catch (Exception ex2) {
                    sb.Append("<div style='color:red'> Error Can't Read Entity: " + ex2.Message + "</div>");
                }
            }
            sb.Append("<hr />");
            sb.Append("<div style='background-color:#FFFFD1" + Config.CommonStyles + "> Stack Trace: " + ex.StackTrace + "</div>");
            body = sb.ToString();
        }

        /// <summary>
        ///     Sends an quick email to the developer.
        /// </summary>
        /// <param name="exception">Your thrown exception to log in your developers email address.</param>
        /// <param name="methodName">Name or the method : System.Reflection.MethodBase.GetCurrentMethod().Name or custom name or nameOf(methodName) C# 6.0</param>
        /// <param name="subject">Mailing subject, your app name will be included automatically.</param>
        /// <param name="entity">Your entity information.</param>
        public void ByEmail(Exception exception, string methodName = "", string subject = "", object entity = null) {
            if (methodName == "") {
                methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            }
            ByEmail(exception, Mvc.Mailer, methodName, subject, entity);
        }
        /// <summary>
        /// Send an asynchronous email to the given email addresses as carbon copy.
        /// </summary>
        /// <param name="exception">Your thrown exception to log in your developers email address.</param>
        /// <param name="mailServer">You can pass your custom mailing server to send the mail from.</param>
        /// <param name="methodName">Name or the method : System.Reflection.MethodBase.GetCurrentMethod().Name or custom name or nameOf(methodName) C# 6.0</param>
        /// <param name="subject">Mailing subject, your app name will be included automatically.</param>
        /// <param name="entity">Your entity information.</param>
        public  void ByEmail(Exception exception, MailServer mailServer, string methodName, string subject = "", object entity = null) {
            if (methodName == "") {
                methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            }
            new Thread(() => {
                if (Config.DeveloperEmails != null && Config.IsNotifyDeveloper) {
                    var body = "";
                    GenerateErrorBody(exception, ref subject, ref body, methodName, entity);
                    body += Config.GetApplicationNameHtml();
                    if (mailServer != null) {
                        mailServer.QuickSend(Config.DeveloperEmails, subject, body, Enums.MailingType.RegularMail,null, false);
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Send an asynchronous email to the given email addresses as carbon copy.
        /// </summary>
        /// <param name="exception">Your thrown exception to log in your developers email address.</param>
        /// <param name="mailingAddresses">Comma separated email address.</param>
        /// <param name="methodName">Name or the method : System.Reflection.MethodBase.GetCurrentMethod().Name or custom name or nameOf(methodName) C# 6.0</param>
        /// <param name="subject">Mailing subject, your app name will be included automatically.</param>
        /// <param name="entity">Your entity information.</param>
        public  void ByEmail(Exception exception, string mailingAddresses, string methodName, string subject = "", object entity = null) {
            if (mailingAddresses != null) {
                ByEmail(exception, mailingAddresses.Split(','), methodName, subject, entity);
            }
        }

        /// <summary>
        /// Send an asynchronous email to the given email addresses as carbon copy.
        /// </summary>
        /// <param name="exception">Your thrown exception to log in your developers email address.</param>
        /// <param name="mailingAddresses">Mailing address to send exception log as a carbon copy.</param>
        /// <param name="methodName">Name or the method : System.Reflection.MethodBase.GetCurrentMethod().Name or custom name or nameOf(methodName) C# 6.0</param>
        /// <param name="subject">Mailing subject, your app name will be included automatically.</param>
        /// <param name="entity">Your entity information.</param>
        public  void ByEmail(Exception exception, string[] mailingAddresses, string methodName, string subject = "", object entity = null) {
            if (methodName == "") {
                methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            }
            new Thread(() => {
                var body = "";
                GenerateErrorBody(exception, ref subject, ref body, methodName, entity);
                body += Config.GetApplicationNameHtml();
                Mvc.Mailer.QuickSend(mailingAddresses, subject, body, Enums.MailingType.CarbonCopy, null, false);
            }).Start();
        }
    }
}