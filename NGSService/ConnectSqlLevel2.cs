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
    //public class ConnectSqlLevel2
    //{
    //    public static void InsertAllListPerson1(Level2 Person, SqlConnection connection, SqlTransaction transaction)
    //    {
    //        String query = "INSERT INTO dbo.Level2(IdFile,Pos,Id,Ref,Alt,DyDis,DyMut,MutID) " +
    //            "VALUES (@IdFile,@Pos,@Id,@Ref,@Alt,@DyDis,@DyMut,@MutID);";
    //        using (SqlCommand command = new SqlCommand(query, connection, transaction))
    //        {
    //            command.Parameters.AddWithValue("@IdFile", "11");//IdFile
    //            command.Parameters.AddWithValue("@Pos", Person.Pos);
    //            command.Parameters.AddWithValue("@Id", Person.Id);
    //            command.Parameters.AddWithValue("@Ref", Person.Ref);
    //            command.Parameters.AddWithValue("@Alt", Person.Alt);
    //            command.Parameters.AddWithValue("@DyDis", Person.DyDis);
    //            command.Parameters.AddWithValue("@DyMut", Person.DyMut);
    //            command.Parameters.AddWithValue("@MutID", Person.MutID);
    //            var result = command.ExecuteNonQuery();
    //            command.CommandText = "SELECT @@IDENTITY";
    //            var IdList = long.Parse(command.ExecuteScalar().ToString());
    //            InsertOneRowListPerson(Person.ListPerson, IdList, connection, transaction);

    //            if (result < 0)
    //                Console.WriteLine("Error inserting data into Database!");
    //        }
    //    }
    //    public static void InsertOneRowListPerson(List<PersonLevel1> ListPerson, long idList, SqlConnection connection, SqlTransaction transaction)
    //    {
    //        foreach (var l2 in ListPerson)
    //        {
    //            String query = "INSERT INTO dbo.ListPersonForLevel1 (IdMutation,NameP,Genotype,AlleleCoverage,TotalCoverage,GenotypeColor,AlleleCoverageColor,TotalCoverageColor) VALUES" +
    //                         " (@IdMutation,@Name,@Genotype,@AlleleCoverage,@TotalCoverage,@GenotypeColor,@AlleleCoverageColor,@TotalCoverageColor)";
    //        // SqlParameters.PersonParameter(l2, idList, connection, transaction, query);
    //        }
    //    }

    //}
}
