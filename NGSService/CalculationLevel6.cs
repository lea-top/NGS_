using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationLevel6
    {
        public static Dictionary<string, List<string>> myDic = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> myDic5 = new Dictionary<string, List<string>>();
        public CalculationLevel6()
        {
            myDic = new Dictionary<string, List<string>>();
            //myDic.Add("2", new List<string>() { "HS", "1224", "0187I" });//2
            //myDic.Add("31", new List<string>() { "CLT", "L1553VFS", "0208I" });//4
            //myDic.Add("32", new List<string>() { "U2S", "C239INS4", "0156I" });
            myDic.Add("21", new List<string>() { "PMT", "Y515X", "0233I" });//3
            myDic.Add("22", new List<string>() { "MMH", "271DUPA", "0122I" });
            myDic.Add("23", new List<string>() { "TS", "1277", "0044U" });
            myDic.Add("24", new List<string>() { "BL", "2481T", "0084U" });
            myDic.Add("25", new List<string>() { "F1A", "2172insG", "0126I" });
            myDic.Add("26", new List<string>() { "E3", "Y35X", "0092I" });
            myDic.Add("27", new List<string>() { "CF", "2751+1INST", "0089I" });
            myDic.Add("28", new List<string>() { "WW", "1167A", "0099I" });
            myDic.ToList();

            myDic5 = new Dictionary<string, List<string>>();
            myDic5.Add("1", new List<string>() { "GAL_5.5DEL_0190U-MUT", "GAL_5.5DEL_0190U-WT_Merged_197798417"});//5
            myDic5.Add("2", new List<string>() { "GEN_MALE_0177I_chrY_197798298", "GEN_MALE_0177I_chrX_197798296"});
            myDic5.Add("3", new List<string>() { "ML_EX1-7_0080U-MUT", "ML_EX1-7_0080U-WT_197798301" });
            myDic5.Add("4", new List<string>() { "NM_R2478_0161U-MUT", "NM_R2478_0161U-WT_197798302"});
            myDic5.Add("5", new List<string>() { "POL_EX2120_0247I-MUT", "POL_EX2120_0247I-WT_197798303" });
            myDic5.Add("6", new List<string>() { "TRM_EXON2-7DEL_0240I-MUT", "TRM_EXON2-7DEL_0240I-WT_197798300" });
            myDic5.ToList();

        }

        public static void Change(Production p)
        {
            //int ifPink = 0;
            for (int i = 0; i < p.ListLevel6.Count; i++)
            {
                //----------------------------------------1------------------------------------------------------ -
                //if (p.ListLevel6[i].ColorDyName.Equals("Pink"))
                //{
                //    if (i - ifPink == 1)
                //    {
                //        ifPink = i;
                //        p.ListLevel6.Remove(p.ListLevel6[i]);
                //    }
                //    else ifPink = i;
                //}
                //----------------------------------------2-------------------------------------------------------
                if (p.ListLevel6[i].DyDis.Equals("HS") && p.ListLevel6[i].DyMut.Equals("1224") && p.ListLevel6[i].MutID.Equals("0187I"))
                {
                  //  Level2 HS1224 = CalculationLevel2.search(p.ListLevel2);
                    for (int x = 0; x < p.ListLevel6[i].ListPerson.Count; x++)
                    {
                        PersonLevel1 result = p.ListLevel2[0].ListPerson.First(s => s.Name.Equals(p.ListLevel6[i].ListPerson[x].Name));
                        if(result!=null)
                        { 
                            p.ListLevel6[i].ListPerson[x].AlleleCoverage = result.AlleleCoverage;
                            p.ListLevel6[i].ListPerson[x].TotalCoverage = result.TotalCoverage;
                            p.ListLevel6[i].ListPerson[x].Genotype = result.Genotype;
                            p.ListLevel6[i].ListPerson[x].Color = result.Color;
                        }
                    }
                    p.ListLevel6[i].ColorDyName = "deepskyblue";
                }
                //-----------------------------------------4-------------------------------------------------------
                else if (p.ListLevel6[i].DyDis.Equals("CLT") && p.ListLevel6[i].DyMut.Equals("L1553VFS") && p.ListLevel6[i].MutID.Equals("0208I"))
                {
                    p.ListLevel6[i] = ChangeItem3(p.ListLevel6[i], p.ListLevel4, "CLT", "L1553VFS", "0208I");
                }
                else if (p.ListLevel6[i].DyDis.Equals("U2S") && p.ListLevel6[i].DyMut.Equals("C239INS4") && p.ListLevel6[i].MutID.Equals("0156I"))
                {
                    p.ListLevel6[i] = ChangeItem3(p.ListLevel6[i], p.ListLevel4, "U2S", "C239INS4", "0156I");
                }
                //-----------------------------------------3-------------------------------------------------------
                else
                {
                    foreach (var y in myDic)
                    {
                        if (p.ListLevel6[i].DyDis.Equals(y.Value[0]) && p.ListLevel6[i].DyMut.Equals(y.Value[1]) && p.ListLevel6[i].MutID.Equals(y.Value[2]))
                        {
                            p.ListLevel6[i] = ChangeItem3(p.ListLevel6[i], p.ListLevel3, y.Value[0], y.Value[1], y.Value[2]);
                            break;
                        }
                    }
                }
                //-----------------------------------------Mpileup Insertions------------------------------------------------------
                if (p.ListLevel6[i].DyDis.Equals(myDic["23"][0]) && p.ListLevel6[i].DyMut.Equals(myDic["23"][1]) && p.ListLevel6[i].MutID.Equals(myDic["23"][2]))
                {
                    p.ListLevel6[i] = ChangeItemMpileupInsertions(p.ListLevel6[i], p.ListMpileupInsertions);
                }
            }
            //-----------------------------------------5-------------------------------------------------------
            var la = new List<Level5>() { };
            var lb = new List<Level5>() { };
            foreach (var item in p.ListLevel5)
            {
                if (item.MutWt.Equals("chrY") || item.MutWt.Equals("MUT"))
                {
                    la.Add(item);
                }
                else lb.Add(item);
            }
            int j = -1;
            foreach (var item in la)//5
            {
                var value = myDic5.First(o => item.Chrom.Equals(o.Value[0]));
                if(value.Key!=null)
                j = GetItemIndex(lb,value.Value[1]);

                p.ListLevel6.Add(new Level1
                {
                    Chrom = "-----",
                    Start = 0,
                    End = "-----",
                    Id = item.Id,
                    Ref = item.Ref,
                    Alt = item.Alt,
                    DyDis = item.DyDis,
                    DyMut = item.DyMut,
                    MutID = item.MutID,
                    GenotypeChrom = "-----",
                    GenotypePos = 0,
                    GenotypeId = "-----",
                    GenotypeRef = "-----",
                    GenotypeAlt = "-----",
                    ColorDyName = "deepskyblue",
                    ListPerson = readPersonLevel5To6(p.ListLevel6,item.ListPerson.ToList(), lb[j].ListPerson,item.DyDis.Equals("GEN")?true:false)
                });
            }
        }
        public static Level1 ChangeItem3(Level1 item, List<Level3> level3, string DyDis, string DyMut, string MutID)
        {
            Level3 changeItem = CalculationLevel3.Search(level3, DyDis, DyMut, MutID);
            item.ColorDyName = "Green";
            for (int i = 0; i < item.ListPerson.Count; i++)
            {
                PersonLevel1 result = changeItem.ListPerson.First(s => s.Name.Equals(item.ListPerson[i].Name));
                if (result != null)
                {
                    if (!result.Genotype.Equals("./."))
                    {
                        item.ListPerson[i].AlleleCoverage = result.AlleleCoverage;
                        item.ListPerson[i].TotalCoverage = result.TotalCoverage;
                        item.ListPerson[i].Genotype = result.Genotype;
                        item.ListPerson[i].Color = result.Color;
                    }
                }
            }
            return item;
        }
        public static Level1 ChangeItemMpileupInsertions(Level1 item, List<Level3> levelMpileupInsertions)
        {
            Level3 changeItem = CalculationMpileupInsertions.searchTs(levelMpileupInsertions);
            item.ColorDyName = "palegreen";
            for (int i = 0; i < item.ListPerson.Count; i++)
            {
                PersonLevel1 result = changeItem.ListPerson.First(s => s.Name.Equals(item.ListPerson[i].Name));
                if (result != null)
                {
                    if (result.Color.Genotype.Equals(ColorMutation.Blue.ToString()))
                    {
                        item.ListPerson[i].Genotype = "0/1";
                        item.ListPerson[i].Color.Genotype = result.Color.Genotype;
                    }
                }
            }
            return item;
        }
        public static int GetItemIndex(List<Level5> lb, string value)
        {
            var j = -1;
            var result = lb.Select((Value, Index) => new { Value, Index })
                            .SingleOrDefault(l => l.Value.Chrom == value);
            j = result == null ? -1 : result.Index;
            return j;
        }

        public static List<PersonLevel1> readPersonLevel5To6(List<Level1> lp6, List<PersonLevel1> lp, List<PersonLevel1> lp2,bool ifGen)
        {
            List<PersonLevel1> listPerson1 = new List<PersonLevel1>() { };
            double sum = 0.00, mean = 0.00;
            double bigSum = 0.00, stdDev;
            int index = 0;
            var backgroundColor = "antiquewhite";
            lp6[0].ListPerson.ForEach(d => {
                if (index % 2 != 0)
                {
                    backgroundColor = "white";
                }
                else backgroundColor = "antiquewhite";

                var listPersonto6Mut = lp.First(l => l.Name.Equals(d.Name));
                var listPersonto6Wt = lp2.First(l => l.Name.Equals(d.Name));
                sum += listPersonto6Mut.TotalCoverage != null && listPersonto6Wt.TotalCoverage != null &&
                                        listPersonto6Mut.TotalCoverage != "0" && listPersonto6Wt.TotalCoverage != "0"
                                        ? (float.Parse(listPersonto6Mut.TotalCoverage) / float.Parse(listPersonto6Wt.TotalCoverage)): 0;            
                listPerson1.Add(
                    new PersonLevel1
                    {
                        Name = listPersonto6Mut.Name,
                        Genotype = "-----",
                        AlleleCoverage = listPersonto6Mut.TotalCoverage != null && listPersonto6Wt.TotalCoverage != null &&
                                        listPersonto6Mut.TotalCoverage != "0" && listPersonto6Wt.TotalCoverage != "0"
                                        ? (float.Parse(listPersonto6Mut.TotalCoverage) / float.Parse(listPersonto6Wt.TotalCoverage)).ToString() : "0",
                        TotalCoverage = "-----",
                        Color = new ColorLevel1 { Genotype = backgroundColor, AlleleCoverage = backgroundColor, TotalCoverage = backgroundColor } 
                    });
                index++;
            });
            mean = sum / listPerson1.Count;
            for (int i = 0; i < listPerson1.Count; i++)
            {
                bigSum += Math.Pow(Double.Parse(listPerson1[i].AlleleCoverage) - mean, 2);
            }
            stdDev = Math.Sqrt(bigSum / (listPerson1.Count-1));

            foreach (var value in listPerson1)
            {
                //value.TotalCoverage = (double.Parse(value.AlleleCoverage) - mean / stdDev).ToString();
                value.TotalCoverage = ((double.Parse(value.AlleleCoverage)-mean) /(stdDev / Math.Sqrt(listPerson1.Count))).ToString();
                value.Genotype = ifGen == true ? Double.Parse(value.AlleleCoverage) < 0.1 ? "0/0" : "0/1" : Double.Parse(value.TotalCoverage) < 10 ? "0/0" : "0/1";
                //value.Genotype = ifGen==true ? Double.Parse(value.TotalCoverage)<0 ? "0/0" : "0/1" : Double.Parse (value.TotalCoverage) < 10 ? "0/0" : "0/1";
                // value.Color.Genotype = value.Genotype.Equals("0/1") ? "" : "";
            }
            foreach (var person in listPerson1)
            {
                CalculationLevel1.WhichColor(person, 1);
            }

            return listPerson1;
        }
    }
}
