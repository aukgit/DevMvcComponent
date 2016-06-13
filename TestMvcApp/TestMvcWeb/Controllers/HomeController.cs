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
        public string time = "Time : " + DateTime.Now + " , min : " + DateTime.Now.Minute;

        public ActionResult Index() {
            time.SaveAsCookie(name);
            ViewBag.time = time;
            return View();
        }

        public ActionResult About() {
            var time2 = time.GetCookieValue(name);
            ViewBag.time = time2;
            
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