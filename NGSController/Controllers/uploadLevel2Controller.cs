using NGSData;
using NGSService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NGSController.Controllers
{
    //[AuthorizeRequest]
    public class uploadLevel2Controller : ApiController
    {
        [HttpPost]
        public long Post()/*Production[FromBody] string ListPlate HttpResponseMessage*/
        {
            Production p = new Production();
            //var nameFile = "";
            var httpRequest = HttpContext.Current.Request;
            long idPath = 0;
            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    var nameNewFile = Guid.NewGuid().ToString() + " - " + DateTime.Now.Ticks;
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/files/") + nameNewFile);
                    idPath = ConnectSqlLevel1.InsertPathFile(HttpContext.Current.Server.MapPath("~/files/") + nameNewFile);
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/files/");
                        var name = System.IO.Path.GetFileName(postedFile.FileName);
                        var t = System.IO.Path.Combine(filePath + nameNewFile, name);
                        postedFile.SaveAs(t);
                        //nameFile = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                        //switch (name)
                        //{
                        //    case "genotype_results.txt":
                        //        {
                        //            MainLevel1 m1 = new MainLevel1(t, p);
                        //            p.ListLevel1 = m1.ListLevel1;
                        //            break;
                        //        }
                        //    case "self-chained.bed":
                        //        {
                        //            MainLevel2 m2 = new MainLevel2(t);
                        //            p.ListLevel2 = m2.ListLevel2;
                        //            break;
                        //        }
                        //    case "insertions_results.txt":
                        //        {
                        //            MainLevel3 m3 = new MainLevel3(t);
                        //            p.ListLevel3 = m3.ListLevel3;
                        //            break;
                        //        }
                        //    case "Hap_insertions_results.txt":
                        //        {
                        //            MainLevel4 m4 = new MainLevel4(t);
                        //            p.ListLevel4 = m4.ListLevel4;
                        //            break;
                        //        }
                        //    case "deletion.bed":
                        //        {
                        //            MainLevel5 m5 = new MainLevel5(t);
                        //            p.ListLevel5 = m5.ListLevel5;
                        //            break;
                        //        }
                        //}
                    }
                    //    // Files.FileWriterLevel6Excel(p.ListLevel1, "", 1);
                    //    MainLevel6 list6 = new MainLevel6(p);
                    //    //p.RunPlates = ConnectSqlLevel0.SelectBarcodePlates(p.ListLevel1);
                    //    MainLevel9 list9 = new MainLevel9(p);
                }
                //foreach (var list in p.GetType().GetProperties())
                //{
                //    if (list.GetValue(p, null).ToString() == null)
                //        throw new Exception(list.GetValue(p, null).ToString() + " = null !");
                //}
            }
            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);

            }
            return idPath;// p;
        }


        [HttpPost]
        public Results GetProduction(int idPath, string barcodePlate)
        {
            Production p = new Production();
            Results r = new Results();
            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            string path = null;
            try
            {
                path = ConnectSqlLevel1.SelectPathFile(idPath);
                var docfiles = new List<string>();
                foreach (string file in Directory.GetFiles(path))
                {
                    var name = System.IO.Path.GetFileName(file);
                    switch (name)
                    {
                        case "genotype_results.txt":
                            {
                                MainLevel1 m1 = new MainLevel1(file, p, barcodePlate);
                                p.ListLevel1 = m1.ListLevel1;
                                break;
                            }
                        case "self-chained.bed":
                            {
                                MainLevel2 m2 = new MainLevel2(file);
                                p.ListLevel2 = m2.ListLevel2;
                                break;
                            }
                        case "insertions_results.txt":
                            {
                                MainLevel3 m3 = new MainLevel3(file);
                                p.ListLevel3 = m3.ListLevel3;
                                break;
                            }
                        case "Hap_insertions_results.txt":
                            {
                                MainLevel4 m4 = new MainLevel4(file);
                                p.ListLevel4 = m4.ListLevel4;
                                break;
                            }
                        case "deletion.bed":
                            {
                                MainLevel5 m5 = new MainLevel5(file);
                                p.ListLevel5 = m5.ListLevel5;
                                break;
                            }
                        case "mpileup_insertions_results.txt":
                            {
                                MainMpileupInsertions mMI = new MainMpileupInsertions(file);
                                p.ListMpileupInsertions = mMI.ListMpileupInsertions;
                                break;
                            }
                    }
                }

                // Files.FileWriterLevel6Excel(p.ListLevel1, "", 1);
                MainLevel6 list6 = new MainLevel6(p);

                //p.RunPlates = ConnectSqlLevel0.SelectBarcodePlates(p.ListLevel1);
                MainLevel9 list9 = new MainLevel9(p);

                foreach (var list in p.GetType().GetProperties())//.Where(p => !p.GetGetMethod().GetParameters().Any() && !p.Name.Equals("BarcodeCantrige") && !p.Name.Equals("NumRun")
                {
                    if (list.GetValue(p, null).ToString() == null)
                        throw new Exception(list.GetValue(p, null).ToString() + " = null !");
                }

                if (ConnectSqlLevel0.SamplesNotInStep1 != null)
                    r.MassegeSamplesNotInStep1 = "the samples : " + ConnectSqlLevel0.SamplesNotInStep1 + "not in step 1 ";
                r.ResultProduction = p;
            }
            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);
            }
            finally
            {
                System.IO.Directory.Delete(path, true);
                ConnectSqlLevel1.DeletePathFile(idPath);
            }
            //ConnectSqlLevel2.InsertAllListPerson(m.ListLevel2);
            return r;
        }
    }
}
