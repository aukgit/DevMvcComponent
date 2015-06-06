#region using block

using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using DevMVCComponent.Database;

#endregion

namespace DevMVCComponent.Error {
    /// <summary>
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
            ByEmail(ex, method, subject);
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
            ByEmail(exception, method, subject);
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
        public virtual string GetEntityValidationHtml(DbEntityValidationException e, string methodName, string optional = "") {
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
        public virtual string GetErrorMsgHtml(Exception e, string methodName, string optional = "") {
            var inner = "";
            if (e is DbEntityValidationException) {
                return GetEntityValidationHtml((DbEntityValidationException) e, methodName, optional);
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
                                          "Optional:{6}<br><hr />", methodName, e, e.Message, e.Source, inner,
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
        public virtual void GenerateErrorBody(Exception ex, ref string subject, ref string body, string method = "",
            object entitySingleObject = null) {
            if (body == null)
                body = "";

            if (!string.IsNullOrEmpty(Config.DeveloperEmail) && Config.IsNotifyDeveloper) {
                body += GetErrorMsgHtml(ex, method);

                if (string.IsNullOrEmpty(subject))
                    subject = string.Format("[{0}] [Error] on [{1}] method at {2}", Config.ApplicationName, method,
                        DateTime.UtcNow);

                if (entitySingleObject != null) {
                    body += "<hr/>";

                    body += "<h1> Entity Description :</h1>";
                    body += "<h1> " + entitySingleObject + "</h1>";
                    try {
                        body += "<div style='color:green'> " + EntityToString.GetHtml(entitySingleObject) + "</div>";
                    } catch (Exception ex2) {
                        body += "<div style='color:red'> Error Can't Read Entity: " + ex2.Message + "</div>";
                    }
                }
                body += "<hr />";
                body += "<div style='background-color:yellow'> Stack Trace: " + ex.StackTrace + "</div>";
            }
        }

        /// <summary>
        /// Sends an quick email to the developer.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="methodName"></param>
        /// <param name="subject"></param>
        /// <param name="entity"></param>
        private void ByEmail(Exception exception, string methodName, string subject = "", object entity = null) {
            new Task(() => {
                if (Config.DeveloperEmail != null && Config.IsNotifyDeveloper) {
                    var body = "";

                    GenerateErrorBody(exception, ref subject, ref body, methodName, entity);
                    body += Config.GetApplicationNameHtml();
                    if (Starter.Mailer != null) {
                        Starter.Mailer.QuickSend(Config.DeveloperEmail, subject, body);
                    }
                }
            }).Start();
        }
    }
}