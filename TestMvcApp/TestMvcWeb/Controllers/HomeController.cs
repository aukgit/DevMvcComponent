using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevMvcComponent;
using DevMvcComponent.Extensions;

namespace TestMvcWeb.Controllers {
    public class HomeController : Controller {
        public static string name = "Hello2.bin";
        public ActionResult Index() {
            string w = "";
            w = w.GetCacheValue(name);
            w.SaveAsCookie(name);
            Mvc.Mailer.QuickSend("devorg.bd@gmail.com", "Hello test from Home ", "Hello body cache : " + w);
            var x = w.GetCookieValue(name);
            Mvc.Mailer.QuickSend("devorg.bd@gmail.com", "Hello test from Home ", "Hello body cookie : " + x);

            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";
            string w = "";
            w = w.GetSession(name);
            w.SaveAsCache(name);
            Mvc.Mailer.QuickSend("devorg.bd@gmail.com", "Hello test from About " + w, "Hello body");
            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";
            var w = "Hello World : " + DateTime.Now;
            w.SaveAsString(name);
            w.SaveInSession(name);
            ViewBag.w = w.GetCookieValue(name);

            return View();
        }
    }
}