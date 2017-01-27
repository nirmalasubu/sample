using Newtonsoft.Json.Linq;
using System;

namespace OnDemandTools.API.Tests.Helpers
{
    public class AiringObjectHelper
    {
        public string UpdateAiringId(string airingId,string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
           
            jsonObject.Add("AiringId",airingId);
       
            return jsonObject.ToString();
        }
       
        public string UpdateDates(string jsonString, int noOfDaysBefore)
        {
            JObject jObject = JObject.Parse(jsonString);

            JArray jArray = (JArray)jObject.SelectToken("Flights");
           
            foreach (JObject obj in jArray)
            {
                obj["Start"] = DateTime.UtcNow.AddDays(noOfDaysBefore);
                obj["End"] = DateTime.UtcNow.AddDays(noOfDaysBefore).AddDays(7);

                noOfDaysBefore = noOfDaysBefore + 7;
            }
            return jObject.ToString();
           
        }
    }
}
