using NGSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    //public enum sampleUnusual {SUP-NGS1,NTC1, SUP-NGS2,NTC2 };
    //public static <int,string> sampleUnusual() { }
    public class ConnectAccessLevel1
    {
        public static string connectionString = ConfigurationManager.AppSettings["connectionStringToAccess"];
        // @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Fluidigm database\Fluidigm.mdb";

        private static string strSQL;
        public ConnectAccessLevel1()
        {
        }

        public static List<Level0> SelectAllListPersonForIdFile(Run IdPlate)
        {
            int countP = 0;
            var lp = new List<Level0>();
            OleDbConnection con = null;
            OleDbDataReader Reader = null;
            try
            {
                con = new OleDbConnection(connectionString);
                con.Open();
                OleDbCommand cmd = null;
                foreach (var p in IdPlate.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any() && !p.Name.Equals("BarcodeCantrige") && !p.Name.Equals("NumRun")))
                {
                    InsertParameterForBarcodePlate(con, cmd, Reader, p.GetValue(IdPlate, null).ToString(), ++countP, lp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Reader != null) Reader.Close();
                if (con != null) con.Close();
            }
            return lp;
        }
        public static void InsertParameterForBarcodePlate(OleDbConnection con,OleDbCommand cmd ,OleDbDataReader Reader, string barcodeP,int countP, List<Level0> lp )
        {
            strSQL = "select * from Rikuz WHERE PlateBC=@IdPlate";
            cmd = new OleDbCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@IdPlate", barcodeP);
            Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                var l = new Level0();
                l.NumPlate = "P" + countP;
                l.IdPlate = Reader["PlateBC"].ToString();
                l.Or260280 = float.Parse(Reader["260/280"].ToString());
                l.Comments = Reader["Comments"].ToString();
                l.Type = Reader["Type"].ToString();
                l.IdPos = int.Parse(Reader["Position"].ToString());
                l.NgUl = float.Parse(Reader["ng/ul"].ToString());
                l.Vol = int.Parse(Reader["Vol"].ToString());
                l.SampleName = Reader["LabNum"].ToString().Equals("NTC") ? (countP == 1 ? "SUP-NGS1" : (countP == 2 ? "NTC1" : (countP == 3 ? "SUP-NGS2" : (countP == 4 ? "NTC2" :
                    throw new Exception("sample name is 'NTC' and has not name Plate")
                    )))) : Reader["LabNum"].ToString();
                l.Pos = Reader["Loc"].ToString();
                l.Id = long.Parse(Reader["ID"].ToString());

                lp.Add(l);
            }

        }

    }
}
