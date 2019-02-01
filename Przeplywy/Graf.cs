using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przeplywy
{
    public class Graf
    {
        protected int[,] macierzMaxPrzeplywu;
        protected int iloscWierzcholkow;
        protected bool[] tablicaOdwiedzonych = null;

        public const int czasNieUstawiony = 0;

        public int[,] MacierzMaxPrzeplywu { get { return (int[,])macierzMaxPrzeplywu.Clone(); } }
        public int IloscWierzcholkow { get { return iloscWierzcholkow; } }
        public bool[] TablicaOdwiedzonych { get { return (bool[])tablicaOdwiedzonych.Clone(); } }

        public Graf(int iloscWierzcholkow)
        {
            this.iloscWierzcholkow = iloscWierzcholkow;
            macierzMaxPrzeplywu = new int[iloscWierzcholkow, iloscWierzcholkow];
            tablicaOdwiedzonych = new bool[iloscWierzcholkow];
        }
        public Graf(Graf g)
        {
            iloscWierzcholkow = g.iloscWierzcholkow;
            macierzMaxPrzeplywu = g.MacierzMaxPrzeplywu;
            tablicaOdwiedzonych = new bool[iloscWierzcholkow];
        }
        public void UsunKrawedzie()
        {
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                for (int j = 0; j < iloscWierzcholkow; j++)
                {
                    macierzMaxPrzeplywu[i, j] = 0;
                }
            }
        }
        public virtual int Przeplyw(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            return macierzMaxPrzeplywu[wierzcholekStartowy, wierzcholekDocelowy];
        }
        private void UstawKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy, int przeplyw)
        {
            macierzMaxPrzeplywu[wierzcholekStartowy, wierzcholekDocelowy] = przeplyw;
        }
        public void DodajKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy, int przeplyw)
        {
            UstawKrawedzSkierowana(wierzcholekStartowy, wierzcholekDocelowy, przeplyw);
        }
        public void DodajKrawedzNieskierowana(int wierzcholek1, int wierzcholek2, int przeplyw)
        {
            UstawKrawedzSkierowana(wierzcholek1, wierzcholek2, przeplyw);
            UstawKrawedzSkierowana(wierzcholek2, wierzcholek1, przeplyw);
        }
        public void UsunKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            UstawKrawedzSkierowana(wierzcholekStartowy, wierzcholekDocelowy, 0);
        }
        public void UsunKrawedzNieskierowana(int wierzcholek1, int wierzcholek2)
        {
            UstawKrawedzSkierowana(wierzcholek1, wierzcholek2, 0);
            UstawKrawedzSkierowana(wierzcholek2, wierzcholek1, 0);
        }
        public List<int> Sasiedzi(int wierzcholek)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                if (macierzMaxPrzeplywu[wierzcholek, i] > 0)
                    result.Add(i);
            }
            return result;
        }
        public void WyczyscTabliceOdwiedzonych()
        {
            for (int i = 0; i < iloscWierzcholkow; i++)
                tablicaOdwiedzonych[i] = false;
        }

        public KeyValuePair<int, int[,]> MaksymalnyPrzeplyw_FordFulkerson(int s, int k)
        {
            int[,] aktualnaMacierzPrzeplywu = new int[iloscWierzcholkow, iloscWierzcholkow];
            int przeplywSciezki = int.MaxValue;
            int przeplyw = 0;

            while (true)
            {
                var sciezka = SciezkaPowiekszajaca(s, k, aktualnaMacierzPrzeplywu, ref przeplywSciezki);
                if (sciezka == null)
                    break;
                
                // Scieżka składa się z wierzchołka poprzedniego 'p' oraz obecnego 'w'.
                var e = sciezka.GetEnumerator();
                e.MoveNext();
                int p = e.Current;
                int w;
                while (e.MoveNext())
                {
                    w = e.Current;

                    // Zaktualizowanie przepływu
                    aktualnaMacierzPrzeplywu[w, p] += przeplywSciezki;
                    aktualnaMacierzPrzeplywu[p, w] -= przeplywSciezki;

                    p = w;
                }

                przeplyw += przeplywSciezki;
            }
            return new KeyValuePair<int, int[,]>(przeplyw, aktualnaMacierzPrzeplywu);
        }
        private List<int> SciezkaPowiekszajaca(int s, int k, int[,] aktualnaMacierzPrzeplywu, ref int przeplywSciezki)
        {
            List<int> sciezka = new List<int>();
            przeplywSciezki = int.MaxValue;

            bool znaleziono = IdzWGlab(s, k, aktualnaMacierzPrzeplywu, sciezka, ref przeplywSciezki);
            WyczyscTabliceOdwiedzonych();

            if (znaleziono)
                return sciezka;
            else
                return null;
        }
        private bool IdzWGlab(int w, int k, int[,] przeplyw, List<int> sciezka, ref int przeplywSciezki)
        {
            if (w == k)
            {
                sciezka.Add(w);
                return true;
            }

            tablicaOdwiedzonych[w] = true;
            foreach (var s in Sasiedzi(w))
            {
                if (tablicaOdwiedzonych[s] == true || przeplyw[w, s] == macierzMaxPrzeplywu[w, s])
                    continue;
                
                if (IdzWGlab(s, k, przeplyw, sciezka, ref przeplywSciezki))
                {
                    sciezka.Add(w);
                    przeplywSciezki = Math.Min(przeplywSciezki, macierzMaxPrzeplywu[w, s]);
                    return true;
                }
            }
            return false;
        }
    }
}
