using Newtonsoft.Json;
using NGSData;
using NGSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NGSController.Controllers
{
    //[AuthorizeRequest]
    public class uploadLevel0Controller : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string ListPlate)
        {
            HttpResponseMessage massage = null;
            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            try
            {
                var filters = JsonConvert.DeserializeObject<Run>(ListPlate);
                MainLevel0 m0 = new MainLevel0(filters);

                // MainLevel0 m0 = new MainLevel0(new List<string> { "MBX003169", "MBX003171", "MBX003175", "MBX003187" });
                var path = ConnectSqlLevel0.SelectTemplateToDataTable(m0.ListLevel0, filters.NumRun);/* m0.IdNumberingRuns"001"*/
                massage = SaveLevel2Controller.download(path, (filters.BarcodeCantrige + ".csv"), "vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //    (m0.IdNumberingRuns+"(" + ListPlate[0]  + ")" + " - Step1.csv")
            }
            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);

            }
            return massage;
        }
        public string Post(int id, [FromBody] string ListPlate)
        {
            string IdNumberingRuns;
           var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                ReasonPhrase = ""
            };
            try
            {
                var filters = JsonConvert.DeserializeObject<Run>(ListPlate);
                IdNumberingRuns = ConnectSqlLevel0.InsertIdCantrigeAndSelectRuns(filters.BarcodeCantrige);
                IdNumberingRuns = IdNumberingRuns.ToString().PadLeft(5, '0');
            }
            catch (Exception e)
            {
                response.ReasonPhrase = e.Message;
                throw new HttpResponseException(response);

            }
            return IdNumberingRuns;
        }
    }
}
