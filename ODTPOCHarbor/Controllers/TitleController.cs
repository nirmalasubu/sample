using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODTPOCHarbor.Controllers
{
    [Route("api/[controller]")]
    public class TitleController : Controller
    {
        // GET: /<controller>/
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            LogzIOServiceHelper log = new LogzIOServiceHelper();
            log.Send(new Dictionary<string, object>() { { "message", "retrieved title information" } });

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://flow.tbs.io");
           
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("/v2/title/since/" + id + "?api_key=871ab820-4f0a-11e4-bb1f-e3da723a5795").Result;
            if (response.IsSuccessStatusCode)
            {              
                
                return new JsonResult(response.Content.ReadAsStringAsync().Result);
                
            }

          

            return new JsonResult("Couldn't find anything since "+id);

        }
    }
}
