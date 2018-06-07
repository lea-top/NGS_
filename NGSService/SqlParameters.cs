using NGSData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    class SqlParameters
    {
        public static void InsertAllListPerson(List<PersonLevel1> person, int numFile, string chrom, long start, string end, string id, string ref1, string alt, string dyDis,
            string dyMut, string mutID, string genotypeChrom, long genotypePos, string genotypeId, string genotypeRef, string genotypeAlt, string colorDyName,
            string idRuns, SqlConnection connection, SqlTransaction transaction)
        {

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            String query =
                "BEGIN" +
                    " IF NOT EXISTS (SELECT * FROM Mutations WHERE Mutations.NumFile = @NumFile" +
                                    " and Mutations.Start1 = @Start " +
                                    "and Mutations.Chrom = @Chrom " +
                                    "and Mutations.End1 = @End " +
                                    //"and Mutations.Id=@Id " +
                                    //"and Mutations.Ref=@Ref " +
                                    //"and Mutations.Alt=@Alt " +
                                    "and Mutations.DyDis=@DyDis " +
                                    "and Mutations.DyMut=@DyMut " +
                                    "and Mutations.MutID=@MutID " +
                                    " and Mutations.GenotypeRef=@GenotypeRef " +
                                    "and Mutations.GenotypeAlt=@GenotypeAlt) " +
                    "BEGIN " +
                        "INSERT INTO dbo.Mutations(NumFile,Chrom,Start1,End1,Id,Ref,Alt,DyDis,DyMut,MutID,GenotypeChrom,GenotypePos,GenotypeId,GenotypeRef,GenotypeAlt,ColorDyName) " +
                        "VALUES (@NumFile,@Chrom,@Start,@End,@Id,@Ref,@Alt,@DyDis,@DyMut,@MutID,@GenotypeChrom,@GenotypePos,@GenotypeId,@GenotypeRef,@GenotypeAlt,@ColorDyName)" +
                    " END " +
                    "select IdMutation from dbo.Mutations WHERE Mutations.NumFile = @NumFile " +
                                    "and Mutations.Start1 = @Start " +
                                    "and Mutations.Chrom = @Chrom " +
                                    "and Mutations.End1 = @End " +
                                    //"and Mutations.Id=@Id " +
                                    //"and Mutations.Ref=@Ref " +
                                    //"and Mutations.Alt=@Alt " +
                                    "and Mutations.DyDis=@DyDis " +
                                    "and Mutations.DyMut=@DyMut " +
                                    "and Mutations.MutID=@MutID " +
                                    " and Mutations.GenotypeRef=@GenotypeRef " +
                                    "and Mutations.GenotypeAlt=@GenotypeAlt " +
                " END";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@NumFile", numFile);
                if (chrom == null)
                    command.Parameters.AddWithValue("@Chrom", DBNull.Value);
                else command.Parameters.AddWithValue("@Chrom", chrom);
                command.Parameters.AddWithValue("@Start", start);
                if (end == null)
                    command.Parameters.AddWithValue("@End", DBNull.Value);
                else command.Parameters.AddWithValue("@End", end);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Ref", ref1);
                command.Parameters.AddWithValue("@Alt", alt);
                command.Parameters.AddWithValue("@DyDis", dyDis);
                command.Parameters.AddWithValue("@DyMut", dyMut);
                command.Parameters.AddWithValue("@MutID", mutID);
                if (genotypeChrom == null)
                    command.Parameters.AddWithValue("@GenotypeChrom", DBNull.Value);
                else command.Parameters.AddWithValue("@GenotypeChrom", genotypeChrom);
                command.Parameters.AddWithValue("@GenotypePos", genotypePos);
                if (genotypeId == null)
                    command.Parameters.AddWithValue("@GenotypeId", DBNull.Value);
                else command.Parameters.AddWithValue("@GenotypeId", genotypeId);
                if (genotypeRef == null)
                    command.Parameters.AddWithValue("@GenotypeRef", DBNull.Value);
                else command.Parameters.AddWithValue("@GenotypeRef", genotypeRef);
                if (genotypeAlt == null)
                    command.Parameters.AddWithValue("@GenotypeAlt", DBNull.Value);
                else command.Parameters.AddWithValue("@GenotypeAlt", genotypeAlt);
                command.Parameters.AddWithValue("@ColorDyName", colorDyName);
                //connection.Open();
                var result = long.Parse(command.ExecuteScalar().ToString());
                InsertOneRowListPerson(person, result, idRuns, numFile, connection, transaction);

                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
            // connection.Close();
            //}

        }
        public static void InsertAllListPerson(List<PersonLevel9> person, string dyDis, string dyMut, string mutID, string colorDyName,
           string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            String query = "select IdMutation from Mutations WHERE Mutations.NumFile = @NumFile " +
                                    "and Mutations.DyDis=@DyDis " +
                                    "and Mutations.DyMut=@DyMut " +
                                    "and Mutations.MutID=@MutID ";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@NumFile", 6);
                command.Parameters.AddWithValue("@DyDis", dyDis);
                command.Parameters.AddWithValue("@DyMut", dyMut);
                command.Parameters.AddWithValue("@MutID", mutID);
                //connection.Open();
                var result = long.Parse(command.ExecuteScalar().ToString());
                InsertOneRowListPersonFinal(person, result, idRuns, connection, transaction);
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
            // connection.Close();
            //}
        }
        public static void InsertOneRowListPerson(List<PersonLevel1> ListPerson, long idList, string idRuns, int numFile, SqlConnection connection, SqlTransaction transaction)
        {
            foreach (var l2 in ListPerson)
            {
                String query = "INSERT INTO dbo.ListPerson (IdRuns,IdMutation,NumFile,NameP,Genotype,AlleleCoverage,TotalCoverage,GenotypeColor,AlleleCoverageColor,TotalCoverageColor) VALUES" +
                             " (@IdRuns,@IdMutation,@numFile ,@Name,@Genotype,@AlleleCoverage,@TotalCoverage,@GenotypeColor,@AlleleCoverageColor,@TotalCoverageColor)";
                PersonParameter(l2, idList, idRuns, numFile, connection, transaction, query);
            }
        }
        public static void InsertOneRowListPersonFinal(List<PersonLevel9> ListPerson, long idList, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            foreach (var l2 in ListPerson)
            {
                String query = "INSERT INTO dbo.ListPersonFinal (IdRuns,IdMutation,NameP,Result,ResultColor) VALUES" +
                             " (@IdRuns,@IdMutation,@Name,@Result,@ResultColor)";
                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@IdRuns", idRuns);
                    command.Parameters.AddWithValue("@IdMutation", idList);
                    command.Parameters.AddWithValue("@Name", l2.Name);
                    command.Parameters.AddWithValue("@Result", l2.Results);
                    command.Parameters.AddWithValue("@ResultColor", l2.Color);
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
            }
        }
        public static void PersonParameter(PersonLevel1 l2, long idList, string idRuns, int numFile, SqlConnection connection, SqlTransaction transaction, string query)
        {
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@IdRuns", idRuns);
                command.Parameters.AddWithValue("@IdMutation", idList);
                command.Parameters.AddWithValue("@numFile", numFile);
                command.Parameters.AddWithValue("@Name", l2.Name);
                command.Parameters.AddWithValue("@Genotype", l2.Genotype);
                if (l2.AlleleCoverage == null)
                {
                    command.Parameters.AddWithValue("@AlleleCoverage", DBNull.Value);
                }
                else command.Parameters.AddWithValue("@AlleleCoverage", l2.AlleleCoverage);
                if (l2.TotalCoverage == null)
                {
                    command.Parameters.AddWithValue("@TotalCoverage", DBNull.Value);
                }
                else command.Parameters.AddWithValue("@TotalCoverage", l2.TotalCoverage);
                command.Parameters.AddWithValue("@GenotypeColor", l2.Color.Genotype);
                command.Parameters.AddWithValue("@AlleleCoverageColor", l2.Color.AlleleCoverage);
                command.Parameters.AddWithValue("@TotalCoverageColor", l2.Color.TotalCoverage);
                int result = command.ExecuteNonQuery();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }

    }
}
