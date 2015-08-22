using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevMvcComponent;
using TestDevMvc.Testing;
using DevMvcComponent.Extensions;
using DevMvcComponent.Mail;
using DevMvcComponent.Miscellaneous;

namespace TestDevMvc {
    class Program {
        static void Main(string[] args) {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var password = "newPasswordPleaseDon'tChangeItForEveryone";
            var mailServer = new GmailServer("just.food.mailer@gmail.com", password);
            Mvc.Setup("TestMvc", "devorg.bd@gmail.com", assembly, mailServer);

            Mvc.Mailer.QuickSend("devorg.bd@gmail.com", "subject", "<b>body</b>", isHtml: false);
            try {
                throw new Exception("Hello World");
            } catch (Exception ex) {
                Mvc.Error.ByEmail(ex, "devorg.bd@gmail.com,alim.enosis@gmail.com", "Method Name", "Custom Subject", Mvc.Mailer);
            }
            Console.WriteLine("Done sending email");
            Console.ReadKey();
        }
    }
}
