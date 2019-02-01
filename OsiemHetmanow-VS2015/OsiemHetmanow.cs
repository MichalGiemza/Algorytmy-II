using System;
using System.Collections.Generic;
using System.Text;

namespace OsiemHetmanow
{
    class OsiemHetmanow
    {
        public static int[] ZnajdzRozwiazanie(int wymiarySzachownicy)
        {
            int[] ustawienieHetmanow = CzystaSzachownica(wymiarySzachownicy);
            int pozycjaX = 0;

            if (ProbujUstawic(ustawienieHetmanow, pozycjaX))
                return ustawienieHetmanow;
            else
                return null;
        }

        public static List<int[]> ZnajdzWszystkieRozwiazania(int wymiarySzachownicy)
        {
            int[] ustawienieHetmanow = CzystaSzachownica(wymiarySzachownicy);
            int pozycjaX = 0;
            List<int[]> rozwiazania = new List<int[]>();

            ProbujUstawicDlaWieluRozwiazan(ustawienieHetmanow, pozycjaX, rozwiazania);
                
            return rozwiazania;
        }

        private static bool ProbujUstawicDlaWieluRozwiazan(int[] ustawienieHetmanow, int pozycjaX, List<int[]> rozwiazania)
        {
            bool probaUdana = false;
            int pozycjaY = -1;
            
            while (probaUdana == false && pozycjaX < ustawienieHetmanow.Length)
            {
                if (pozycjaY < ustawienieHetmanow.Length)
                    pozycjaY += 1;
                else
                    return false;

                if (PozycjaDozwolona(ustawienieHetmanow, pozycjaX, pozycjaY))
                {
                    UstawHetmana(ustawienieHetmanow, pozycjaX, pozycjaY);

                    if (pozycjaX + 1 < ustawienieHetmanow.Length)
                    {   // Rekurencja po pozycjach w poziomie
                        probaUdana = ProbujUstawicDlaWieluRozwiazan(ustawienieHetmanow, pozycjaX + 1, rozwiazania);
                        
                        if (probaUdana == false)
                            UsunHetmana(ustawienieHetmanow, pozycjaX);
                    }
                    else
                    {
                        // Hetmani ustawieni
                        rozwiazania.Add((int[])ustawienieHetmanow.Clone());
                        UsunHetmana(ustawienieHetmanow, pozycjaX);
                        continue;
                    }
                }
            }
            return true;
        }

        private static bool ProbujUstawic(int[] ustawienieHetmanow, int pozycjaX)
        {
            bool probaUdana = false;
            int pozycjaY = -1;

            while (probaUdana == false && pozycjaX < ustawienieHetmanow.Length)
            {
                if (pozycjaY < ustawienieHetmanow.Length)
                    pozycjaY += 1;
                else
                    return false;
                   
                if (PozycjaDozwolona(ustawienieHetmanow, pozycjaX, pozycjaY))
                {
                    UstawHetmana(ustawienieHetmanow, pozycjaX, pozycjaY);

                    if (pozycjaX + 1 < ustawienieHetmanow.Length)
                    {   // Rekurencja po pozycjach w poziomie
                        probaUdana = ProbujUstawic(ustawienieHetmanow, pozycjaX + 1);

                        if (probaUdana == false)
                            UsunHetmana(ustawienieHetmanow, pozycjaX);
                    }
                    else
                        // Hetmani ustawieni
                        return true;
                }
            }
            return probaUdana;
        }

        private static int[] CzystaSzachownica(int wymiarySzachownicy)
        {
            int[] szachwonica = new int[wymiarySzachownicy];

            for (int i = 0; i < wymiarySzachownicy; i++)
                szachwonica[i] = -1;

            return szachwonica;
        }

        private static bool PozycjaDozwolona(int[] ustawienieHetmanow, int pozycjaX, int pozycjaY)
        {
            // Czy niedozwolona w pionie, czy zajęta
            if (ustawienieHetmanow[pozycjaX] != -1)
                return false;

            for (int i = 0; i < ustawienieHetmanow.Length; i++)
            {
                // Czy niedozwolona w poziomie
                if (pozycjaY == ustawienieHetmanow[i])
                    return false;

                // Czy niedozwolona w skosie (rosnący)
                if (ustawienieHetmanow[i] != -1 && ustawienieHetmanow[i] == pozycjaY - pozycjaX + i)
                    return false;

                // Czy niedozwolona w skosie (malejący)
                if (ustawienieHetmanow[i] != -1 && ustawienieHetmanow[i] == pozycjaY + pozycjaX - i)
                    return false;
            }
            return true;
        }
        
        private static void UstawHetmana(int[] ustawienieHetmanow, int pozycjaX, int pozycjaY)
        {
            ustawienieHetmanow[pozycjaX] = pozycjaY;
        }

        private static void UsunHetmana(int[] ustawienieHetmanow, int pozycjaX)
        {
            ustawienieHetmanow[pozycjaX] = -1;
        }

        public static void WypiszSzachownice(int[] ustawienieHetmanow)
        {
            foreach (var pozycja in ustawienieHetmanow)
            {
                Console.Write(" {0}", pozycja);
            }
            Console.WriteLine();
        }
    }
}
