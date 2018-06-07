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
   public class MainLevel5
    {
        public List<Level5> ListLevel5 { get; set; }/* = new List<Level2>() { };*/
        public MainLevel5(string path)
        {
            ListLevel5 = Files.readTxtLevel5(path);
            InsertCalculationForPerson5();
        }

        // הוספת השינויים האותומטיים לכל אדם
        private void InsertCalculationForPerson5()
        {
            //var l = ListLevel5;
            //foreach (var item in ListLevel5)
            //{
            //    if (!(item.Chrom.Equals("GAL_5.5DEL_0190U-MUT") && item.Pos == 270) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("GAL_5.5DEL_0190U-WT_Merged_197798417") && item.Pos == 80) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("GEN_MALE_0177I_chrX_197798296") && item.Pos == 56) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("GEN_MALE_0177I_chrY_197798298") && item.Pos == 92) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("ML_EX1-7_0080U-MUT") && item.Pos == 83 )&&
            //        !(item.Pos == 92 &&item.Chrom.Equals("ML_EX1-7_0080U-WT_197798301") && item.Pos == 74) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("NM_R2478_0161U-MUT") && item.Pos == 433 )&&
            //        !(item.Pos == 92 &&item.Chrom.Equals("NM_R2478_0161U-WT_197798302")&& item.Pos == 131 )&&
            //        !(item.Pos == 92 &&item.Chrom.Equals("POL_EX2120_0247I-MUT") && item.Pos == 344 )&&
            //        !(item.Pos == 92 &&item.Chrom.Equals("POL_EX2120_0247I-WT_197798303") && item.Pos == 140) &&
            //        !(item.Pos == 92 &&item.Chrom.Equals("TRM_EXON2-7DEL_0240I-MUT") && item.Pos == 186) &&
            //        !(item.Chrom.Equals("TRM_EXON2-7DEL_0240I-WT_197798300") && item.Pos == 114))
            //        ListLevel5.Remove(item);
            //}
            //ListLevel5.Sort(s => { if (s.MutWt == "chrX") return 0; 
            //else return x.PartName.CompareTo(y.PartName)
            //});


            foreach (var person in ListLevel5)
            {
                switch (person.DyDis)
                {
                    case "GAL":
                        person.ColorDyName = "Green";
                        break;
                    case "GEN":
                        person.ColorDyName = "Orange";
                        break;
                    case "ML":
                        person.ColorDyName = "Yellow";
                        break;
                    case "NM":
                        person.ColorDyName = "Bordeaux";
                        break;
                    case "POL":
                        person.ColorDyName = "Blue";
                        break;
                    case "TRM":
                        person.ColorDyName = "LigthGreen";
                        break;
                    default:
                        break;
                }
                Level5 p = person;
                Thread thread1 = new Thread(() => new CalculationLevel5(p));
                thread1.Start();
            }
        }
        public static void InsertAllListPerson(Level5 Person, string idRuns, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameters.InsertAllListPerson(Person.ListPerson,5, Person.Chrom, 0, "", Person.Id, Person.Ref, Person.Alt, Person.DyDis,
             Person.DyMut, Person.MutID, "", 0, "", "", "", Person.ColorDyName,
               idRuns, connection, transaction);
        }
    }
}
