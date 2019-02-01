using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmKruskala
{
    class Program
    {
        const int n = 8;
        static void Main(string[] args)
        {
            Graf g = new Graf(n);
            Random r = new Random();
            
            for (int i = 0; i < n; i++) // dodanie krawedzi
            {
                for (int j = i; j < n; j++)
                {
                    g.DodajKrawedzNieskierowana(i, j, r.Next(1, 100));
                }
            }

            Console.WriteLine("Macierz wag grafu g: ");
            WypiszMacierzWag(g.MacierzWag, n);


            Graf mst = g.MinDrzewoRozp_Kruskal();
            Console.WriteLine("Macierz wag drzewa mst stworzonego na podstawie grafu g: ");
            WypiszMacierzWag(mst.MacierzWag, n);

            Console.ReadLine();
        }

        public static void WypiszMacierzWag(int[,] m, int n)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("   ");
            for (int i = 0; i < n; i++)
                Console.Write("{0:D2} ", i);
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("{0:D2} ", i);

                Console.BackgroundColor = defaultColor;
                for (int j = 0; j < n; j++)
                {
                    if (j <= i)
                    {
                        Console.Write("   ");
                        continue;
                    }

                    if (m[i, j] == int.MaxValue)
                        Console.Write("-- ");
                    else
                        Console.Write("{0:D2} ", m[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
