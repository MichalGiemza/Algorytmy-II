using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_CNF
{
    class Program
    {
        static void Main(string[] args)
        {
            const string wyr1 = "1|3 & 1|-4 & 2|-4 & 2|-5 & 3|-5 & 1|-6 & 2|-6 & 3|-6 & 4|7 & 5|7 & 6|7";
            const string wyr2 = "1|1 & -1|-1";
            const string wyr3 = "1|2 & 2|3 & 3|1 & 4|-2 & -3|-4";

            bool r1 = _2_CNF.CzySpelnialne(wyr1);
            bool r2 = _2_CNF.CzySpelnialne(wyr2);
            bool r3 = _2_CNF.CzySpelnialne(wyr3);

            Console.WriteLine(wyr1);
            Console.WriteLine("Wyrażenie jest spełnialne: " + (r1 ? "Prawda" : "Fałsz"));
            Console.WriteLine();
            Console.WriteLine(wyr2);
            Console.WriteLine("Wyrażenie jest spełnialne: " + (r2 ? "Prawda" : "Fałsz"));
            Console.WriteLine();
            Console.WriteLine(wyr3);
            Console.WriteLine("Wyrażenie jest spełnialne: " + (r3 ? "Prawda" : "Fałsz"));

            Console.ReadLine();
        }
    }
}

//Graf g = new Graf(8);
//
//{
//    g.DodajKrawedzSkierowana(0, 1);
//
//    g.DodajKrawedzSkierowana(1, 2);
//    g.DodajKrawedzSkierowana(1, 4);
//    g.DodajKrawedzSkierowana(1, 5);
//
//    g.DodajKrawedzSkierowana(2, 3);
//    g.DodajKrawedzSkierowana(2, 6);
//
//    g.DodajKrawedzSkierowana(3, 2);
//    g.DodajKrawedzSkierowana(3, 7);
//
//    g.DodajKrawedzSkierowana(4, 0);
//    g.DodajKrawedzSkierowana(4, 5);
//
//    g.DodajKrawedzSkierowana(5, 6);
//
//    g.DodajKrawedzSkierowana(6, 5);
//    g.DodajKrawedzSkierowana(6, 7);
//
//    g.DodajKrawedzSkierowana(7, 7);
//} // Dodawanie krawedzi