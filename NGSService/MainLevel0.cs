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
    public class MainLevel0
    {
        public List<Level0> ListLevel0 { get; set; }
        //public String IdNumberingRuns { get; set; }
        public MainLevel0(Run IdPlate)
        {
            ListLevel0 = ConnectAccessLevel1.SelectAllListPersonForIdFile(IdPlate);
            //IdNumberingRuns = ConnectSqlLevel0.InsertIdCantrigeAndSelectRuns(IdPlate.BarcodeCantrige);
            //IdNumberingRuns = IdNumberingRuns.ToString().PadLeft(5, '0');

            InsertCalculationForPerson0();
            InsertIntoDatabaseListPerson(ListLevel0, IdPlate);
        }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson0()
        {

            var duplicates = ListLevel0
                            .GroupBy(i => i.SampleName)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key);

            duplicates.ToList().ForEach(d =>
                    ListLevel0.ForEach(l => { if (l.SampleName.Equals(d)) l.SampleName = l.SampleName + ((char)(int.Parse(l.NumPlate[1].ToString()) - 1 + 'A' )).ToString().ToUpper(); }
            ));

            ListLevel0.ForEach(p => p.Pos = p.Pos.Replace("\t", ""));
        }

        //להכניס נתונים לטבלה
        public static void InsertIntoDatabaseListPerson(List<Level0> list1, Run IdPlate)
        {
            SqlTransaction transaction;
            using (SqlConnection connection = new SqlConnection(ConnectSqlLevel1.connectionString))
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    list1.ForEach(p1 => ConnectSqlLevel0.InsertAllListPerson0(p1, IdPlate.BarcodeCantrige, connection, transaction));

                    ConnectSqlLevel0.InsertToRuns(IdPlate, connection,transaction);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
                connection.Close();
            }
        }

    }
}
