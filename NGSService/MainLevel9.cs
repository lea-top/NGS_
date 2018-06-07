using NGSData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NGSService
{
    public enum  ValueMutation { REP, POS, NEG, AFF }

    public class MainLevel9
    {
        public List<Level9> ListLevel9 { get; set; }
        public MainLevel9(Production p)
        {
            ListLevel9 = new List<Level9>();
            InsertCalculationForPerson6(p);
        }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson6(Production p)
        {
            p.ListLevel9 = Enumerable.Range(0, p.ListLevel6.Count)
            .Select(i =>
                new Level9()
                {
                    DyDis = p.ListLevel6[i].DyDis,
                    DyMut = p.ListLevel6[i].DyMut,
                    MutID = p.ListLevel6[i].MutID,
                    ColorDyName = p.ListLevel6[i].ColorDyName,
                    ListPerson = p.ListLevel6[i].ListPerson.Select(p1 =>
                       new PersonLevel9()
                       {
                           Name = p1.Name,
                           //Results = (p1.Color.TotalCoverage.Equals("Red") ? "REP" :
                           //                                    (p1.Color.Genotype.Equals("Blue") ? "POS" :
                           //                                    (p1.Color.Genotype.Equals("Bordeaux") ? "AFF" :
                           //                                    (p1.Color.Genotype.Equals("Green") ? "NEG" : "")))),
                           //Color = (p1.Color.TotalCoverage.Equals("Red") ? "Orange" : p1.Color.Genotype)
                           Results = WhichColorRed(p.ListLevel8, p1),
                           Color = IfColorRed(p.ListLevel8, p1.Name) ? ColorMutation.Orange.ToString() : (p1.Color.TotalCoverage.Equals(ColorMutation.Red.ToString()) ? ColorMutation.Orange.ToString() : p1.Color.Genotype)
                       }

                     ).ToList()
                })
               .ToList();
            SetAllColRed(p.ListLevel9);


        }
        private void SetAllColRed(List<Level9> l9)
        {
            var c = 0;
            for (int j = 0; j < l9[0].ListPerson.Count; j++)
            {
                c = 0;
                for (int i = 0; i < l9.Count; i++)
                {
                    if (l9[i].ListPerson[j].Color.Equals(ColorMutation.Orange.ToString()))
                        c++;
                }
                if ((c + 6) == l9.Count)
                {
                    l9.ForEach(l => { l.ListPerson[j].Color = ColorMutation.Orange.ToString(); l.ListPerson[j].Results = ValueMutation.REP.ToString(); });
                }
            }

        }
        private string WhichColorRed(List<Level0> l8, PersonLevel1 p1)
        {
            if (IfColorRed(l8, p1.Name))
                return ValueMutation.REP.ToString();
            if (p1.Color.TotalCoverage.Equals(ColorMutation.Red.ToString()))
                return ValueMutation.REP.ToString();
            else if (p1.Color.Genotype.Equals(ColorMutation.Blue.ToString()))
                return ValueMutation.POS.ToString(); 
            else if (p1.Color.Genotype.Equals(ColorMutation.Bordeaux.ToString()))
                return ValueMutation.AFF.ToString();
            else if (p1.Color.Genotype.Equals(ColorMutation.Green.ToString()))
                return ValueMutation.NEG.ToString();
            return "";
        }
        private bool IfColorRed(List<Level0> l8, string name)
        {
            if (!name.Equals("SUP-NGS1") && !name.Equals("SUP-NGS2"))
            {
                var ColorRed = l8.FirstOrDefault(cons => cons.SampleName.Equals(name));
                if (ColorRed != null)
                    if (ColorRed.NgUl <= 5) return true;
            }
            return false;
          
        }
        public static void InsertAllListPerson(Level9 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, Person.DyDis, Person.DyMut, Person.MutID, Person.ColorDyName, idRuns, connection, transaction);
        }
    }
}
