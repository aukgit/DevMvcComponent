using System;
using DevMvcComponent.Extensions;
using TestDevMvc.Testing;

namespace TestDevMvc {
    internal class Program {
        private static void Main(string[] args) {
            //while (true) {
            //    var input1 = Console.ReadLine();
            //    var input2 = Console.ReadLine();
            //    var res = input1.IsMatchAtLast(input2, 2);
            //    Console.WriteLine("\"" + input1 + "\".IsStringMatchfromLast(\"" + input2 + "\", 2): " + res);
            //    Console.ReadKey();
            //}

            // file saving test

            ClassA a = null;
            var filename = "Hello.bin";
            a = a.ReadBinaryAs<ClassA>(filename);
            if (a == null) {
                a = new ClassA();
                a.B = "Hello World " + DateTime.Now;
                a.A = "Minute " + DateTime.Now.Minute;
            }
            a.Print();
            a.SaveAsBinary(filename);
            a = a.ReadBinaryAs(filename);
            a.Print();
            Console.ReadKey();
        }
    }
}