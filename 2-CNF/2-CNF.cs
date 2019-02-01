using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_CNF
{
    public static class _2_CNF
    {
        public static bool CzySpelnialne(string wyr)
        {
            // Dla x0..x(n-1), graf zawieraja 2n wierzchołków, niezanegowane 0..n-1 oraz zanegowane n..2n-1
            Graf g = WczytajGraf(wyr);

            return CzySpelnialne(g);
        }
        public static bool CzySpelnialne(Graf g)
        {
            int[] SSS = g.WyznaczSSS();

            int n = g.IloscWierzcholkow / 2;
            for (int w = 0; w < n; w++)
            {
                if (SSS[w] == SSS[w + n])
                    return false;
            }

            return true;
        }
        private static Graf WczytajGraf(string input)
        {
            Graf g;
            var alt = input.Replace(" ", "").Split('&');
            int[,] data = new int[alt.Length, 2];

            int max = 0;
            for (int i = 0; i < alt.Length; i++)
            {
                var c = alt[i].Split('|');
                int n1 = int.Parse(c[0]);
                int n2 = int.Parse(c[1]);

                max = Math.Max(max, Math.Abs(n1));
                max = Math.Max(max, Math.Abs(n2));

                data[i, 0] = n1;
                data[i, 1] = n2;
            }

            g = new Graf(max * 2);

            for (int i = 0; i < alt.Length; i++)
            {
                StworzImplikacje(g, data[i, 0], data[i, 1]);
            }

            return g;
        }
        private static void StworzImplikacje(Graf g, int n1, int n2) // W tej metodzie indeksowanie od 1
        {
            if (n1 == 0 || n2 == 0 || g == null)
                throw new Exception();

            if (n1 < 0)
                n1 = Zaneguj(-n1, g.IloscWierzcholkow);
            if (n2 < 0)
                n2 = Zaneguj(-n2, g.IloscWierzcholkow);

            // X1 | X2   ->   ~X2 => X1
            g.DodajKrawedzSkierowana(Zaneguj(n2, g.IloscWierzcholkow) - 1, n1 - 1);
            // X1 | X2   ->   ~X1 => X2
            g.DodajKrawedzSkierowana(Zaneguj(n1, g.IloscWierzcholkow) - 1, n2 - 1);
        }
        private static int Zaneguj(int n, int iloscWierzcholkow)
        {
            return (n - 1 + iloscWierzcholkow / 2) % iloscWierzcholkow + 1;
        }
    }
}
