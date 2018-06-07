using GemBox.Spreadsheet;
using NGSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

//using NsExcel = Microsoft.Office.Interop.Excel;
namespace NGSService
{
    public class Files
    {
        public Files() { }
        public static List<Level1> readTxtLevel1(string path)
        {
            var colorName = "";
            bool total = false;
            var listPersonName = new List<string>();
            List<Level1> listLevel1 = new List<Level1>() { };
            List<PersonLevel1> listPerson1 = new List<PersonLevel1>() { };
            //string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\devorak\Desktop\genotype_results.txt");
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);/*@"C:\Users\devorak\Desktop\genotype_results.txt"*/
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string headerLine = streamReader.ReadLine();
                var splittedheaderLine = headerLine.Split(new char[] { '\t' });
                foreach (var item in splittedheaderLine.Skip(12))
                {
                    listPersonName.Add(item.Substring(9, item.Length - 9).Split('_')[0]);// "Genotype_"));
                }
                string line;

                while (((line = streamReader.ReadLine())) != null)/*.Skip(1)*/
                {
                    var splittedLines = line.Split(new char[] { '\t' });
                    if (splittedLines != null && splittedLines.Any())
                    {
                        var nameSplit = splittedLines[6].Split('_', '?');
                        //if(!splittedLines[6].Equals("CN?693?0038U") && !splittedLines[6].Equals("WFS?R558C?0167I"))
                        //{
                        if (splittedLines[11].Contains(","))
                        {
                            colorName = "Red";
                            total = true;
                        }
                        else { total = false; colorName = "";
                        }

                        //}
                        listLevel1.Add(new Level1
                        {
                            Chrom = splittedLines[0],
                            Start = long.Parse(splittedLines[1]),
                            End = splittedLines[2],
                            Id = splittedLines[3],
                            Ref = splittedLines[4],
                            Alt = splittedLines[5],
                            //DyName = splittedLines[6],
                            DyDis = nameSplit[0],
                            DyMut = nameSplit[1],
                            MutID = nameSplit[2],
                            GenotypeChrom = splittedLines[7],
                            GenotypePos = long.Parse(splittedLines[8]),
                            GenotypeId = splittedLines[9],
                            GenotypeRef = splittedLines[10],
                            GenotypeAlt = splittedLines[11],
                            ColorDyName = colorName,
                            ListPerson = readPersonLevel1(splittedLines.Skip(12).ToList(), listPersonName,total)
                        });
                    }
                }
            }
            return listLevel1;
        }
        public static List<PersonLevel1> readPersonLevel1(List<string> lp, List<string> listPersonName,bool total)
        {
            var backgroundColor = "antiquewhite";
            int index = 0;
            List<PersonLevel1> listPerson1 = new List<PersonLevel1>() { };
            foreach (var l in lp)
            {
                if (index % 2 != 0)
                { backgroundColor = "white"; }
                else backgroundColor = "antiquewhite";
                if (l != null && l != "")
                {
                    var value = l.Split(':');
                    //if (value[0].Contains("2")) {
                    //  return  ChangeTotalToAllele(lp, listPersonName);
                    //}
                   
                    listPerson1.Add(
                    new PersonLevel1
                    {
                        Name = listPersonName[index++].Replace("s", " ").Replace("a", ".").Replace("y", "]").Replace("x", "["),
                        Genotype = value.Length >= 1 ?  value[0] : value[0],
                        //AlleleCoverage = value.Length >= 2 ? value[1] : "0",
                        //TotalCoverage = value.Length >= 3 ? value[2] : "0",
                        AlleleCoverage = total==true? "/":(value.Length >= 2 ? value[1] : "0"),
                        TotalCoverage = total == true ? (value.Length >= 2 ? value[1]: "0") : (value.Length >= 3 ? value[2]: "0"),
                        Color = new ColorLevel1 { Genotype = backgroundColor, AlleleCoverage = backgroundColor, TotalCoverage = backgroundColor }
                    });
                }
            }
            return listPerson1;
        }
        //public static List<PersonLevel1> ChangeTotalToAllele(List<string> lp, List<string> listPersonName)
        //{
        //    var backgroundColor = "antiquewhite";
        //    int index = 0;
        //    List<PersonLevel1> listPerson1 = new List<PersonLevel1>() { };
        //    foreach (var l in lp)
        //    {
        //        if (index % 2 != 0)
        //        { backgroundColor = "white";
        //        }
        //        else backgroundColor = "antiquewhite";
        //        if (l != null && l != "")
        //        {
        //            var value = l.Split(':');
        //            listPerson1.Add(
        //            new PersonLevel1
        //            {
        //                Name = listPersonName[index++].Replace("s", " ").Replace("a", ".").Replace("y", "]").Replace("x", "["),
        //                Genotype = value.Length >= 1 ? value[0] : value[0],
        //                AlleleCoverage = "/",
        //                TotalCoverage = value.Length >= 2 ? value[1] : "0",//value.Length >= 3 ? value[2] : "0",
        //                Color = new ColorLevel1 { Genotype = backgroundColor, AlleleCoverage = backgroundColor, TotalCoverage = backgroundColor }
        //            });
        //        }
        //    }
        //    return listPerson1;
        //}

        public static List<Level2> readTxtLevel2(string path)
        {
            var listPersonName = new List<string>();
            List<Level2> listLeve2 = new List<Level2>() { };
            List<PersonLevel1> listPerson2 = new List<PersonLevel1>() { };
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string headerLine = streamReader.ReadLine();
                var splittedheaderLine = headerLine.Split(new char[] { '\t' });
                foreach (var item in splittedheaderLine.Skip(5))
                {
                    listPersonName.Add(item.Substring(0, item.IndexOf("_S")));
                }
                string line;

                while (((line = streamReader.ReadLine())) != null)
                {
                    var splittedLines = line.Split(new char[] { '\t' });
                    if (splittedLines != null && splittedLines.Any())
                    {

                        if (splittedLines[0].Equals("HS_1224_0187I_197798326") && splittedLines[1].Equals("111") && splittedLines[4].Equals("C") && splittedLines[3].Equals("A"))
                        {
                            var nameSplit = splittedLines[0].Split('_', '?');
                            listLeve2.Add(new Level2
                            {
                                //Chrom = splittedLines[0],
                                DyDis = nameSplit[0],
                                DyMut = nameSplit[1],
                                MutID = nameSplit[2],
                                Pos = splittedLines[1],
                                Id = splittedLines[2],
                                Ref = splittedLines[3],
                                Alt = splittedLines[4],
                                ColorDyName = "",
                                ListPerson = readPersonLevel2(splittedLines.Skip(5).ToList(), listPersonName)
                            });
                        }
                    }
                }
            }
            return listLeve2;
        }
        public static List<PersonLevel1> readPersonLevel2(List<string> lp, List<string> listPersonName)
        {
            var backgroundColor = "antiquewhite";
            int index = 0;
            List<PersonLevel1> listPerson2 = new List<PersonLevel1>() { };
            foreach (var l in lp)
            {
                if (index % 2 != 0)
                { backgroundColor = "white"; }
                else backgroundColor = "antiquewhite";
                if (l != null && l != "")
                {
                    var value = l.Split(':');
                    listPerson2.Add(
                    new PersonLevel1
                    {
                        Name = listPersonName[index++].Replace("s", " ").Replace("a", ".").Replace("y", "]").Replace("x", "["),
                        Genotype = value.Length >= 1 ? value[0] : value[0],
                        AlleleCoverage = value.Length >= 2 ? value[1] : "0",
                        TotalCoverage = value.Length >= 3 ? value[2] : "0",
                        Color = new ColorLevel1 { Genotype = backgroundColor, AlleleCoverage = backgroundColor, TotalCoverage = backgroundColor }
                    });
                }
            }
            return listPerson2;
        }
        public static List<Level3> readTxtLevel3(string path)
        {
            var listPersonName = new List<string>();
            List<Level3> listLevel3 = new List<Level3>() { };
            List<PersonLevel1> listPerson3 = new List<PersonLevel1>() { };
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string headerLine = streamReader.ReadLine();
                var splittedheaderLine = headerLine.Split(new char[] { '\t' });
                foreach (var item in splittedheaderLine.Skip(12))
                {
                    var t = item.IndexOf("_S");
                    var i = item.Split('_');
                    if (i[0].Equals("Hap"))
                    { listPersonName.Add(i[2]); }
                    else listPersonName.Add(i[1]);
                }
                string line;

                while (((line = streamReader.ReadLine())) != null)
                {
                    var splittedLines = line.Split(new char[] { '\t' });
                    if (splittedLines != null && splittedLines.Any())
                    {
                        var nameSplit = splittedLines[6].Split('_', '?');
                        listLevel3.Add(new Level3
                        {
                            Chrom = splittedLines[0],
                            Start = long.Parse(splittedLines[1]),
                            End = splittedLines[2],
                            Id = splittedLines[3],
                            Ref = splittedLines[4],
                            Alt = splittedLines[5],
                            //DyName = splittedLines[6],
                            DyDis = nameSplit[0],
                            DyMut = nameSplit[1],
                            MutID = nameSplit[2],
                            InsertionsChrom = splittedLines[7],
                            InsertionsPos = long.Parse(splittedLines[8]),
                            InsertionsId = splittedLines[9],
                            InsertionsRef = splittedLines[10],
                            InsertionsAlt = splittedLines[11],
                            ColorDyName = "",
                            ListPerson = readPersonLevel3(splittedLines.Skip(12).ToList(), listPersonName)
                        });
                    }
                }
            }
            return listLevel3;
        }
        public static List<PersonLevel1> readPersonLevel3(List<string> lp, List<string> listPersonName)
        {
            var backgroundColor = "antiquewhite";
            int index = 0;
            List<PersonLevel1> listPerson3 = new List<PersonLevel1>() { };
            foreach (var l in lp)
            {
                if (index % 2 != 0)
                { backgroundColor = "white"; }
                else backgroundColor = "antiquewhite";
                if (l != null && l != "")
                {
                    var value = l.Split(':');
                    listPerson3.Add(
                    new PersonLevel1
                    {
                        Name = listPersonName[index++].Replace("s", " ").Replace("a", ".").Replace("y", "]").Replace("x", "["),
                        Genotype = value.Length >= 1 ? value[0] : value[0],
                        AlleleCoverage = value.Length >= 2 ? value[1] : "0",
                        TotalCoverage = value.Length >= 3 ? value[2] : "0",
                        Color = new ColorLevel1 { Genotype = backgroundColor, AlleleCoverage = backgroundColor, TotalCoverage = backgroundColor }
                    });
                }
            }
            return listPerson3;
        }
        public static List<Level5> readTxtLevel5(string path)
        {
            var listPersonName = new List<string>();
            List<Level5> listLeve5 = new List<Level5>() { };
            List<PersonLevel1> listPerson2 = new List<PersonLevel1>() { };
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string headerLine = streamReader.ReadLine();
                var splittedheaderLine = headerLine.Split(new char[] { '\t' });
                foreach (var item in splittedheaderLine.Skip(5))
                {
                    listPersonName.Add(item.Substring(0, item.IndexOf("_S")));
                }
                string line;

                while (((line = streamReader.ReadLine())) != null)
                {
                    var splittedLines = line.Split(new char[] { '\t' });
                    if (splittedLines != null && splittedLines.Any())
                    {

                        if ((splittedLines[0].Equals("GAL_5.5DEL_0190U-MUT") && splittedLines[1] == "270") ||
                            (splittedLines[0].Equals("GAL_687INSCA_0228IandGAL_5.5DEL_0190U-WT_Merged_197798417") && splittedLines[1] == "80") ||
                            (splittedLines[0].Equals("GEN_MALE_0177I_chrX_197798296") && splittedLines[1] == "56") ||
                            (splittedLines[0].Equals("GEN_MALE_0177I_chrY_197798298") && splittedLines[1] == "92") ||
                            (splittedLines[0].Equals("ML_EX1-7_0080U-MUT") && splittedLines[1] == "83") ||
                            (splittedLines[0].Equals("ML_EX1-7_0080U-WT_197798301") && splittedLines[1] == "74") ||
                            (splittedLines[0].Equals("NM_R2478_0161U-MUT") && splittedLines[1] == "433") ||
                            (splittedLines[0].Equals("NM_R2478_0161U-WT_197798302") && splittedLines[1] == "131") ||
                            (splittedLines[0].Equals("POL_EX2120_0247I-MUT") && splittedLines[1] == "344") ||
                            (splittedLines[0].Equals("POL_EX2120_0247I-WT_197798303") && splittedLines[1] == "140") ||
                            (splittedLines[0].Equals("TRM_EXON2-7DEL_0240I-MUT") && splittedLines[1] == "186") ||
                            (splittedLines[0].Equals("TRM_EXON2-7DEL_0240I-WT_197798300") && splittedLines[1] == "114"))
                        {
                            if (splittedLines[0].Equals("GAL_687INSCA_0228IandGAL_5.5DEL_0190U-WT_Merged_197798417"))
                            {
                                splittedLines[0] = "GAL_5.5DEL_0190U-WT_Merged_197798417";
                            }
                            var nameSplit = splittedLines[0].Split('_', '?');
                            var nameMutWt = nameSplit[2].Split('-');
                            listLeve5.Add(new Level5
                            {
                                Chrom = splittedLines[0],
                                DyDis = nameSplit[0],
                                DyMut = nameSplit[1],
                                MutID = nameMutWt[0],
                                MutWt = nameMutWt.Length > 1 ? nameMutWt[1] : nameSplit[3],
                                Pos = int.Parse(splittedLines[1]),
                                Id = splittedLines[2],
                                Ref = splittedLines[3],
                                Alt = splittedLines[4],
                                ColorDyName = "",
                                ListPerson = readPersonLevel2(splittedLines.Skip(5).ToList(), listPersonName)
                            });
                        }
                    }
                }
            }
            return listLeve5;
        }
        public static string FileWriterLevel6(List<Level1> listPerson, string path, long idShipping)
        {
            try
            {
                var header = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",", /**,\"{14}\"",*/
                                         "Chrom",
                                         "Start",
                                         "End",
                                         "Id",
                                         "Ref",
                                         "Alt",
                                         "DyDis",
                                         "DyMut",
                                         "MutID",
                                         "GenotypeChrom",
                                         "GenotypePos",
                                         "GenotypeId",
                                         "GenotypeRef",
                                         "GenotypeAlt"/*,*/
                                                      //listPerson[0].ListPerson.Select(p => new object[] { "Genotype" + p.Name, p.Name + "allele", p.Name + "total" })
                                        );
                listPerson[0].ListPerson.ForEach(a => header = header + string.Format("\"{0}\",\"{1}\",\"{2}\",", "Genotype" + a.Name, a.Name + "allele", a.Name + "total"));
                var csv = new StringBuilder();
                csv.AppendLine(header);
                foreach (var trade in listPerson)
                {
                    header = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\" ,\"{11}\",\"{12}\",\"{13}\",",
                        trade.Chrom.ToString(),
                        trade.Start,
                        trade.End.ToString(),
                        trade.Id,
                        trade.Ref,
                        trade.Alt,
                        trade.DyDis.ToString(),
                        trade.DyMut,
                        trade.MutID,
                        trade.GenotypeChrom,
                        trade.GenotypePos,
                        trade.GenotypeId,
                        trade.GenotypeRef,
                        trade.GenotypeAlt);
                    trade.ListPerson.ForEach(a => header += string.Format("\"{0}\",\"{1}\",\"{2}\",", a.Genotype, a.AlleleCoverage, a.TotalCoverage));
                    csv.AppendLine(header);
                }
                path = ConfigurationManager.AppSettings["PathSavefile"] + DateTime.Now.Minute.ToString() + "(" + listPerson[0].DyDis + ")" + "-final.csv";//Step4
                File.WriteAllText(path, csv.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return path;
        }

        public static string FileWriterLevel6Excel(List<Level1> listPerson, string path, long idShipping)
        {
            try
            {

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
                ExcelFile ef = new ExcelFile();
                ExcelWorksheet ws = ef.Worksheets.Add("Writing");

                ws.Columns[0].Width = 10* 256;
                ws.Columns[1].Width = 10* 256;
                ws.Columns[2].Width = 10* 256;
                ws.Columns[3].Width = 10* 256;
                ws.Columns[4].Width = 10* 256;
                ws.Columns[5].Width = 10* 256;
                ws.Columns[6].Width = 10* 256;
                ws.Columns[7].Width = 10* 256;
                ws.Columns[8].Width = 10* 256;
                ws.Columns[9].Width = 10* 256;
                ws.Columns[10].Width =10 * 256;
                ws.Columns[11].Width =10 * 256;
                ws.Columns[12].Width =10 * 256;
                ws.Columns[13].Width =10 * 256;
                int numCol = 14;
                foreach (var item in listPerson[0].ListPerson)
                {
                    ws.Columns[numCol++].Width = 5 * 256;
                    ws.Columns[numCol++].Width = 5 * 256;
                    ws.Columns[numCol++].Width = 5 * 256;
                }
                int i, j;
                // ws.Cells[0, 0].Value = "Id";
                ws.Cells[0, 0].Value = "Chrom";
                ws.Cells[0, 1].Value = "Start";
                ws.Cells[0, 2].Value = "End";
                ws.Cells[0, 3].Value = "Id";
                ws.Cells[0, 4].Value = "Ref";
                ws.Cells[0, 5].Value = "Alt";
                ws.Cells[0, 6].Value = "DyDis";
                ws.Cells[0, 7].Value = "DyMut";
                ws.Cells[0, 8].Value = "MutID";
                ws.Cells[0, 9].Value = "GenotypeChrom";
                ws.Cells[0, 10].Value = "GenotypePos";
                ws.Cells[0, 11].Value = "GenotypeId";
                ws.Cells[0, 12].Value = "GenotypeRef";
                ws.Cells[0, 13].Value = "GenotypeAlt";
                numCol = 14;
                foreach (var item in listPerson[0].ListPerson)
                {
                    ws.Cells[0, numCol++].Value = "Genotype" + item.Name;
                    ws.Cells[0, numCol++].Value = item.Name + "allele";
                    ws.Cells[0, numCol++].Value = item.Name + "total";
                }
                int length = numCol;
                CellStyle tmpStyle = new CellStyle();
                tmpStyle.Borders.SetBorders(MultipleBorders.All | MultipleBorders.Inside | MultipleBorders.Top, Color.Black, LineStyle.Thin);

                tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                tmpStyle.FillPattern.SetSolid(Color.LightBlue);
                tmpStyle.Font.Weight = ExcelFont.BoldWeight;
                //tmpStyle.Font.Color = Color.White;
                // tmpStyle.Borders.SetBorders(MultipleBorders.Right | MultipleBorders.Top, Color.Black, LineStyle.Thin);
                tmpStyle.WrapText = false;//true אם השורות יגלשו..

                ws.Cells.GetSubrangeAbsolute(0, 0, 0 , 16).Style = tmpStyle;//16
                ws.Cells.GetSubrangeAbsolute(1,6, listPerson.Count,8).Style = tmpStyle;
                tmpStyle = new CellStyle();
                tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                tmpStyle.Font.Weight = ExcelFont.BoldWeight;
                i = 1; j = 2;
                var ab = 'A';
                CellStyle tmpStyleColor = new CellStyle();
                tmpStyleColor.Borders.SetBorders(MultipleBorders.All | MultipleBorders.Inside | MultipleBorders.Top, Color.Black, LineStyle.Thin);

                foreach (var item in listPerson)
                {
                    // ExcelCell cell= ws.Cells;/* = ws.Cells[i,0]*/
                    //    ws.Cells[i, 0].Value = item.Id.ToString();
                    ws.Cells[i, 0].Value = item.Chrom;
                    ws.Cells[i, 1].Value = item.Start;
                    ws.Cells[i, 2].Value = item.End;
                    ws.Cells[i, 3].Value = item.Id;
                    ws.Cells[i, 4].Value = item.Ref;
                    ws.Cells[i, 5].Value = item.Alt;
                    ws.Cells[i, 6].Value = item.DyDis;
                    ws.Cells[i, 7].Value = item.DyMut;
                    ws.Cells[i, 8].Value = item.MutID;
                    ws.Cells[i, 9].Value = item.GenotypeChrom;
                    ws.Cells[i, 10].Value = item.GenotypePos;
                    ws.Cells[i, 11].Value = item.GenotypeId;
                    ws.Cells[i, 12].Value = item.GenotypeRef;
                    ws.Cells[i, 13].Value = item.GenotypeAlt;
                    if (item.ColorDyName != null && item.ColorDyName != "")
                    {
                        ws = setColor(tmpStyleColor, ws, i, 6, item.ColorDyName, numCol);
                    }
                    else
                    {
                        tmpStyle.FillPattern.SetSolid(Color.LightBlue);
                        ws.Cells.GetSubrangeAbsolute(0, 0, 0, numCol).Style = tmpStyle;
                    }
                    numCol = 14;
                    foreach (var it in item.ListPerson)
                    {
                        if (it.Color != null )/*&& item.ColorDyName != "Yellow"*/
                        {
                            if (it.Color.Genotype != null)
                            {
                                ws = setColorResult(tmpStyleColor, ws, i, numCol, it.Color.Genotype,numCol);
                            }
                        }
                        ws.Cells[i, numCol++].Value = it.Genotype;
                        if (it.Color != null)
                        {
                            if (it.Color.AlleleCoverage != null )/*&& item.ColorDyName != "Yellow"*/
                            {
                                ws = setColorResult(tmpStyleColor, ws, i, numCol, it.Color.AlleleCoverage,numCol);
                            }
                        }
                        ws.Cells[i, numCol++].Value = it.AlleleCoverage;
                        if (it.Color != null)
                        {
                            if (it.Color.TotalCoverage != null )/*&& item.ColorDyName != "Yellow"*/
                            {
                                ws = setColorResult(tmpStyleColor, ws, i, numCol, it.Color.TotalCoverage,numCol);
                            }
                        }
                        ws.Cells[i, numCol++].Value = it.TotalCoverage;
                    }

                    i++;
                }
                CellRange range = ws.Cells.GetSubrange("A1:O" + (listPerson.Count + 1));
                range.Style.Borders.SetBorders(MultipleBorders.Inside, Color.Black, LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.All, Color.Black, LineStyle.Thin);
                path = ConfigurationManager.AppSettings["PathSavefile"] + idShipping + "(" + listPerson[0].DyDis + ")" + "-final.xlsx";//Step4
                ef.Save(path);

                ////NsExcel.ApplicationClass excapp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //NsExcel.Application excapp = new Microsoft.Office.Interop.Excel.Application();
                ////if you want to make excel visible           
                //excapp.Visible = false;
                ////create a blank workbook
                //var workbook = excapp.Workbooks.Add(NsExcel.XlWBATemplate.xlWBATWorksheet);
                ////or open one - this is no pleasant, but yue're probably interested in the first parameter
                //path = ConfigurationManager.AppSettings["PathSavefile"] + DateTime.Now.Minute.ToString() + "()" + "-final.xlsx";//Step4
                ////string workbookPath = path;
                ////var workbook = excapp.Workbooks.Open(workbookPath,
                ////    0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",
                ////    true, false, 0, true, false, false);
                ////Not done yet. You have to work on a specific sheet - note the cast
                ////You may not have any sheets at all. Then you have to add one with NsExcel.Worksheet.Add()
                //var sheet = (NsExcel.Worksheet)workbook.Sheets[1]; //indexing starts from 1
                //////do something usefull: you select now an individual cell
                ////var range = sheet.get_Range("A1", "A1");
                ////range.Value2 = "test"; //Value2 is not a typo
                ////now the list
                //string cellName;
                //int counter = 1;
                //foreach (var item in listPerson)
                //{
                //    cellName = "A" + counter.ToString();
                //    var range = sheet.get_Range(cellName, cellName);
                //    range.Value2 = item.Chrom;
                //    range.Value = item.DyDis;
                //    ++counter;
                //}
               //workbook.SaveAs(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return path;
        }
        public static string FileWriterLevel9FinalExcel(List<Level9> listPerson, string path, long idShipping)
        {
            try
            {

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
                ExcelFile ef = new ExcelFile();
                ExcelWorksheet ws = ef.Worksheets.Add("Writing");

                ws.Columns[0].Width = 10 * 256;
                ws.Columns[1].Width = 10 * 256;
                ws.Columns[2].Width = 10 * 256;    
                int numCol = 3;
                foreach (var item in listPerson[0].ListPerson)
                {
                    ws.Columns[numCol++].Width = 5 * 256;
                    ws.Columns[numCol++].Width = 5 * 256;
                    ws.Columns[numCol++].Width = 5 * 256;
                }
                int i, j;
                ws.Cells[0, 0].Value = "DyDis";
                ws.Cells[0, 1].Value = "DyMut";
                ws.Cells[0, 2].Value = "MutID";
                numCol = 3;
                foreach (var item in listPerson[0].ListPerson)
                {
                    ws.Cells[0, numCol++].Value =  item.Name;
                }
                int length = numCol;
                CellStyle tmpStyle = new CellStyle();
                tmpStyle.Borders.SetBorders(MultipleBorders.All | MultipleBorders.Inside | MultipleBorders.Top, Color.Black, LineStyle.Thin);

                tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                tmpStyle.FillPattern.SetSolid(Color.LightBlue);
                tmpStyle.Font.Weight = ExcelFont.BoldWeight;
                tmpStyle.WrapText = false;//true אם השורות יגלשו..

                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Style = tmpStyle;//18
                ws.Cells.GetSubrangeAbsolute(1,0, listPerson.Count, 2).Style = tmpStyle;//8
                tmpStyle = new CellStyle();
                tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                tmpStyle.Font.Weight = ExcelFont.BoldWeight;
                i = 1; j = 2;
                var ab = 'A';
                CellStyle tmpStyleColor = new CellStyle();
                tmpStyleColor.Borders.SetBorders(MultipleBorders.All | MultipleBorders.Inside | MultipleBorders.Top, Color.Black, LineStyle.Thin);

                foreach (var item in listPerson)
                {
                    ws.Cells[i, 0].Value = item.DyDis;
                    ws.Cells[i, 1].Value = item.DyMut;
                    ws.Cells[i, 2].Value = item.MutID;
                    if (item.ColorDyName != null && item.ColorDyName != "")
                    {
                        ws = setColorToFinal9(tmpStyleColor, ws, i, 0, item.ColorDyName, numCol);
                    }
                    else
                    {
                        tmpStyle.FillPattern.SetSolid(Color.LightBlue);
                        ws.Cells.GetSubrangeAbsolute(0, 0, 0, numCol).Style = tmpStyle;
                    }
                    numCol = 3;
                    foreach (var it in item.ListPerson)
                    {
                        if (it.Color != null)
                        {
                            if (it.Color != null)
                            {
                                ws = setColorResult(tmpStyleColor, ws, i, numCol, it.Color, numCol);
                            }
                        }
                        ws.Cells[i, numCol++].Value = it.Results;
                    }

                    i++;
                }
                CellRange range = ws.Cells.GetSubrange("A1:O" + (listPerson.Count + 1));
                range.Style.Borders.SetBorders(MultipleBorders.Inside, Color.Black, LineStyle.Thin);
                range.Style.Borders.SetBorders(MultipleBorders.All, Color.Black, LineStyle.Thin);
                path = ConfigurationManager.AppSettings["PathSavefile"] + idShipping + "(" + listPerson[0].DyDis + ")" + "-final9.xlsx";
                ef.Save(path);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return path;
        }
        public static ExcelWorksheet setColorResult(CellStyle tmpStyle, ExcelWorksheet ws, int i,int j, string color, int numCol)
        {
            switch (color) {
                case "Blue": tmpStyle.FillPattern.SetSolid(Color.LightSkyBlue);
                    break;
                case "Red":
                    tmpStyle.FillPattern.SetSolid(Color.Red);
                    break;
                case "Yellow":
                   tmpStyle.FillPattern.SetSolid(Color.Yellow);
                    break;
                case "Grey":
                    tmpStyle.FillPattern.SetSolid(Color.LightGray);
                    break;
                case "Green":
                    tmpStyle.FillPattern.SetSolid(Color.LightGreen);
                    break;
                case "Pink":
                    tmpStyle.FillPattern.SetSolid(Color.HotPink);
                    break;
                case "palegreen":
                    tmpStyle.FillPattern.SetSolid(Color.PaleGreen);
                    break;
                case "deepskyblue":
                    tmpStyle.FillPattern.SetSolid(Color.DeepSkyBlue);
                    break;
                case "Bordeaux":
                    tmpStyle.FillPattern.SetSolid(Color.MediumPurple);
                    break;
                case "Orange":
                    tmpStyle.FillPattern.SetSolid(Color.Orange);
                    break;
                case "antiquewhite":
                    tmpStyle.FillPattern.SetSolid(Color.LavenderBlush);
                    break;
                default: tmpStyle.FillPattern.SetSolid(Color.White);
                    break;
            }
            ws.Cells.GetSubrangeAbsolute(i, j, i, j).Style = tmpStyle;
            
            return ws;
        }
        public static ExcelWorksheet setColor(CellStyle tmpStyle, ExcelWorksheet ws, int i, int j, string color,int numCol)
        {
            switch (color)
            {
                case "Blue":
                    tmpStyle.FillPattern.SetSolid(Color.LightSkyBlue);
                    break;
                case "deepskyblue":
                    tmpStyle.FillPattern.SetSolid(Color.DeepSkyBlue);
                    break;
                case "Red":
                    tmpStyle.FillPattern.SetSolid(Color.Red);
                    break;
                case "Yellow":
                    tmpStyle.FillPattern.SetSolid(Color.Yellow);
                    ws.Cells.GetSubrangeAbsolute(i, 9, i, 13).Style = tmpStyle;
                    return ws;
                case "Orange":
                    tmpStyle.FillPattern.SetSolid(Color.Orange);
                    break;
                case "Pink":
                    tmpStyle.FillPattern.SetSolid(Color.HotPink);
                    break;
                case "Green":
                    tmpStyle.FillPattern.SetSolid(Color.LightGreen);
                    break;
                case "palegreen":
                    tmpStyle.FillPattern.SetSolid(Color.PaleGreen);
                    break;
            }
            ws.Cells.GetSubrangeAbsolute(i, j, i, j + 2).Style = tmpStyle;
            return ws;
        }
        public static ExcelWorksheet setColorToFinal9(CellStyle tmpStyle, ExcelWorksheet ws, int i, int j, string color, int numCol)
        {
            switch (color)
            {
                case "Blue":
                    tmpStyle.FillPattern.SetSolid(Color.LightSkyBlue);
                    break;
                case "deepskyblue":
                    tmpStyle.FillPattern.SetSolid(Color.DeepSkyBlue);
                    break;
                case "Red":
                    tmpStyle.FillPattern.SetSolid(Color.Red);
                    break;
                case "Yellow":
                    tmpStyle.FillPattern.SetSolid(Color.Yellow);
                    ws.Cells.GetSubrangeAbsolute(i, 9, i, 13).Style = tmpStyle;
                    return ws;
                case "Pink":
                    tmpStyle.FillPattern.SetSolid(Color.HotPink);
                    break;
                case "Green":
                    tmpStyle.FillPattern.SetSolid(Color.LightGreen);
                    break;
                case "palegreen":
                    tmpStyle.FillPattern.SetSolid(Color.PaleGreen);
                    break;
                case "Orange":
                    tmpStyle.FillPattern.SetSolid(Color.Orange);
                    break;
            }
             ws.Cells.GetSubrangeAbsolute(i, j, i, j + 2).Style = tmpStyle;
            return ws;
        }
    }

}
