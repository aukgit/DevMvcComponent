using System;

namespace TestDevMvc.Testing {
    [Serializable]
    public class ClassA {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }

        public void Print() {
            Console.WriteLine("A : " + this.A);
            Console.WriteLine("B : " + this.B);
            Console.WriteLine("C : " + this.C);
        }
    }
}