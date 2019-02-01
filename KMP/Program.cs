using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMP
{
    class Program
    {
        static void Main(string[] args)
        {
            // KMP
            string T = "bacbababaabcbababaca";
            string P = "ababaca";

            int p = SzukajWzorcaKMP(P, T);
            Console.WriteLine(P);
            Console.WriteLine(T.Substring(p, P.Length));
            
            Console.ReadLine();
        }

        static int SzukajWzorcaKMP(string P, string T)
        {
            int[] pi = ObliczPrefiksy(P);
            int k = 0;
            for (int q = 0; q < T.Length; q++)
            {
                while (k > 0 && P[k] != T[q])
                    k = pi[k];

                if (P[k] == T[q])
                    k++;

                if (k == P.Length)
                    return q - P.Length + 1;
            }
            return -1;
        }

        static int[] ObliczPrefiksy(string P)
        {
            int[] pi = new int[P.Length];

            pi[0] = 0;

            int k = 0;
            for (int q = 1; q < P.Length; q++)
            {
                while (k > 0 && P[k] != P[q])
                    k = pi[k];

                if (P[k] == P[q])
                    k++;

                pi[q] = k;
            }
            return pi;
        }
    }
}
