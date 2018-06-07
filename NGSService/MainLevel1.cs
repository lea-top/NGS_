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
    public class MainLevel1
    {
        public static Dictionary<string, List<string>> myDicPink = new Dictionary<string, List<string>>();
        public List<Level1> ListLevel1 { get; set; }/* = new List<Level2>() { };*/
        public MainLevel1(string path, Production p,string barcodePlate)
        {

            myDicPink = new Dictionary<string, List<string>>();
            myDicPink.Add("1", new List<string>() { "BL", "2281", "0054U" });//1
            myDicPink.Add("2", new List<string>() { "POL", "3761INSG", "0225I" });
            myDicPink.Add("3", new List<string>() { "GAL", "687INSCA", "0228I" });
            myDicPink.ToArray();

            ListLevel1 = Files.readTxtLevel1(path);

            // p.RunPlates = ConnectSqlLevel0.SelectBarcodePlates(ListLevel1);
            p.RunPlates = new Run();
            // p.RunPlates.BarcodeCantrige = ConnectSqlLevel0.SelectIdRunBySampleName(ListLevel1[0].ListPerson[0].Name);
            //p.RunPlates = ConnectSqlLevel0.SelectRuns(p.RunPlates.BarcodeCantrige);
            p.RunPlates = ConnectSqlLevel0.SelectRuns(barcodePlate);
            if (p.RunPlates.BarcodeCantrige == null) { throw new Exception("the barcode of plate 1 : "+ barcodePlate+" , not found"); }
            MainLevel8 list8 = new MainLevel8(p.RunPlates);//צריך לשלוף פה גם את הבר קוד של הריצה 
            //p.ListLevel8 = list8.ListLevel8;


            p.ListLevel8 = InsertCalculationForPerson1(p.RunPlates.BarcodeCantrige);
        }

        // הוספת השינויים האותומטיים לכל אדם
        private List<Level0> InsertCalculationForPerson1(string barcodeCantrige)
        {
            var duplicates = ListLevel1
                            .GroupBy(i => i.DyDis + i.DyMut + i.MutID)
                            .Where(g => g.Count() > 1)
                          //  .Select(g => g.Key);
                          .Select(y => new { Element = y.Key, Counter = y.Count() }).ToList();

            //duplicates.ToList().ForEach(dup =>
            //{
            //    if (dup.Element.Equals(myDicPink["3"][0]+ myDicPink["3"][1]+ myDicPink["3"][2]) && dup.Counter <= 2 ||
            //        dup.Element.Equals(myDicPink["2"][0]+ myDicPink["2"][1]+ myDicPink["2"][2]) && dup.Counter <= 2 || 
            //        dup.Element.Equals(myDicPink["1"][0]+ myDicPink["1"][1]+ myDicPink["1"][2]) && dup.Counter <= 4 )
            //    { }
            //    else
            //    {
            //        ListLevel1.ForEach(mutshion =>
            //        {
            //            if ((mutshion.DyDis+mutshion.DyMut+mutshion.MutID).Equals(dup.Element) && mutshion.Ref.Equals(mutshion.GenotypeRef))
            //            {
            //                mutshion.ListPerson.ForEach(person =>
            //                {
            //                    if (person.Genotype.Equals("0/0") || person.Genotype.Equals("./."))
            //                    {
            //                         var p1 = ListLevel1.Find(mutshion2 => ((mutshion2.DyDis + mutshion2.DyMut + mutshion2.MutID).Equals(dup.Element) && !mutshion2.Ref.Equals(mutshion2.GenotypeRef)))
            //                              .ListPerson.Find(p => p.Name.Equals(person.Name));
            //                        person = new PersonLevel1() {Name=p1.Name,Genotype=p1.Genotype,AlleleCoverage=p1.AlleleCoverage,TotalCoverage=p1.TotalCoverage,Color=p1.Color};
            //                    }
            //                });
            //            }
            //        });  
            //    };
            //});
            for (int i = 0; i < duplicates.Count; i++)
            {
                if (duplicates[i].Element.Equals(myDicPink["3"][0] + myDicPink["3"][1] + myDicPink["3"][2]) && duplicates[i].Counter <= 2 ||
                    duplicates[i].Element.Equals(myDicPink["2"][0] + myDicPink["2"][1] + myDicPink["2"][2]) && duplicates[i].Counter <= 2 ||
                    duplicates[i].Element.Equals(myDicPink["1"][0] + myDicPink["1"][1] + myDicPink["1"][2]) && duplicates[i].Counter <= 4)
                { }
                else
                {
                    var m1 = ListLevel1.Find(mutshion2 => ((mutshion2.DyDis + mutshion2.DyMut + mutshion2.MutID).Equals(duplicates[i].Element) && mutshion2.Ref.Equals(mutshion2.GenotypeRef)));
                    m1.ColorDyName = ColorMutation.Orange.ToString();
                    var m2 = ListLevel1.FindAll(mutshion2 => ((mutshion2.DyDis + mutshion2.DyMut + mutshion2.MutID).Equals(duplicates[i].Element) && !mutshion2.Ref.Equals(mutshion2.GenotypeRef)));
                    for (int i2 = 0; i2 < m2.Count; i2++)
                    {
                        for (int y = 0; y < m1.ListPerson.Count; y++)
                        {
                            if (m1.ListPerson[y].Genotype.Equals("0/0") || m1.ListPerson[y].Genotype.Equals("./."))
                            {
                                var p2 = m2[i2].ListPerson.Find(p => p.Name.Equals(m1.ListPerson[y].Name));
                                m1.ListPerson[y] = new PersonLevel1() { Name = p2.Name, Genotype = p2.Genotype, AlleleCoverage = p2.AlleleCoverage, TotalCoverage = p2.TotalCoverage, Color = p2.Color };
                            }
                        }
                        ListLevel1.Remove(m2[i2]);
                    }
                }
            }
            foreach (var person in ListLevel1)
            {
                CalculationLevel1 c1 = new CalculationLevel1(person) { };

                //Level1 p = person;
                //Thread thread1 = new Thread(() => new CalculationLevel1(p));
                //thread1.Start();
            }
           
            //foreach (var sample in ListLevel1[0].ListPerson)
            //{
            //    if (ConnectSqlLevel0.SelectIfSampleinLevel0(barcodeCantrige, sample.Name) == null)//?? איך יודעים איזה פלטה??
            //    { throw new Exception("the : " + sample.Name + "not in step 1"); }
            //}
            foreach (var item in myDicPink)
            {
                int ifPink = 0;
                var listPink = ListLevel1.FindAll(f => f.ColorDyName.Equals(ColorMutation.Pink.ToString()) && f.DyDis.Equals(item.Value[0]) && f.DyMut.Equals(item.Value[1]) && f.MutID.Equals(item.Value[2]));
                listPink.ForEach(o => { if (++ifPink < listPink.Count) ListLevel1.Remove(listPink[ifPink]); });
            }
            return ConnectSqlLevel0.SelectIfSampleinLevel0(barcodeCantrige, ListLevel1[0].ListPerson);
        }
        public static void InsertAllListPerson(Level1 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson, 1, Person.Chrom, Person.Start, Person.End, Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, Person.GenotypeChrom, Person.GenotypePos, Person.GenotypeId, Person.GenotypeRef, Person.GenotypeAlt, Person.ColorDyName,
               idRuns, connection, transaction);
        }
        ////להכניס נתונים לטבלה
        //public static void InsertIntoDatabaseListPerson(List<Level1> list1, List<string> idFiles)
        //{
        //    SqlTransaction transaction;
        //    using (SqlConnection connection = new SqlConnection(ConnectSqlLevel1.connectionString))
        //    {
        //        connection.Open();
        //        transaction = connection.BeginTransaction();
        //        try
        //        {
        //            foreach (var l in list1)
        //            {
        //              //  ConnectSqlLevel1.InsertAllListPerson1(l, connection, transaction);
        //            }
        //            SqlCommand command = null;
        //            //   ConnectSqlLevel1.InsertTblMovedLevel(connection, command, transaction, "1111");
        //            // ConnectSqlLevel2.UpdateTblMovedLevelToNotMove(connection, command, transaction, "?????");  אחרי שיהיה שלב 1
        //            // ConnectSqlLevel2.InsertIntoTblGroupPlates( con, cmd, transaction, idFiles)

        //            transaction.Commit();
        //        }
        //        catch (Exception e)
        //        {
        //            transaction.Rollback();
        //            throw e;
        //        }
        //        connection.Close();
        //    }
        //}

    }
}
