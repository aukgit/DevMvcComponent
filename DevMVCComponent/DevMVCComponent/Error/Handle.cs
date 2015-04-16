using DevMVCComponent.Database;
using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;


namespace DevMVCComponent.Error {
    public class Handle {

        public Handle() {
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        public Handle(Exception ex) {
            ByEmail(ex, "No Name");
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        public Handle(Exception ex, string method) {
            ByEmail(ex, method);
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        public Handle(Exception ex, string method, string subject) {
            ByEmail(ex, method, subject);
        }

        public Handle(Exception ex, string method, object entity) {
            ByEmail(ex, method, "", entity);
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>

        public Handle(Exception ex, string method, string subject, object entity) {
            ByEmail(ex, method, subject, entity);
        }

        public void HandleBy(Exception ex) {
            ByEmail(ex, "No Name");
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        public void HandleBy(Exception exception, string method)
        {
            ByEmail(exception, method);
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        public void HandleBy(Exception exception, string method, string subject)
        {
            ByEmail(exception, method, subject);
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>
        public void HandleBy(Exception exception, string method, object entity)
        {
            ByEmail(exception, method, "", entity);
        }
        /// <summary>
        /// Sends an email to the developer if run into any errors.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method">Method name should contains parenthesis.()</param>
        /// <param name="subject">Email subject</param>
        /// <param name="entity">Single entity data that you are trying to save. You can also pass null.</param>
        public void HandleBy(Exception exception, string method, string subject, object entity) {
            ByEmail(exception, method, subject, entity);
        }


        public string GetEntityValidationHTML(DbEntityValidationException e, string MethodName, string Optional = "") {
            string showError = String.Format("(Failed)Method: {0}\n" +
                                               "<br/>Exception :{1}\n" +
                                               "<br/><b>Stack Trace :{2}</b>\n" +
                                               "<br/>Optional:{3}\n", MethodName, e.ToString(), e.StackTrace, Optional);
            //Trace.TraceError(showError);
            //Console.WriteLine(showError);
            showError += "<br/>DBEntity Errors:-><br/> <div style='color:red;font-weight:bolder;'>";
            foreach (var eve in e.EntityValidationErrors) {
                showError += String.Format("EntityType: {0}<br/>" +
                                           "State :{1}<br/>", eve.Entry.Entity.GetType().Name, eve.Entry.State.ToString());

                foreach (var ve in eve.ValidationErrors) {
                    //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    showError += String.Format("->Property: {0}<br/>" +
                                              "->Error:{1}<br/>", ve.PropertyName, ve.ErrorMessage);

                }
            }
            showError += "</div><br/>";
            return showError;

        }

        public string GetErrorMsgHTML(Exception e, string MethodName, string Optional = "") {
            string inner = "";
            if (e is DbEntityValidationException) {
                return GetEntityValidationHTML((DbEntityValidationException)e, MethodName, Optional);

            }
            if (e.InnerException != null) {
                inner = e.InnerException.ToString();
            }
            string showError = String.Format("(Failed)Method: {0}<br>" +
                                             "Exception :{2}<br>" +
                                             "<h3 style='color:red;font-weight:bolder;'>Message:{1}</h3><br/>" +
                                             "Source:{3}<br>" +
                                             "Inner Exception:{4}<br>" +
                                             "Stack Trace:{5}<br>" +
                                             "Optional:{6}<br><hr />", MethodName, e.ToString(), e.Message, e.Source, inner, e.StackTrace, Optional);

            return showError;

        }

        public void GenerateErrorBody(Exception ex, ref string subject, ref string body, string method = "", object EntitySingleObject = null) {
            if (body == null)
                body = "";

            if (Config.DeveloperEmail != null && Config.DeveloperEmail != "" && Config.IsNotifyDeveloper) {
                body += GetErrorMsgHTML(ex, method);

                if (subject == null || subject == "")
                    subject = string.Format("[{0}] [Error] on [{1}] method at {2}", Config.ApplicationName, method, DateTime.UtcNow);

                if (EntitySingleObject != null) {
                    body += "<hr/>";

                    body += "<h1> Entity Description :</h1>";
                    body += "<h1> " + EntitySingleObject.ToString() + "</h1>";
                    try {
                        body += "<div style='color:green'> " + EntityToString.GetHTML(EntitySingleObject) + "</div>";
                    } catch (Exception ex2) {
                        body += "<div style='color:red'> Error Can't Read Entity: " + ex2.Message.ToString() + "</div>";
                    }
                }
                body += "<hr />";
                body += "<div style='background-color:yellow'> Stack Trace: " + ex.StackTrace + "</div>";
            }
        }

        private void ByEmail(Exception exception, string methodName, string subject = "", object entity = null) {
           
            new Task(() => {
                if (Config.DeveloperEmail != null && Config.IsNotifyDeveloper) {
                    string body = "";
                  
                    GenerateErrorBody(exception, ref subject, ref body, methodName, entity);
                    body += Config.GetApplicationNameHTML();
                    if (Starter.Mailer != null) {
                        Starter.Mailer.QuickSend(Config.DeveloperEmail, subject, body);
                    }
                }
            }).Start();
            
        }

    }
}