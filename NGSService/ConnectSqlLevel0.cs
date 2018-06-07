using NGSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class ConnectSqlLevel0
    {
        public static string connectionString = ConfigurationManager.AppSettings["connectionString"];
        public static string SamplesNotInStep1 { get; set; }
        public ConnectSqlLevel0()
        {
        }

        public static string SelectTemplateToDataTable(List<Level0> listPerson, string numRuns)/*,OleDbTransaction transaction*/
        {
            string path = "";
            try
            {
                System.Data.DataTable sheet1 = new System.Data.DataTable("Excel Sheet");
                var strConnXLS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;" +
                             "Data Source={0};Extended Properties=Text;", ConfigurationManager.AppSettings["PathTamplatefile"]);
                string selectSql = @"select * from [Example_SampleSheet_MiSeq_384samples.csv] ";/*Example_SampleSheet_MiSeq_384sa$*/
                using (OleDbConnection connection = new OleDbConnection(strConnXLS))/* csbuilder.ConnectionString*/
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, connection))
                {
                    connection.Open();
                    adapter.Fill(sheet1);

                    sheet1.Rows[1][1] = DateTime.Now.ToString("dd/MM/yyyy");
                    sheet1.Rows[5][1] = "run-" + numRuns;
                    foreach (var p in listPerson)//הכנסת הנתונים
                    {
                        //p.Pos = p.Pos.Replace("\t", "");
                        string pos = "", sampleName = "";
                        if (p.Pos.Length == 2)
                            pos = p.Pos.Substring(0, 1) + '0' + p.Pos.Substring(1, 1);
                        else pos = p.Pos;
                        sampleName = p.SampleName.Replace(" ", "s").Replace(".", "a").Replace("]", "y").Replace("[", "x");
                        foreach (DataRow row in sheet1.Rows)
                        {
                            if (row[3].Equals(pos) && row[2].Equals(p.NumPlate))
                            {
                                row[0] = sampleName;
                                row[1] = sampleName;
                                break;
                            }
                        }
                    }
                    //foreach (DataRow row in dt.Rows)//בדיקה אם יש שורות ריקות
                    //{
                    //    if (row["Pos"].ToString() != null && row["SampleName"] == null && row["Col3"].ToString() != "96-Well"
                    //        || row["Pos"].ToString() != null && row["SampleName"].ToString() == "" && row["Col3"].ToString() != "96-Well")
                    //    {
                    //        row.Delete();
                    //    }
                    //}
                }
                path = WriteTemplateToFile(sheet1, numRuns);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // if (con != null) con.Close();
            }
            return path;
        }


        public static string WriteTemplateToFile(DataTable submittedDataTable, string numRuns)
        {
            var path = ConfigurationManager.AppSettings["PathTamplatefile"];
            var fullPath = path + Guid.NewGuid().ToString() + "-" + DateTime.Now.Ticks + "-" + numRuns + "-finalStep1.csv";
            try
            {
                StringBuilder sb = new StringBuilder();
                //string[] columnNames = submittedDataTable.Columns.Cast<DataColumn>().
                //                                  Select(column => column.).
                //     ToArray();
                //sb.AppendLine(string.Join(",", ""));
                sb.AppendLine("[Header],,,,,,,");
                foreach (DataRow row in submittedDataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }
                File.WriteAllText(fullPath, sb.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // if (con != null) con.Close();
            }
            return fullPath;
        }
        public static string InsertIdCantrigeAndSelectRuns(string idCantrige)/*, SqlTransaction transaction*/
        {

            long idNumberingRuns = 0;
            String query = "INSERT INTO dbo.NumberingRuns(IdCantrige) " +
                                  "VALUES (@idCantrige);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@idCantrige", idCantrige);

                var result = command.ExecuteNonQuery();
                command.CommandText = "SELECT @@IDENTITY";// SCOPE_IDENTITY()
                idNumberingRuns = long.Parse(command.ExecuteScalar().ToString());
                //    InsertOneRowListPerson(Person.ListPerson, IdList, connection, transaction);

                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }

            return idNumberingRuns.ToString();
        }
        public static void InsertAllListPerson0(Level0 Person, string idRun, SqlConnection connection, SqlTransaction transaction)
        {

            String query = "INSERT INTO dbo.Level0(IdRun,IdPlate,NumPlate,SampleName,Pos,Id,Or260280,Type1,IdPos,NgUl,Vol,Comments) " +
                                  "VALUES (@IdRun,@IdPlate,@NumPlate,@SampleName,@Pos,@Id,@Or260280,@Type,@IdPos,@NgUl,@Vol,@Comments);";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@IdRun", idRun);
                command.Parameters.AddWithValue("@IdPlate", Person.IdPlate);
                command.Parameters.AddWithValue("@NumPlate", Person.NumPlate);
                command.Parameters.AddWithValue("@SampleName", Person.SampleName);
                command.Parameters.AddWithValue("@Pos", Person.Pos);
                command.Parameters.AddWithValue("@Id", Person.Id);
                command.Parameters.AddWithValue("@Or260280", Person.Or260280);
                command.Parameters.AddWithValue("@Type", Person.Type);
                command.Parameters.AddWithValue("@IdPos", Person.IdPos);
                command.Parameters.AddWithValue("@NgUl", Person.NgUl);
                command.Parameters.AddWithValue("@Vol", Person.Vol);
                command.Parameters.AddWithValue("@Comments", Person.Comments);
                var result = command.ExecuteNonQuery();
                //command.CommandText = "SELECT @@IDENTITY";// SCOPE_IDENTITY()
                //var IdList = long.Parse(command.ExecuteScalar().ToString());
                //InsertOneRowListPerson(Person.ListPerson, IdList, connection, transaction);

                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }

        }
        public static void InsertToRuns( Run run, SqlConnection connection, SqlTransaction transaction)
        {

            String query = "INSERT INTO dbo.Runs(IdCantrige,IdRun,IdPlate1,IdPlate2,IdPlate3,IdPlate4) " +
                                  "VALUES (@IdCantrige,@IdRun,@IdPlate1,@IdPlate2,@IdPlate3,@IdPlate4);";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@IdCantrige",run.BarcodeCantrige);
                command.Parameters.AddWithValue("@IdRun",    run.NumRun);
                command.Parameters.AddWithValue("@IdPlate1", run.BarcodeP1);
                command.Parameters.AddWithValue("@IdPlate2", run.BarcodeP2);
                command.Parameters.AddWithValue("@IdPlate3", run.BarcodeP3);
                command.Parameters.AddWithValue("@IdPlate4", run.BarcodeP4);
                var result = command.ExecuteNonQuery();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }

        }
        public static List<Level0> SelectAllListPersonForIdFile(string idPlate)
        {
            var lp = new List<Level0>();
            SqlConnection con = null;
            SqlDataReader Reader = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = null;
                var strSQL = "select * from dbo.Level0 WHERE IdPlate=@IdPlate";
                cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@IdPlate", idPlate);
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                    var p = new Level0();
                    p.Id = long.Parse(Reader["Id"].ToString());
                    p.IdPlate = Reader["IdPlate"].ToString();
                    p.NumPlate = Reader["NumPlate"].ToString();
                    p.SampleName = Reader["SampleName"].ToString();
                    p.Pos = Reader["Pos"].ToString();
                    p.Or260280 = float.Parse(Reader["Or260280"].ToString());
                    p.NgUl = float.Parse(Reader["NgUl"].ToString());
                    p.Vol = int.Parse(Reader["Vol"].ToString());
                    p.Type = Reader["Type1"].ToString();
                    p.Comments = Reader["Comments"].ToString();
                    p.IdPos = int.Parse(Reader["IdPos"].ToString());
                    lp.Add(p);
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
        public static Run SelectRuns(string barcodePlate)
        {
            int c = 0;
            var r = new Run();
            SqlConnection con = null;
            SqlDataReader Reader = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = null;
                //var strSQL = "select * from dbo.Runs WHERE IdCantrige=@BarcodeCantrige";
                var strSQL = "select * from dbo.Runs WHERE IdPlate1=@barcodePlate";
                cmd = new SqlCommand(strSQL, con);
                //cmd.Parameters.AddWithValue("@BarcodeCantrige", BarcodeCantrige);
                cmd.Parameters.AddWithValue("@barcodePlate", barcodePlate);
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {
                   r.NumRun =          Reader["IdRun"].ToString();
                   r.BarcodeCantrige = Reader["IdCantrige"].ToString();
                   r.BarcodeP1 =       Reader["IdPlate1"].ToString();
                   r.BarcodeP2 =       Reader["IdPlate2"].ToString();
                   r.BarcodeP3 =       Reader["IdPlate3"].ToString();
                   r.BarcodeP4 =       Reader["IdPlate4"].ToString();
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
            return r;
        }

        public static string SelectBarcodePlateOne(int IdPos, string NumPlate,string sampleName, string strSQL)
        {
            SqlConnection con = null;
            SqlDataReader Reader = null;
            SqlCommand cmd = null;
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@NumPlate", NumPlate);
                cmd.Parameters.AddWithValue("@IdPos", IdPos);
                cmd.Parameters.AddWithValue("@SampleName", sampleName);

                Reader = cmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        return Reader["IdPlate"].ToString();
                    }
                }
                return null;
            }
            finally
            {
                if (Reader != null) Reader.Close();
                if (con != null) con.Close();
            }
        }
        public static List<Level0> SelectIfSampleinLevel0( string IdCantrige, List<PersonLevel1> list1)
        {
            SqlConnection con = null;
            var lp = new List<Level0>();
            SqlDataReader Reader = null;
            SqlCommand cmd = null;
            try
            {
                var strSQL = "select * from dbo.Level0 WHERE SampleName=@SampleName and IdRun=@IdCantrige";
                con = new SqlConnection(connectionString);
                con.Open();
               
                foreach (var sample in list1)
                {
                    cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@IdCantrige", IdCantrige);
                    cmd.Parameters.AddWithValue("@SampleName", sample.Name);
                    Reader = cmd.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var p = new Level0();
                            p.Id = long.Parse(Reader["Id"].ToString());
                            p.IdPlate = Reader["IdPlate"].ToString();
                            p.NumPlate = Reader["NumPlate"].ToString();
                            p.SampleName = Reader["SampleName"].ToString();
                            p.Pos = Reader["Pos"].ToString();
                            p.Or260280 = float.Parse(Reader["Or260280"].ToString());
                            p.NgUl = float.Parse(Reader["NgUl"].ToString());
                            p.Vol = int.Parse(Reader["Vol"].ToString());
                            p.Type = Reader["Type1"].ToString();
                            p.Comments = Reader["Comments"].ToString();
                            p.IdPos = int.Parse(Reader["IdPos"].ToString());
                            lp.Add(p);
                            //  return Reader["SampleName"].ToString();
                        }
                        //cmd.Clone();
                        //Reader.Close();
                    }
                    else { SamplesNotInStep1 += sample.Name + " , "; }
                    Reader.Close();
                    //else { throw new Exception("the samples : " + sample.Name +" , " + "not in step 1 "); }
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
    }
}
