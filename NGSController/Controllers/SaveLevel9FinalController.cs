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
    public class SaveLevel9FinalController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Save(string id, [FromBody] string listPerson)
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
                    var filters = JsonConvert.DeserializeObject<List<Level9>>(listPerson);
                   
                    massage = send(filters, id);
                   
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
        public static HttpResponseMessage send(List<Level9> filters,string idRun)
        {

            HttpResponseMessage massage = null;
            try
            {
                var path = "";
                path = Files.FileWriterLevel9FinalExcel(filters, "", 1);
                massage = SaveLevel2Controller.download(path, ("(" + idRun + ")" + " - final.xlsx"), "vnd.openxmlformats-officedocument.spreadsheetml.sheet");


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
       
    }
}
