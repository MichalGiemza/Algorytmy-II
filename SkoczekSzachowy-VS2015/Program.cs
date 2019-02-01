using System;

namespace AlgorytmyII
{
    class Program
    {
        static void Main(string[] args)
        {
            PierwszeRozwiazanie();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            WszystkieRozwiazania();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            MetodaWarnsdorffa();
        }

        static void PierwszeRozwiazanie()
        {
            var rozwiazanie = SkoczekSzachowy.ZnajdzRozwiazanie(5, 5);
            Console.WriteLine("Jedno rozwiązanie:");
            SkoczekSzachowy.WypiszSzachownice(rozwiazanie, 5, 5);

            Console.ReadLine();
        }
        
        static void WszystkieRozwiazania()
        {
            var rozwiazania = SkoczekSzachowy.ZnajdzWszystkieRozwiazania(5, 5);
            
            foreach (var r in rozwiazania)
                SkoczekSzachowy.WypiszSzachownice(r, 5, 5);
            
            Console.WriteLine("\nWszystkie rozwiązania: (ilość: {0})", rozwiazania.Count);

            Console.ReadLine();
        }

        static void MetodaWarnsdorffa()
        {
            var warnsdorff = SkoczekSzachowy.ZnajdzRozwiazanieWarnsdorff(10);
            Console.WriteLine("Jedno rozwiązanie dla reguły Warnsdorffa:");
            SkoczekSzachowy.WypiszSzachownice(warnsdorff, 10, 10);

            Console.ReadLine();
        }
    }
}
