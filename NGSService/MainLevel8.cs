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
   public class MainLevel8
    {
       public List<Level0> ListLevel8 { get; set; }
       public MainLevel8(Run idPlates) {

            ListLevel8 = new List<Level0>();
           // SelectFromSqlListPersonConcentration(idPlates);         
       }
        //// הוספת השינויים האותומטיים לכל אדם
        //private void SelectFromSqlListPersonConcentration(Run idPlates)
        //{
        //    ListLevel8 = new List<Level0>();
        //    foreach (var idP in idPlates.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any() && !p.Name.Equals("BarcodeCantrige") && !p.Name.Equals("NumRun")))
        //    {
        //        ListLevel8.AddRange(ConnectSqlLevel0.SelectAllListPersonForIdFile(idP.GetValue(idPlates, null).ToString()));
        //    }
        //    //idPlates.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any() && !p.Name.Equals("BarcodeCantrige")).ToList().ForEach(idP =>
        //    //{
        //    //    ListLevel8.AddRange(ConnectSqlLevel0.SelectAllListPersonForIdFile(idP.GetValue(idPlates, null).ToString()));
        //    //});

        //}
    }
}
