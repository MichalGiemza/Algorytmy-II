using System;

namespace OsiemHetmanow
{
    class Program
    {
        static void Main(string[] args)
        {
            var rozwiazanie = OsiemHetmanow.ZnajdzRozwiazanie(8);
            Console.WriteLine("Jedno rozwiązanie:");
            OsiemHetmanow.WypiszSzachownice(rozwiazanie);

            Console.ReadLine();

            var rozwiazania = OsiemHetmanow.ZnajdzWszystkieRozwiazania(8);
            Console.WriteLine("Wszystkie rozwiązania: (ilość: {0})", rozwiazania.Count);

            Console.ReadLine();

            foreach (var r in rozwiazania)
                OsiemHetmanow.WypiszSzachownice(r);

            Console.ReadLine();
        }
    }
}
