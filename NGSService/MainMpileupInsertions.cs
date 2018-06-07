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
   public class MainMpileupInsertions
    {
       public List<Level3> ListMpileupInsertions { get; set; }
       public MainMpileupInsertions(string path) {
            ListMpileupInsertions = Files.readTxtLevel3(path);
            InsertCalculationForPerson();         
       }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson()
        {
            var item = CalculationMpileupInsertions.searchTs(ListMpileupInsertions);
            CalculationMpileupInsertions c = new CalculationMpileupInsertions(item);
        }

        public static void InsertAllListPerson(Level3 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 55, Person.Chrom, Person.Start, Person.End, Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, Person.InsertionsChrom, Person.InsertionsPos, Person.InsertionsId, Person.InsertionsRef, Person.InsertionsAlt, Person.ColorDyName,
               idRuns, connection, transaction);
        }
    }
}
