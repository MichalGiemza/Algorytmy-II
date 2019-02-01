using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmyII
{
    public static class SkoczekSzachowy
    {
        private static readonly int[][] listaRuchowSkoczka = new int[][]
        {
            new int[2] { 2, 1},   // 1
            new int[2] { 1, 2},   // 2
            new int[2] { -1, 2},  // 3
            new int[2] { -2, 1},  // 4
            new int[2] { -2, -1}, // 5
            new int[2] { -1, -2}, // 6
            new int[2] { 1, -2},  // 7
            new int[2] { 2, -1}   // 8
        };

        public static int[,] ZnajdzRozwiazanie(int szerokoscSzachownicy, int wysokoscSzachownicy, int pozycjaStartowaX = 0, int pozycjaStartowaY = 0) 
        {
            int[,] rozwiazanie = new int[wysokoscSzachownicy, szerokoscSzachownicy];
            int ruchyWykonane = 1;
            int[] pozycjaStartowa;
            
            pozycjaStartowa = new int[2] { pozycjaStartowaY, pozycjaStartowaX };
            rozwiazanie[pozycjaStartowa[0], pozycjaStartowa[1]] = 1;

            ProbujRuch(rozwiazanie, pozycjaStartowa, ref ruchyWykonane, szerokoscSzachownicy, wysokoscSzachownicy);
            if (ruchyWykonane == szerokoscSzachownicy * wysokoscSzachownicy)
                return rozwiazanie;

            return null;
        }

        public static List<int[,]> ZnajdzWszystkieRozwiazania(int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            int[,] rozwiazanie;
            int ruchyWykonane;
            int[] pozycjaStartowa;
            List<int[,]> rozwiazania = new List<int[,]>();

            //for (int i = 0; i < wysokoscSzachownicy; i++)
            //{
            //    for (int j = 0; j < szerokoscSzachownicy; j++)
            //    {
            pozycjaStartowa = new int[2] { 0, 0 };//{ i, j };

            rozwiazanie = new int[wysokoscSzachownicy, szerokoscSzachownicy];

            rozwiazanie[pozycjaStartowa[0], pozycjaStartowa[1]] = 1;
            ruchyWykonane = 1;

            ProbujRuchWieleRozwiazan(rozwiazanie, pozycjaStartowa, ref ruchyWykonane, szerokoscSzachownicy, wysokoscSzachownicy, rozwiazania);
            //    }
            //}
            return rozwiazania;
        }

        public static int[,] ZnajdzRozwiazanieWarnsdorff(int wymiarySzachownicy, int pozycjaStartowaX = 0, int pozycjaStartowaY = 0)
        {
            int[,] rozwiazanie = new int[wymiarySzachownicy, wymiarySzachownicy];
            int ruchyWykonane = 1;
            int[] pozycjaStartowa;
            int[,] tablicaMozliwosciRuchow = StworzTabliceMozliwosciRuchow(rozwiazanie, wymiarySzachownicy, wymiarySzachownicy);

            pozycjaStartowa = new int[2] { pozycjaStartowaY, pozycjaStartowaX };
            rozwiazanie[pozycjaStartowa[0], pozycjaStartowa[1]] = 1;

            ProbujRuchWarnsdorff(rozwiazanie, pozycjaStartowa, ref ruchyWykonane, wymiarySzachownicy, tablicaMozliwosciRuchow);
            if (ruchyWykonane == wymiarySzachownicy * wymiarySzachownicy)
                return rozwiazanie;

            return null;
        }

        private static bool ProbujRuchWarnsdorff(int[,] szachownica, int[] pozycja, ref int ruchyWykonane, int wymiarySzachownicy, int[,] tablicaMozliwosciRuchow)
        {
            bool ruchUdany = true;
            Queue<int[]> listaRuchow;
            int[] nowaPozycja;

            listaRuchow = StworzListeRuchowWarnsdorff(szachownica, pozycja, tablicaMozliwosciRuchow, wymiarySzachownicy);
            do
            {
                if (listaRuchow.Count == 0)
                    break;

                nowaPozycja = listaRuchow.Dequeue();

                if (CzyRuchDopuszczalny(szachownica, nowaPozycja, wymiarySzachownicy, wymiarySzachownicy))
                {
                    WykonajRuchWarnsdorff(nowaPozycja, szachownica, ref ruchyWykonane, tablicaMozliwosciRuchow, wymiarySzachownicy, wymiarySzachownicy); // WIP

                    if (ruchyWykonane != wymiarySzachownicy * wymiarySzachownicy)
                    {
                        ruchUdany = ProbujRuchWarnsdorff(szachownica, nowaPozycja, ref ruchyWykonane, wymiarySzachownicy, tablicaMozliwosciRuchow);

                        if (ruchUdany == false)
                            CofnijRuchWarnsdorff(nowaPozycja, szachownica, ref ruchyWykonane, tablicaMozliwosciRuchow, wymiarySzachownicy, wymiarySzachownicy); // WIP
                        else
                            return true;
                    }
                    else
                        return true; // Dotarcie do rozwiązania
                }
            } while (true);

            return false;
        }

        private static Queue<int[]> StworzListeRuchowWarnsdorff(int[,] szachownica, int[] pozycja, int[,] tablicaMozliwosciRuchow, int wymiarySzachownicy)
        {
            SortedList<int, int[]> listaRuchow = new SortedList<int, int[]>(new DuplicateKeyComparer<int>());
            
            foreach (var ruch in listaRuchowSkoczka)
            {
                int[] nowaPozycja = new int[] {
                        pozycja[0] + ruch[0],
                        pozycja [1] + ruch[1]
                    };

                if (CzyRuchDopuszczalny(szachownica, nowaPozycja, wymiarySzachownicy, wymiarySzachownicy))
                    listaRuchow.Add(tablicaMozliwosciRuchow[nowaPozycja[0], nowaPozycja[1]], nowaPozycja);
            }
            return new Queue<int[]>(listaRuchow.Values);
        }

        private static int[,] StworzTabliceMozliwosciRuchow(int[,] szachownica, int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            int[,] tablicaMozliwosciRuchow = new int[wysokoscSzachownicy, szerokoscSzachownicy];

            for (int i = 0; i < wysokoscSzachownicy; i++)
            {
                for (int j = 0; j < szerokoscSzachownicy; j++)
                {
                    foreach (var r in listaRuchowSkoczka)
                    {
                        if (CzyRuchDopuszczalny(szachownica, new int[] { r[0] + i, r[1] + j }, szerokoscSzachownicy, wysokoscSzachownicy))
                            tablicaMozliwosciRuchow[i, j] += 1;
                    }
                }
            }
            return tablicaMozliwosciRuchow;
        }

        private static bool ProbujRuchWieleRozwiazan(int[,] szachownica, int[] pozycja, ref int ruchyWykonane, int szerokoscSzachownicy, int wysokoscSzachownicy, List<int[,]> rozwiazania)
        {
            bool ruchUdany = true;
            Queue<int[]> listaRuchow;
            int[] nowaPozycja;

            listaRuchow = StworzListeRuchow(pozycja);
            do
            {
                if (listaRuchow.Count == 0)
                    break;

                nowaPozycja = listaRuchow.Dequeue();

                if (CzyRuchDopuszczalny(szachownica, nowaPozycja, szerokoscSzachownicy, wysokoscSzachownicy))
                {
                    WykonajRuch(nowaPozycja, szachownica, ref ruchyWykonane);

                    if (ruchyWykonane != szerokoscSzachownicy * wysokoscSzachownicy)
                    {
                        ruchUdany = ProbujRuchWieleRozwiazan(szachownica, nowaPozycja, ref ruchyWykonane, szerokoscSzachownicy, wysokoscSzachownicy, rozwiazania);

                        if (ruchUdany == false)
                            CofnijRuch(nowaPozycja, szachownica, ref ruchyWykonane);
                        else
                            return true;
                    }
                    else
                    {
                        rozwiazania.Add((int[,])szachownica.Clone());
                        CofnijRuch(nowaPozycja, szachownica, ref ruchyWykonane);
                        continue;
                    }
                }
            } while (true);

            return false;
        }

        private static bool ProbujRuch(int[,] szachownica, int[] pozycja, ref int ruchyWykonane, int szerokoscSzachownicy, int wysokoscSzachownicy) 
        {
            bool ruchUdany = true;
            Queue<int[]> listaRuchow;
            int[] nowaPozycja;

            listaRuchow = StworzListeRuchow(pozycja);
            do
            {
                if (listaRuchow.Count == 0)
                    break;

                nowaPozycja = listaRuchow.Dequeue();

                if (CzyRuchDopuszczalny(szachownica, nowaPozycja, szerokoscSzachownicy, wysokoscSzachownicy))
                {
                    WykonajRuch(nowaPozycja, szachownica, ref ruchyWykonane);

                    if (ruchyWykonane != szerokoscSzachownicy * wysokoscSzachownicy)
                    {
                        ruchUdany = ProbujRuch(szachownica, nowaPozycja, ref ruchyWykonane, szerokoscSzachownicy, wysokoscSzachownicy);

                        if (ruchUdany == false)
                            CofnijRuch(nowaPozycja, szachownica, ref ruchyWykonane);
                        else
                            return true;
                    }
                    else
                        return true; // Dotarcie do rozwiązania
                }
            } while (true);

            return false;
        }

        private static Queue<int[]> StworzListeRuchow(int[] pozycja)
        {
            Queue<int[]> listaRuchow = new Queue<int[]>();

            foreach (var ruch in listaRuchowSkoczka)
            {
                listaRuchow.Enqueue(new int[] 
                { 
                    pozycja[0] + ruch[0],
                    pozycja [1] + ruch[1]
                });
            }
            return listaRuchow;
        }

        private static bool CzyRuchDopuszczalny(int[,] szachownica, int[] nowaPozycja, int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            if (nowaPozycja[0] >= wysokoscSzachownicy || nowaPozycja[0] < 0)
                return false;

            if (nowaPozycja[1] >= szerokoscSzachownicy || nowaPozycja[1] < 0)
                return false;

            return szachownica[nowaPozycja[0], nowaPozycja[1]] == 0;
        }

        private static void WykonajRuch(int[] nowaPozycja, int[,] szachownica, ref int ruchyWykonane)
        {
            ruchyWykonane += 1;
            szachownica[nowaPozycja[0], nowaPozycja[1]] = ruchyWykonane;
        }

        private static bool CofnijRuch(int[] nowaPozycja, int[,] szachownica, ref int ruchyWykonane)
        {
            ruchyWykonane -= 1;
            szachownica[nowaPozycja[0], nowaPozycja[1]] = 0;

            return true;
        }

        private static void WykonajRuchWarnsdorff(int[] nowaPozycja, int[,] szachownica, ref int ruchyWykonane, int[,] tablicaMozliwosciRuchow, int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            ruchyWykonane += 1;
            szachownica[nowaPozycja[0], nowaPozycja[1]] = ruchyWykonane;
            //tablicaMozliwosciRuchow[nowaPozycja[0], nowaPozycja[1]] -= 0;

            var listaRuchowSkoczka = StworzListeRuchow(nowaPozycja);
            foreach (var r in listaRuchowSkoczka)
            {
                if (CzyRuchDopuszczalny(szachownica, new int[] { r[0], r[1] }, szerokoscSzachownicy, wysokoscSzachownicy))
                    tablicaMozliwosciRuchow[r[0], r[1]] -= 1;
            }
        }

        private static bool CofnijRuchWarnsdorff(int[] nowaPozycja, int[,] szachownica, ref int ruchyWykonane, int[,] tablicaMozliwosciRuchow, int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            ruchyWykonane -= 1;
            szachownica[nowaPozycja[0], nowaPozycja[1]] = 0;

            var listaRuchowSkoczka = StworzListeRuchow(nowaPozycja);
            foreach (var r in listaRuchowSkoczka)
            {
                if (CzyRuchDopuszczalny(szachownica, new int[] { r[0], r[1] }, szerokoscSzachownicy, wysokoscSzachownicy))
                    tablicaMozliwosciRuchow[r[0], r[1]] += 1;
            }

            return true;
        }

        public static void WypiszSzachownice(int[,] szachownica, int szerokoscSzachownicy, int wysokoscSzachownicy)
        {
            if (szachownica == null)
            {
                Console.Write("Brak szachownicy!");
                return;
            }

            for (int i = 0; i < wysokoscSzachownicy; i++)
            {
                for (int j = 0; j < szerokoscSzachownicy; j++)
                {
                    Console.Write("{0,2} ", szachownica[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;
                else
                    return result;
            }
        }
    }
}
