using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDevMvc.Testing;
using DevMvcComponent.Extensions;
namespace TestDevMvc {
    class Program {
        static void Main(string[] args) {
            var a = new ClassA() {
                A = 1,
                B = 2,
                C = 3
            };
            var b = a.Cast<ClassA, ClassB>();
            Console.WriteLine(b.A);
            Console.WriteLine(b.B);
            Console.ReadKey();
        }
    }
}
