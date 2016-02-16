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
    public class ClassA {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

  
    }

    public class ClassB {
        public int A { get; set; }
        public int B { get; set; }
        public int D { get; set; }
    }

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
                Mvc.Error.ByEmail(ex, "devorg.bd@gmail.com,alim.enosis@gmail.com", "", "Custom Subject", Mvc.Mailer);
            }
            Console.WriteLine("Done sending email");

          

            var mailServer2 = new CustomMailServer("info@wereviewapp.com", "New1Year20011@3!", "mail.wereviewapp.com",
                587, false);
            Mvc.Setup("Test mail", "devorg.bd@gmail.com", assembly, mailServer2);

            mailServer.QuickSend("devorg.bd@gmail.com", "Testing email", "<b>Body</b>");
            try {
                throw new Exception("Error example");
            } catch (Exception ex) {
                Mvc.Error.ByEmail(ex, "devorg.bd@gmail.com,alim.enosis@gmail.com", "", "Custom Subject", Mvc.Mailer);
            }
            Mvc.Error.ByEmail(new Exception("Hello World"), mailServer2, "");

            //var aType = new ClassA() {
            //    A = 1, B = 2, C = 3
            //};

            //var bType = new ClassB(){
            //    A = aType.A, B = aType.B // no loger need that.
            //};
            // using DevMvcComponent.Extensions;
            //var bType = aType.Cast<ClassA, ClassB>(); // returns a new ClassB object having A = 1, B= 2; 
            //Console.WriteLine("A:" + bType.A);
            //Console.WriteLine("B:" + bType.B);
            //Console.WriteLine("D:" + bType.D);


            Console.ReadKey();
        }
    }
}
