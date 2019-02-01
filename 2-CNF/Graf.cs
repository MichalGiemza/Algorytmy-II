using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_CNF
{
    public class Graf
    {
        protected bool[,] macierzKrawedzi;
        protected int iloscWierzcholkow;
        protected int[] tablicaCzasow = null;
        protected bool[] tablicaOdwiedzonych = null;

        public const int czasNieUstawiony = 0;

        public bool[,] MacierzKrawedzi { get { return (bool[,])macierzKrawedzi.Clone(); } }
        public int IloscWierzcholkow { get { return iloscWierzcholkow; } }
        public int[] TablicaCzasow { get { return (int[])tablicaCzasow.Clone(); } }
        public bool[] TablicaOdwiedzonych { get { return (bool[])tablicaOdwiedzonych.Clone(); } }

        public Graf(int iloscWierzcholkow)
        {
            this.iloscWierzcholkow = iloscWierzcholkow;
            macierzKrawedzi = new bool[iloscWierzcholkow, iloscWierzcholkow];
            tablicaOdwiedzonych = new bool[iloscWierzcholkow];
            tablicaCzasow = new int[iloscWierzcholkow];
            UsunKrawedzie();
        }
        public Graf(Graf g)
        {
            iloscWierzcholkow = g.iloscWierzcholkow;
            macierzKrawedzi = g.MacierzKrawedzi;
            tablicaCzasow = g.TablicaCzasow;
            tablicaOdwiedzonych = new bool[iloscWierzcholkow];
        }
        public void UsunKrawedzie()
        {
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                for (int j = 0; j < iloscWierzcholkow; j++)
                {
                    macierzKrawedzi[i, j] = false;
                }
            }
        }
        public virtual bool Krawedz(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            return macierzKrawedzi[wierzcholekStartowy, wierzcholekDocelowy];
        }
        private void UstawKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy, bool polacz)
        {
            macierzKrawedzi[wierzcholekStartowy, wierzcholekDocelowy] = polacz;
        }
        public void DodajKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            UstawKrawedzSkierowana(wierzcholekStartowy, wierzcholekDocelowy, true);
        }
        public void DodajKrawedzNieskierowana(int wierzcholek1, int wierzcholek2)
        {
            UstawKrawedzSkierowana(wierzcholek1, wierzcholek2, true);
            UstawKrawedzSkierowana(wierzcholek2, wierzcholek1, true);
        }
        public void UsunKrawedzSkierowana(int wierzcholekStartowy, int wierzcholekDocelowy)
        {
            UstawKrawedzSkierowana(wierzcholekStartowy, wierzcholekDocelowy, false);
        }
        public void UsunKrawedzNieskierowana(int wierzcholek1, int wierzcholek2)
        {
            UstawKrawedzSkierowana(wierzcholek1, wierzcholek2, false);
            UstawKrawedzSkierowana(wierzcholek2, wierzcholek1, false);
        }
        public void TransponujMacierzKrawedzi()
        {
            bool[,] nowaMacierzKrawedzi = new bool[iloscWierzcholkow, iloscWierzcholkow];

            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                for (int j = 0; j < iloscWierzcholkow; j++)
                {
                    nowaMacierzKrawedzi[i, j] = macierzKrawedzi[j, i];
                }
            }

            macierzKrawedzi = nowaMacierzKrawedzi;
        }
        public List<int> Sasiedzi(int wierzcholek)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                if (macierzKrawedzi[wierzcholek, i] == true)
                    result.Add(i);
            }
            return result;
        }
        public IList<int> Sasiedzi2(int wierzcholek)
        {
            SortedList<int, int> result = new SortedList<int, int>();
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                if (macierzKrawedzi[wierzcholek, i] == true)
                    result.Add(-tablicaCzasow[i], i);
            }
            return result.Values;
        }
        public void WyczyscTabliceOdwiedzonych()
        {
            for (int i = 0; i < iloscWierzcholkow; i++)
                tablicaOdwiedzonych[i] = false;
        }

        public int[] WyznaczSSS()
        {
            //Graf grafSSS = new Graf(iloscWierzcholkow);
            int[] SSS = new int[iloscWierzcholkow];
            bool[,] pierwotnaMacierzKrawedzi = macierzKrawedzi;

            WyczyscTabliceOdwiedzonych();
            DFS();

            TransponujMacierzKrawedzi();
            
            WyczyscTabliceOdwiedzonych();
            DFS2(SSS);

            macierzKrawedzi = pierwotnaMacierzKrawedzi;
            return SSS;
        }

        private void DFS()
        {
            int czas = 0;
            for (int i = 0; i < iloscWierzcholkow; i++)
            {
                if (tablicaOdwiedzonych[i] == true)
                    continue;

                // Czas zwiększa się przy wybraniu kolejnego węzła
                czas += 1;
                IdzWGlab(i, ref czas);
            }
        }
        private void DFS2(int[] SSS)
        {
            SortedList<int, int> result = new SortedList<int, int>();
            for (int i = 0; i < iloscWierzcholkow; i++)
                result.Add(-tablicaCzasow[i], i);

            int skladowa = 0;
            foreach (var i in result.Values)
            {
                if (tablicaOdwiedzonych[i] == true)
                    continue;
                
                IdzWGlab2(i, SSS, skladowa);
                skladowa += 1;
            }
        }
        private void IdzWGlab(int w, ref int czas)
        {
            tablicaCzasow[w] = czas;
            tablicaOdwiedzonych[w] = true;
            foreach (var s in Sasiedzi(w))
            {
                if (tablicaOdwiedzonych[s] == true)
                    continue;
                
                // Czas zwiększa się przy przeskoku
                czas += 1;
                IdzWGlab(s, ref czas);
            }

            // Czas zwiększa się przy powrocie
            czas += 1;
            tablicaCzasow[w] = czas;
        }
        private void IdzWGlab2(int w, int[] SSS, int skladowa)
        {
            SSS[w] = skladowa;

            tablicaOdwiedzonych[w] = true;
            foreach (var s in Sasiedzi2(w))
            {
                if (tablicaOdwiedzonych[s] == true)
                    continue;
                
                IdzWGlab2(s, SSS, skladowa);
            }
        }
    }
}
