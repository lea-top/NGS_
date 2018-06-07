using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationMpileupInsertions
    {
        public CalculationMpileupInsertions(Level3 mutation)
        {
            IfNeedColorListPerson4(mutation);
        }
        public static Level3 searchTs(List<Level3> mutations)
        {
            foreach (var item in mutations)
            {
                if (item.DyDis.Equals("TS") && item.DyMut.Equals("1277") && item.MutID.Equals("0044U") && item.Alt.Equals("GGATA") && item.Ref.Equals("G") && item.Chrom.Equals("chr15")
                    && item.Start.Equals(72346579) && item.End.Equals("72346579") && item.Id.Equals("rs387906309") && item.InsertionsAlt.Contains("GGATA") && item.InsertionsRef.Equals("G")
                    && item.InsertionsPos.Equals(72346579) && item.InsertionsChrom.Equals("chr15"))
                    return item;
            }
            return null;

        }     
        public void IfNeedColorListPerson4(Level3 mutation)
        {
            foreach (var person in mutation.ListPerson)
            {
                WhichColor(person);
            }
        }
        public static void WhichColor(PersonLevel1 person)
        {
            if (person.Genotype.Equals("."))
                person.Color.Genotype = ColorMutation.Grey.ToString();
            else
            {
                var genotype = person.Genotype.Split(',');
                if (int.Parse(genotype[0]) == 255 )
                    person.Color.Genotype = ColorMutation.Blue.ToString();
                else if (int.Parse(genotype[0]) == 0 )
                     person.Color.Genotype = ColorMutation.Green.ToString();
            }
        }
    }
}
