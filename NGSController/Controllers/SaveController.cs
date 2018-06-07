using Newtonsoft.Json;
using NGSData;
using NGSService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NGSController.Controllers
{
    public class SaveController : ApiController
    {

        [HttpPost]
        public string Save([FromBody] string listPerson)
        {
            string massage = null;
            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            try
            {
                if (listPerson == null)
                {
                    throw new Exception("There was no data to save");
                }
                else
                {
                    var filters = JsonConvert.DeserializeObject<Production>(listPerson);
                    //  FileIntegrity.ifMoved(filters[0].IdFile, 2, true);//בודק אם עבר בשלבים
                    //   var ifOverwriteSave = FileIntegrity.ifSaveNextStep(filters[0].IdFile, 2, true);//בודק אם עבר בשלבים
                    //  if (ifOverwriteSave != false)
                    //  {
                    //     var search = ConnectDatabase.SelectAllTblSmaLevel2(filters[0].IdFile);
                    //     if (search.Count == 0)
                    //       {
                    InsertIntoDatabaseListPerson(filters);
                    massage = "The Runs " + filters.RunPlates.NumRun+" / " + filters.RunPlates.BarcodeCantrige + " added to table !";
                    //       }
                    //    else
                    //    {
                    //        massage = "false";
                    //    }
                    //}
                    //else massage = "overwrite";
                }
            }
            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);
            }
            return massage;
        }

        //להכניס נתונים לטבלה
        public static void InsertIntoDatabaseListPerson(Production list1)
        {
            SqlTransaction transaction;
            using (SqlConnection connection = new SqlConnection(ConnectSqlLevel1.connectionString))
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    list1.ListLevel1.ForEach(p1 => MainLevel1.InsertAllListPerson(p1,list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel2.ForEach(p2 => MainLevel2.InsertAllListPerson(p2, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel3.ForEach(p3 => MainLevel3.InsertAllListPerson(p3, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel4.ForEach(p4 => MainLevel4.InsertAllListPerson(p4, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel5.ForEach(p5 => MainLevel5.InsertAllListPerson(p5, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListMpileupInsertions.ForEach(p55 => MainMpileupInsertions.InsertAllListPerson(p55, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel6.ForEach(p6 => MainLevel6.InsertAllListPerson(p6, list1.RunPlates.NumRun, connection, transaction));
                    list1.ListLevel9.ForEach(p9 => MainLevel9.InsertAllListPerson(p9, list1.RunPlates.NumRun, connection, transaction));

                    //   ConnectSqlLevel1.InsertTblMovedLevel(connection, command, transaction, "1111");
                    // ConnectSqlLevel2.UpdateTblMovedLevelToNotMove(connection, command, transaction, "?????");  אחרי שיהיה שלב 1
                    // ConnectSqlLevel2.InsertIntoTblGroupPlates( con, cmd, transaction, idFiles)
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
                connection.Close();
            }
        }





    }
}
