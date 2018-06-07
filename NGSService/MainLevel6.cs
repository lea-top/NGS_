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
   public class MainLevel6
    {
       public List<Level1> ListLevel6 { get; set; }
       public MainLevel6(Production p) {
            // p.ListLevel6 = new List<Level1>(p.ListLevel1);
            ListLevel6 = new List<Level1>();
            InsertCalculationForPerson6(p);         
       }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson6(Production p)
        {
            p.ListLevel6 = Enumerable.Range(0, p.ListLevel1.Count)
            .Select(i => new Level1() {
                Chrom = p.ListLevel1[i].Chrom,
                Start = p.ListLevel1[i].Start,
                End = p.ListLevel1[i].End,
                Id = p.ListLevel1[i].Id,
                Ref = p.ListLevel1[i].Ref,
                Alt = p.ListLevel1[i].Alt,
                DyDis = p.ListLevel1[i].DyDis,
                DyMut = p.ListLevel1[i].DyMut,
                MutID = p.ListLevel1[i].MutID,
                GenotypeChrom = p.ListLevel1[i].GenotypeChrom,
                GenotypePos = p.ListLevel1[i].GenotypePos,
                GenotypeId = p.ListLevel1[i].GenotypeId,
                GenotypeRef = p.ListLevel1[i].GenotypeRef,
                GenotypeAlt = p.ListLevel1[i].GenotypeAlt,
                ColorDyName = p.ListLevel1[i].ColorDyName,
                ListPerson = p.ListLevel1[i].ListPerson.Select(p1 => new PersonLevel1()
                {
                    Name = p1.Name,
                    Genotype = p1.Genotype,
                    AlleleCoverage = p1.AlleleCoverage,
                    TotalCoverage = p1.TotalCoverage,
                    Color =
                new ColorLevel1 { Genotype = p1.Color.Genotype, AlleleCoverage = p1.Color.AlleleCoverage, TotalCoverage = p1.Color.TotalCoverage }
                }).ToList() 
            })
            .ToList();

            CalculationLevel6 c = new CalculationLevel6();
            CalculationLevel6.Change(p);

        }
        public static void InsertAllListPerson(Level1 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 6, Person.Chrom, Person.Start, Person.End, Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, Person.GenotypeChrom, Person.GenotypePos, Person.GenotypeId, Person.GenotypeRef, Person.GenotypeAlt, Person.ColorDyName,
               idRuns, connection, transaction);
        }
    }
}
