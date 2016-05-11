
// based on http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace DBSCAN_GUI
{
    public class Algoritam
    {
        public const int NOISE = -1;//ako je C=-1 točka je šum
        public const int UNCLASSIFIED = 0;// ako je C=0 ne pripada nijednom klasteru
        public List<Tocka> D = new List<Tocka>();//D-dataset 
        public double eps;//epsilon radijus okoline
        public int MinPts;//minimalna zadana gustoća

        public List<List<Tocka>> klasteri;
        public String text;

        public void dodajTocku(int x, int y)//punjenje dataseta (liste)
        {
            D.Add(new Tocka(x, y));
        }

        public int dist(Tocka p, Tocka q)//Euklidska udaljenost
        {
            return (int)Math.Sqrt(Math.Pow(q.X - p.X, 2) + Math.Pow(q.Y - p.Y, 2));
        }

        public void pokreniAlgoritam(List<Tocka> D)
        {
            klasteri = DBSCAN(D, eps, MinPts);//lista točaka grupiranih u klastere bez šuma

            // **************** ispis dataseta u textbox
            text += "Total [" + D.Count + "] points:\n";
            foreach (Tocka p in D) text += p;


            // ************ ispis točaka prema klasterima u textbox
            int total = 0;
            for (int i = 0; i < klasteri.Count; i++)
            {
                int count = klasteri[i].Count;
                total += count;
                text += "\n\nCluster[" + (i + 1) + "] contains [" + count + "] points:\n";
                foreach (Tocka p in klasteri[i]) text += p;
            }

            //************** ispis šuma u textbox
            total = D.Count - total;
            if (total > 0)
            {
                text += "\n\nTotal [" + total + "] points that represent noise:\n";
                foreach (Tocka p in D)
                {
                    if (p.C == NOISE) text += p;
                }
            }
            else
            {
                text += "\n\nNo points that represent noise.\n";
            }

        }

        public List<List<Tocka>> DBSCAN(List<Tocka> D, double eps, int MinPts)
         //vraća sortiranu listu točaka grupiranih u klastere bez šuma
        {
            if (D == null) return null;

            List<List<Tocka>> klasteri = new List<List<Tocka>>();
            int C = 1;
            for (int i = 0; i < D.Count; i++)
             //markiramo sve točke dataseta oznakom pripadajućeg klastera/šuma
            {
                Tocka p = D[i];
                if (p.C == UNCLASSIFIED)
                    if (expandCluster(D, p, C, eps, MinPts)) C++;
            }

            
            int total = D.OrderBy(p => p.C).Last().C;
            // sortiramo dataset prema markerima
            //marker zadnjeg klastera u ascending sortiranoj listi
            //ujedno i ukupni broj klastera

            if (total < 1) return klasteri; //nema klastera

            for (int i = 0; i < total; i++) klasteri.Add(new List<Tocka>());
            //punimo svaku klaster listu točkama prema markeru

            foreach (Tocka p in D)
            {
                if (p.C > UNCLASSIFIED) klasteri[p.C - 1].Add(p);
            }
            return klasteri;
        }

        public bool expandCluster(List<Tocka> NeighborPts, Tocka p, int C, double eps, int MinPts)
        {
            List<Tocka> seeds = regionQuery(NeighborPts, p, eps);
            if (seeds.Count < MinPts) // p nije jezgrena točka
            {
                p.C = NOISE;
                return false;
            }
            else // sve točke u seeds su 'dohvatljive gustoćom' iz točke p
            {
                for (int i = 0; i < seeds.Count; i++) seeds[i].C = C;// markiramo tocke oznakom klastera
                seeds.Remove(p);//izbacimo početnu točku iz seed-a
                while (seeds.Count > 0)//provjeravamo okolinu ostalih točaka
                {
                    Tocka currentP = seeds[0];
                    List<Tocka> result = regionQuery(NeighborPts, currentP, eps);
                    if (result.Count >= MinPts)//provjeravamo gustoću nove okoline
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            Tocka resultP = result[i];//provjeravamo svaku točku nove okoline
                            if (resultP.C == UNCLASSIFIED || resultP.C == NOISE)
                            {
                                if (resultP.C == UNCLASSIFIED) seeds.Add(resultP);
                                //ako nije klasificirana dodajemo je klasteru
                                resultP.C = C;
                            }
                        }
                    }
                    seeds.Remove(currentP);//smanjujemo seed za 1
                }
                return true;
            }
        }

        public List<Tocka> regionQuery(List<Tocka> D, Tocka p, double eps)
         //provjerava pripadnost točke okolini
        {
            List<Tocka> okolina = new List<Tocka>();
            for (int i = 0; i < D.Count; i++)
            {
                if (dist(p, D[i]) <= eps) okolina.Add(D[i]);
                //ako je unutar radijusa, pripada okolini
            }
            return okolina;
        }
    }
}
