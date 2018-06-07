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
   public class MainLevel2
    {
        public List<Level2> ListLevel2 { get; set; }/* = new List<Level2>() { };*/
        public MainLevel2(string path)
        {
            ListLevel2 = Files.readTxtLevel2(path);
            InsertCalculationForPerson2();
            //InsertCalculationForPerson1();
        }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson2()
        {
            ListLevel2.ForEach(L2 => new CalculationLevel2(L2));
            //var item = CalculationLevel2.search(ListLevel2);
            //CalculationLevel2 c = new CalculationLevel2(ListLevel2[0]);
        }
        public static void InsertAllListPerson(Level2 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 2, "", 0, "", Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, "", 0, "", "", "", Person.ColorDyName,
               idRuns, connection, transaction);
        }

    }
}
