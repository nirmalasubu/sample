using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.Tests.Helpers
{
    public class AiringObjectHelper
    {
       

        public AiringObjectHelper()
        {

        }
        public string UpdateAiringId(string airingId,string jsonString)
        {

            JObject jsonObject = JObject.Parse(jsonString);
           
            jsonObject.Add("AiringId",airingId);
       
            return jsonObject.ToString();

        }

      

        public string UpdateDates(string jsonString, int noOfDaysBefore)
        {
            JObject s = JObject.Parse(jsonString);

            JArray j = (JArray)s.SelectToken("Flights");
           

            foreach (JObject obj in j)
            {
              
                obj["Start"] = DateTime.UtcNow.AddDays(noOfDaysBefore);
                obj["End"] = DateTime.UtcNow.AddDays(noOfDaysBefore).AddDays(7);

                noOfDaysBefore = noOfDaysBefore + 7;
            }
            return s.ToString();
           
        }
    }
}
