using NGSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class ConnectSqlLevel1
    {
        public static string connectionString = ConfigurationManager.AppSettings["connectionString"];
        //@"Data Source=.\SQLEXPRESS;Initial Catalog = NGSData; Integrated Security = True";
        //User ID=NT AUTHORITY\IUSR; @"Data Source=DEVORAK2018\SQLEXPRESS;Initial Catalog = NGSData; Integrated Security = SSPI;User ID=DYL\Devorak; Password=NULL";
        //User ID=DYL\DevorakAuthentication=Active Directory Integrated Data Source=DEVORAK2018;Database=NGSData; Initial Catalog=True 259;
        private static string strSQL;
        //@"Data Source=DEVORAK2018\SQLEXPRESS;Initial Catalog=NGSData;Integrated Security=true";
        public static string SelectPathFile(int idPath)
        {
            SqlConnection con = null;
            SqlDataReader Reader = null;
            string path="";
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = null;
                var strSQL = "select NamePath from dbo.PathFiles WHERE Id=@idPath";
                cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@idPath", idPath);
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                {

                   path = Reader["NamePath"].ToString();
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
            return path;
        }
        public static void DeletePathFile(int idPath)
        {
            SqlConnection con = null;
            SqlDataReader Reader = null;
            string path = "";
            try
            {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = null;
                var strSQL = "Delete from dbo.PathFiles WHERE Id=@idPath";
                cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@idPath", idPath);
                cmd.ExecuteNonQuery();
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
        }
        public static long InsertPathFile(string path)
        {
            long idPath;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String query = "INSERT INTO dbo.PathFiles(NamePath) " +
                "VALUES (@path);";
            /*SELECT SCOPE_IDENTITY();*/
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@path", path);
                connection.Open();
                var result = command.ExecuteNonQuery();
                command.CommandText = "SELECT @@IDENTITY";// SCOPE_IDENTITY()
                idPath = long.Parse(command.ExecuteScalar().ToString());
                if (result< 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
                connection.Close();
            }
            return idPath;
        }
      
        //public static void InsertAllListPerson(Level1 Person,string idRuns, SqlConnection connection, SqlTransaction transaction)
        //{

        //    //using (SqlConnection connection = new SqlConnection(connectionString))
        //    //{
        //    String query = "BEGIN IF NOT EXISTS(SELECT * FROM Mutations WHERE Mutations.NumFile = @NumFile and Mutations.Start1 = @Start and Mutations.Chrom = @Chrom and Mutations.End1 = @End)" +
        //    "BEGIN INSERT INTO dbo.Mutations(NumFile,Chrom,Start1,End1,Id,Ref,Alt,DyDis,DyMut,MutID,GenotypeChrom,GenotypePos,GenotypeId,GenotypeRef,GenotypeAlt,ColorDyName) " +
        //        "VALUES (@NumFile,@Chrom,@Start,@End,@Id,@Ref,@Alt,@DyDis,@DyMut,@MutID,@GenotypeChrom,@GenotypePos,@GenotypeId,@GenotypeRef,@GenotypeAlt,@ColorDyName) END "+
        //       "select IdMutation from dbo.Mutations WHERE Mutations.NumFile = @NumFile and Mutations.Start1 = @Start and Mutations.Chrom = @Chrom and Mutations.End1 = @End END";

        //    using (SqlCommand command = new SqlCommand(query, connection, transaction))
        //    {
        //       command.Parameters.AddWithValue("@NumFile", "11");//IdFile
        //        command.Parameters.AddWithValue("@Chrom", Person.Chrom);
        //        command.Parameters.AddWithValue("@Start", Person.Start);
        //        command.Parameters.AddWithValue("@End", Person.End);
        //        command.Parameters.AddWithValue("@Id", Person.Id);
        //        command.Parameters.AddWithValue("@Ref", Person.Ref);
        //        command.Parameters.AddWithValue("@Alt", Person.Alt);
        //        command.Parameters.AddWithValue("@DyDis", Person.DyDis);
        //        command.Parameters.AddWithValue("@DyMut", Person.DyMut);
        //        command.Parameters.AddWithValue("@MutID", Person.MutID);
        //        command.Parameters.AddWithValue("@GenotypeChrom", Person.GenotypeChrom);
        //        command.Parameters.AddWithValue("@GenotypePos", Person.GenotypePos);
        //        command.Parameters.AddWithValue("@GenotypeId", Person.GenotypeId);
        //        command.Parameters.AddWithValue("@GenotypeRef", Person.GenotypeRef);
        //        command.Parameters.AddWithValue("@GenotypeAlt", Person.GenotypeAlt);
        //        command.Parameters.AddWithValue("@ColorDyName", Person.ColorDyName);
        //        //connection.Open();
        //        var result = long.Parse(command.ExecuteScalar().ToString());
        //        SqlParameters.InsertOneRowListPerson(Person.ListPerson,result, idRuns, connection, transaction);

        //        if (result < 0)
        //            Console.WriteLine("Error inserting data into Database!");
        //    }
        //    // connection.Close();
        //    //}

        //}
       
        //public static void UpdateTblMovedLevelToNotMove(SqlConnection con, SqlCommand cmd, SqlTransaction transaction, string idFile)
        //{
        //    strSQL = "UPDATE dbo.MovedLevel SET Level1 = 1 WHERE IdFile=@IdFile";
        //    cmd = new SqlCommand(strSQL, con, transaction);
        //    cmd.Parameters.AddWithValue("@IdFile", idFile);
        //    cmd.ExecuteNonQuery();
        //}
        //public static void InsertTblMovedLevel(SqlConnection con, SqlCommand cmd, SqlTransaction transaction, string idFile)
        //{
        //    strSQL = "Insert into  dbo.MovedLevel(IdGroup,Level1)Values(@IdFile,1)";
        //    cmd = new SqlCommand(strSQL, con, transaction);
        //    cmd.Parameters.AddWithValue("@IdFile", idFile);
        //    cmd.ExecuteNonQuery();
        //    //cmd.CommandText = "SELECT @@IDENTITY";
        //    //IdTxtFile = cmd.ExecuteScalar().ToString();
        //}
    }
}
