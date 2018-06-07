using Newtonsoft.Json;
using NGSData;
using NGSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace NGSController.Controllers
{
    public class SaveLevel2Controller : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Save(int id,string idRun,[FromBody] string listPerson)
        {
            HttpResponseMessage massage = null;
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
                    var filters = JsonConvert.DeserializeObject<List<Level1>>(listPerson);

                    massage = send(id, idRun, filters);

                }
            }

            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);
            }
            return massage;
        }
        [STAThread]
        public static HttpResponseMessage send(int id,string idRun, List<Level1> filters)
        {

            HttpResponseMessage massage = null;
            try
            {
                var path = "";
                if (id == 1)
                {
                    path = Files.FileWriterLevel6(filters, "", 1);//write to excel
                    massage = download(path, ("(" + idRun + ")" + " - Summary.csv"), "vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
                else
                {
                    path = Files.FileWriterLevel6Excel(filters, "", 1);
                    massage = download(path, ("(" + idRun + ")" + " - Summary.xlsx"), "vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
            }
            return massage;
        }
        public static HttpResponseMessage download(string path, string fileName, string type = "octet-stream")
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            if (path == "")
            {
                result.Content = null;
            }
            else
            {
                try
                {
                    var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(string.Format("application/{0}", type));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    };
                }
                catch (Exception e)
                {
                    response.ReasonPhrase = e.Message;
                    throw new HttpResponseException(response);
                }
            }
            return result;
        }

    }
}
