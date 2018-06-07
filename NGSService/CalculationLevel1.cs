using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public enum ColorMutation { Grey, Green, Blue, Bordeaux, Red, Orange, Yellow, Pink }
    class CalculationLevel1
    {
        public static List<ColorMutation> Color { get; set; }
        public static Dictionary<int, List<string>> UnusualGenotypeResult = new Dictionary<int, List<string>>();
        public static Dictionary<int, List<string>> UnusualPink = new Dictionary<int, List<string>>();
        public CalculationLevel1(Level1 mutation)
        {
            //לבדוק אם כמו שלב 1 
            UnusualGenotypeResult = new Dictionary<int, List<string>>();
            UnusualGenotypeResult.Add(1, new List<string>() { "WFS", "R558C", "0167I" });
            UnusualGenotypeResult.Add(2, new List<string>() { "CN", "693", "0038U" });

            UnusualPink = new Dictionary<int, List<string>>();
            UnusualPink.Add(1, new List<string>() { "BL", "2281", "0054U" });//1
            UnusualPink.Add(2, new List<string>() { "POL", "3761INSG", "0225I" });
            UnusualPink.Add(3, new List<string>() { "GAL", "687INSCA", "0228I" });

            IfGenotypePosNotStart(mutation);
            WhichMutation(mutation);
        }

        public void IfGenotypePosNotStart(Level1 mutation)
        {
            if (mutation.GenotypePos != mutation.Start)

            { throw new Exception(mutation.Chrom + " " + mutation.Start + " " + mutation.End + " - in mutation START not worth to Genotype_POS ."); }

        }
        public static void WhichMutation(Level1 mutation)
        {
            int index = 1;
            if (mutation.DyDis.Equals(UnusualGenotypeResult[1][0]) && mutation.DyMut.Equals(UnusualGenotypeResult[1][1]) && mutation.MutID.Equals(UnusualGenotypeResult[1][2]))
            {
                index = mutation.GenotypeAlt.Split(',').ToList().IndexOf("T") + 1;
                mutation.ColorDyName = ColorMutation.Yellow.ToString();
            }
            else if (mutation.DyDis.Equals(UnusualGenotypeResult[2][0]) && mutation.DyMut.Equals(UnusualGenotypeResult[2][1]) && mutation.MutID.Equals(UnusualGenotypeResult[2][2]))
            {
                index = mutation.GenotypeAlt.Split(',').ToList().IndexOf("A") + 1;
                mutation.ColorDyName = ColorMutation.Yellow.ToString();
            }
            else if (mutation.GenotypeAlt.IndexOf(',') > -1)
            {
                index = mutation.GenotypeAlt.Split(',').ToList().IndexOf(mutation.Alt) + 1;
            }
            if (mutation.DyDis.Equals(UnusualPink[1][0]) && mutation.DyMut.Equals(UnusualPink[1][1]) && mutation.MutID.Equals(UnusualPink[1][2]) ||
                 mutation.DyDis.Equals(UnusualPink[2][0]) && mutation.DyMut.Equals(UnusualPink[2][1]) && mutation.MutID.Equals(UnusualPink[2][2]) ||
                 mutation.DyDis.Equals(UnusualPink[3][0]) && mutation.DyMut.Equals(UnusualPink[3][1]) && mutation.MutID.Equals(UnusualPink[3][2])) {
                 mutation.ColorDyName = ColorMutation.Pink.ToString();
            }

            SetColorInListPerson(mutation.ListPerson, index);

        }
        public static void SetColorInListPerson(List<PersonLevel1> listPerson, int index)
        {
            foreach (var person in listPerson)
            {
                WhichColor(person, index);
                IfDyNameTotalCoverage(person);
            }
        }

        public static void WhichColor(PersonLevel1 person, int index)
        {
            if (person.Genotype.Equals("./."))
                person.Color.Genotype = ColorMutation.Grey.ToString();
            else
            {
                var genotype = person.Genotype.Split('/');
                if (int.Parse(genotype[0]) == index && int.Parse(genotype[1]) == index)
                    person.Color.Genotype = ColorMutation.Bordeaux.ToString();
                else if (int.Parse(genotype[0]) == index || int.Parse(genotype[1]) == index)
                    person.Color.Genotype = ColorMutation.Blue.ToString();
                else person.Color.Genotype = ColorMutation.Green.ToString();
            }
        }

        public static void IfDyNameTotalCoverage(PersonLevel1 person)
        {
            if (person.TotalCoverage != null && !person.TotalCoverage.Equals("."))
            {
                if (int.Parse(person.TotalCoverage) < 50)
                {
                    person.Color.TotalCoverage = ColorMutation.Red.ToString();
                }
            }
        }
        //public static void IfDyNameAlleleCoverage(PersonLevel1 person)
        //{
        //    if (person.AlleleCoverage != null)
        //    {
        //        if (!person.AlleleCoverage.Equals('0'))
        //        {
        //            person.Color.Genotype = "Blue";
        //        }
        //    }
    }
}
