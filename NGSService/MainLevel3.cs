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
   public class MainLevel3
    {
       public List<Level3> ListLevel3 { get; set; }/* = new List<Level2>() { };*/
       public MainLevel3(string path) {
             ListLevel3 = Files.readTxtLevel3(path);
             InsertCalculationForPerson3();         
       }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson3()
        {

            foreach (var person in ListLevel3)
            {
                Level3 p = person;
                Thread thread1 = new Thread(() => new CalculationLevel3(p));
                thread1.Start();            
            }
        }
        public static void InsertAllListPerson(Level3 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 3, Person.Chrom, Person.Start, Person.End, Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, Person.InsertionsChrom, Person.InsertionsPos, Person.InsertionsId, Person.InsertionsRef, Person.InsertionsAlt, Person.ColorDyName,
               idRuns, connection, transaction);
        }


    }
}
