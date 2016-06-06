using System;

namespace TestDevMvc {
    internal class Program {
        private static void Main(string[] args) {
            while (true) {
                var input1 = Console.ReadLine();
                var input2 = Console.ReadLine();
                var res = input1.IsStringMatchfromLast(input2, 2);
                Console.WriteLine("\"" + input1 + "\".IsStringMatchfromLast(\"" + input2 + "\", 2): " + res);
                Console.ReadKey();
            }
        }
    }
}