using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmKruskala
{
    public class Graf
    {
        protected int iloscWierzcholkow;
        protected int[] tablicaPolaczen; // Wartość dodatnia -> identyfikator wierzchołka korzenia/wskazującego na korzeń/...; wartość ujemna -> -(ilość wierzchołków w drzewie) (wierzchołek z wart. ujemną jest korzeniem);
        protected int[,] macierzWag = null;

        public int IloscWierzcholkow { get { return iloscWierzcholkow; } }
        public int[,] MacierzWag { get { return macierzWag != null ? (int[,])macierzWag.Clone() : null; } }

        public Graf(int iloscWierzcholkow)
        {
            this.iloscWierzcholkow = iloscWierzcholkow;
            WyczyscGraf();
        }
        
        public virtual bool KrawedzIstnieje(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            return macierzWag[wierzcholekStartowy, wierzcholekDocelowy] != int.MaxValue;
        }
        private void UstawKrawedz(int wierzcholekStartowy, int wierzcholekDocelowy, int waga)
        {
            macierzWag[wierzcholekStartowy, wierzcholekDocelowy] = waga;
        }
        public void DodajKrawedzNieskierowana(int wierzcholek1, int wierzcholek2, int waga = 0)
        {
            UstawKrawedz(wierzcholek1, wierzcholek2, waga);
            UstawKrawedz(wierzcholek2, wierzcholek1, waga);
        }
        public void UsunKrawedzNieskierowana(int wierzcholek1, int wierzcholek2)
        {
            UstawKrawedz(wierzcholek1, wierzcholek2, int.MaxValue);
            UstawKrawedz(wierzcholek2, wierzcholek1, int.MaxValue);
        }

        public void WyczyscGraf()
        {
            if (macierzWag == null)
                macierzWag = new int[iloscWierzcholkow, iloscWierzcholkow];

            // Wagi
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                for (int j = 0; j < iloscWierzcholkow; j++)
                {
                    macierzWag[i, j] = int.MaxValue;
                }
            }

            // Tablica polaczen
            tablicaPolaczen = new int[iloscWierzcholkow];
            for (int i = 0; i < iloscWierzcholkow; i++)
                tablicaPolaczen[i] = -1;
        }

        public Graf MinDrzewoRozp_Kruskal()
        {
            Graf mst = new Graf(iloscWierzcholkow);


            // Sortowanie krawedzi
            List<Pair<int, int[]>> listaKrawedzi = new List<Pair<int, int[]>>();
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                for (int j = i + 1; j < iloscWierzcholkow; j++)
                {
                    if (macierzWag[i, j] != int.MaxValue)
                        listaKrawedzi.Add(new Pair<int, int[]>(macierzWag[i, j], new int[] { i, j }));
                }
            }
            listaKrawedzi.Sort();

            // Iterowanie po krawedziach
            foreach (var p in listaKrawedzi)
            {
                var k = p.Value;
                if (mst.CzyIstniejeSciezka(k[0], k[1]))
                    continue;
                else
                {
                    mst.DodajKrawedzNieskierowana(k[0], k[1], macierzWag[k[0], k[1]]);
                    mst.Polacz(k[0], k[1]);
                }
            }

            return mst;
        }

        private int ZnajdzKorzen(int w, bool kompresuj = true)
        {
            int wk = w;
            while (tablicaPolaczen[wk] > 0)
                wk = tablicaPolaczen[wk];

            // Kompresja sciezki
            if (kompresuj)
            {
                int nast = w;
                while (tablicaPolaczen[nast] > 0)
                {
                    nast = tablicaPolaczen[w];
                    tablicaPolaczen[w] = wk;
                }
            }
            
            return wk;
        }
        private bool CzyIstniejeSciezka(int w1, int w2)
        {
            // Szukanie korzeni
            w1 = ZnajdzKorzen(w1);
            w2 = ZnajdzKorzen(w2);

            // Jeśli korzeń jest ten sam dla obu wierzchołków, ścieżka istnieje
            return w1 == w2;
        }
        private void Polacz(int w1, int w2)
        {
            // Szukanie korzeni
            w1 = ZnajdzKorzen(w1, false);
            w2 = ZnajdzKorzen(w2, false);

            if (w1 == w2)
                return;

            if (tablicaPolaczen[w1] < tablicaPolaczen[w2]) // Porównanie (zapisanych za pomocą wartości ujemnej) ilości wierzchołków w drzewach
            {
                int tmp = w1;
                w1 = w2;
                w2 = tmp;
            }

            // korzeń w1 jest teraz korzeniem mniejszego drzewa
            tablicaPolaczen[w2] += tablicaPolaczen[w1];
            tablicaPolaczen[w1] = w2;
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

        public class Pair<TKey, TValue> : IComparable where TKey : IComparable
        {
            TKey key;
            TValue value;

            public TKey Key { get { return key; } set { key = value; } }
            public TValue Value { get { return value; } set { this.value = value; } }

            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
            
            public int CompareTo(object obj)
            {
                return key.CompareTo(((Pair<TKey, TValue>)obj).Key);
            }
        }

        //void Sortuj(List<KeyValuePair<int, int[]>> list, int start, int koniec)
        //{
        //    if (start < koniec)
        //    {
        //
        //    }
        //}
    }
}
