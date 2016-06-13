using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevMvcComponent;
using DevMvcComponent.Mail;

namespace TestMvcWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var password = "newPasswordPleaseDon'tChangeItForEveryone";
            var mailServer = new GmailServer("testing email", "just.food.mailer@gmail.com", password);
            Mvc.Setup("DevMvcComponent Test", "devorg.bd@gmail.com", assembly, mailServer);

            Mvc.Mailer.QuickSend("devorg.bd@gmail.com", "subject", "<b>body</b>", isHtml: true);
            //try {
            //    throw new Exception("Hello World");
            //} catch (Exception ex) {
            //    Mvc.Error.ByEmail(ex, "devorg.bd@gmail.com,alim.enosis@gmail.com", "Method Name", "Custom Subject", Mvc.Mailer);
            //}
            //Console.WriteLine("Done sending email");
        }
    }
}
