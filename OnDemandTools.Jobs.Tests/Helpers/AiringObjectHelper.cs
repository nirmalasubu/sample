﻿using Newtonsoft.Json.Linq;
using System;

namespace OnDemandTools.Jobs.Tests.Helpers
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
                obj["Start"] = DateTime.UtcNow.Date.AddDays(noOfDaysBefore);
                obj["End"] = DateTime.UtcNow.Date.AddDays(noOfDaysBefore).AddDays(7);

                noOfDaysBefore = noOfDaysBefore + 7;
            }
            return jObject.ToString();
           
        }

         public string UpdateDeliverImmedialtely(string jsonString, int noOfDaysBefore, bool deliverImmmdiately)
        {
            
            JObject jObject = JObject.Parse(jsonString);
            JObject Instructions = jObject["Instructions"] as JObject;
            Instructions.Add("DeliverImmediately", true);
            jObject["Instructions"] = Instructions;
            return UpdateDates(jObject.ToString(), noOfDaysBefore);

        }
    }
}
