using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przeplywy
{
    class Program
    {
        const int n = 4;
        static void Main(string[] args)
        {
            Graf g = new Graf(n);

            {
                g.DodajKrawedzSkierowana(0, 1, 10);
                g.DodajKrawedzSkierowana(0, 3, 5);
                g.DodajKrawedzSkierowana(1, 2, 5);
                g.DodajKrawedzSkierowana(3, 2, 10);
                g.DodajKrawedzSkierowana(1, 3, 5);
                g.DodajKrawedzSkierowana(3, 1, 5);
            } // Dodawanie krawedzi

            var przeplyw = g.MaksymalnyPrzeplyw_FordFulkerson(0, 2);

            Console.WriteLine("Graf:");
            WypiszMacierzWag(g.MacierzMaxPrzeplywu, n);
            Console.WriteLine();

            Console.WriteLine("Maksymalny przepływ: " + przeplyw.Key.ToString());
            WypiszMacierzWag(przeplyw.Value, n);

            Console.ReadLine();
        }

        public static void WypiszMacierzWag(int[,] m, int n)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("    ");
            for (int i = 0; i < n; i++)
                Console.Write(" {0:D2} ", i);
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(" {0:D2} ", i);

                Console.BackgroundColor = defaultColor;
                for (int j = 0; j < n; j++)
                {
                    if (m[i, j] == 0)
                        Console.Write("    ");
                    else
                    {
                        if (m[i,j] > 0)
                            Console.Write(" {0:D2} ", m[i, j]);
                        else
                            Console.Write("{0:D2} ", m[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
