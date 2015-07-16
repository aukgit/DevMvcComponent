using System;
using System.Reflection;
using DevMvcComponent.Mailer;

namespace Tester {
    class Program {
        static void Main(string[] args) {
            var assembly = Assembly.GetExecutingAssembly();
            var gmailMailer = new CustomMailConfig("testmailer.why@gmail.com", "asdf1234@#", "smtp.gmail.com", 587);
            gmailMailer.SendAsynchronousEmails = false;
            DevMvcComponent.Starter.Setup("w", "sscsc", assembly, gmailMailer);
            DevMvcComponent.Starter.Mailer.QuickSend("devorg.bd@gmail.com", "Hello", "Body");
            Console.WriteLine("Mail sent.");
            Console.ReadKey();

        }
    }
}
