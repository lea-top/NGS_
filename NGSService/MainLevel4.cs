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
   public class MainLevel4
    {
       public List<Level3> ListLevel4 { get; set; }/* = new List<Level2>() { };*/
       public MainLevel4(string path) {
             ListLevel4 = Files.readTxtLevel3(path);
             InsertCalculationForPerson4();         
       }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson4()
        {
            var item = CalculationLevel4.searchCLT(ListLevel4);
            CalculationLevel4 c = new CalculationLevel4(item);

            item = CalculationLevel4.searchU2S(ListLevel4);
            c = new CalculationLevel4(item);
        }
        public static void InsertAllListPerson(Level3 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 4, Person.Chrom, Person.Start, Person.End, Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, Person.InsertionsChrom, Person.InsertionsPos, Person.InsertionsId, Person.InsertionsRef, Person.InsertionsAlt, Person.ColorDyName,
               idRuns, connection, transaction);
        }
    }
}
