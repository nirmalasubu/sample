using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Jobs.Helpers;
using OnDemandTools.Jobs.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AiringLong = OnDemandTools.Business.Modules.Airing.Model.Airing;

namespace OnDemandTools.Jobs.JobRegistry.Mailbox
{
    enum Delivery
    {
        Sql,
        Ftp,
        Post
    }

    public class Mailbox
    {
        IAiringService airingSvc;
        AppSettings appsettings;
        Serilog.ILogger logger;
        Delivery _delivery;
        public static TimeZoneInfo TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public Mailbox(AppSettings appsettings,
                       Serilog.ILogger logger,
                       IAiringService _airingSvc)
        {
            this.appsettings = appsettings;
            this.logger = logger;
            airingSvc = _airingSvc;
        }

        public void Execute(string deliveryType)
        {
            if (!Enum.TryParse(deliveryType, out _delivery))
                logger.Error(string.Format("Incorrect delivery type. Try {0}, {1}, {2}", Delivery.Sql, Delivery.Post, Delivery.Ftp));

            SetupQueue();
        }

        private void SetupQueue(bool isPriorityQueue = false)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = appsettings.CloudQueue.MqUrl;
                using (IConnection conn = factory.CreateConnection())
                {
                    try
                    {
                        using (IModel model = conn.CreateModel())
                        {
                            try
                            {
                                bool noAck = false;
                                BasicGetResult result = model.BasicGet(appsettings.CloudQueue.MqQueue, noAck);
                                if (result == null)
                                {
                                    logger.Information(string.Format("Queue {0} is not available.", appsettings.CloudQueue.MqQueue));
                                }
                                else
                                {
                                    IBasicProperties props = result.BasicProperties;
                                    byte[] body = result.Body;
                                    ProcessMessage(result.Body, result.BasicProperties.Priority);
                                    // acknowledge receipt of the message
                                    model.BasicAck(result.DeliveryTag, false);
                                }
                            }
                            finally
                            {                                
                                model.Close();
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Mailbox exception");
                //throw;
            }
        }

        private void ProcessMessage(byte[] message, byte priority)
        {
            try
            {
                var messageInString = System.Text.Encoding.UTF8.GetString(message);
                var actualMessage = messageInString;

                if (messageInString.Contains("\\"))
                    messageInString = JsonConvert.DeserializeObject<string>(messageInString);

                var airingMessage = JsonConvert.DeserializeObject<QueueAiring>(messageInString);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("received message: {0}, priority: {1}", actualMessage, priority);
                Console.ResetColor();

                switch (_delivery)
                {
                    case Delivery.Sql:
                        DeliverToSqlDatabase(airingMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex,"Message not processed");
                Console.WriteLine(ex.Message);
            }
        }

        private void DeliverToSqlDatabase(QueueAiring airingMessage)
        {
            using (var db = new OnDemandReportingContext())
            {
                if (airingMessage.Action.Equals("Delete"))
                {
                    var existingAiring = db.Airing.SingleOrDefault(a => a.AiringId == airingMessage.AiringId);

                    if (existingAiring != null)
                    {
                        db.Airing.Remove(existingAiring);
                    }
                }
                else if (airingMessage.Action.Equals("Modify"))
                {
                    var airingView = GetAiringBy(airingMessage.AiringId);

                    if (airingView == null)
                    {
                        logger.Error(string.Format("Airing not found: {0}", airingMessage.AiringId));
                        return;
                    }

                    var airingData = MapAiring(airingView);

                    var existingAiring = db.Airing.SingleOrDefault(a => a.AiringId == airingView.AssetId);

                    if (existingAiring != null)
                    {
                        db.Airing.Remove(existingAiring);
                    }

                    db.Airing.Add(airingData);
                }

                db.SaveChanges();
            }
        }

        private AiringLong GetAiringBy(string airingId)
        {
            var result = airingSvc.GetBy(airingId);

            return result;
        }
        

        private static DateTime ConvertToBc(DateTime dateTime)
        {
            if (dateTime >= dateTime.Date && dateTime < dateTime.Date.AddHours(6))
                return dateTime.AddDays(-1);

            return dateTime;
        }

        private static DateTime ConvertToEst(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo);
        }

        private static DateTime? GetLinearDateTime(AiringLong airingView)
        {
            return (airingView.Airings.Any() && airingView.Airings.First().Linked) ? airingView.Airings.First().Date : null;
        }

        private static DateTime? GetEarliestStartDate(AiringLong airingView)
        {
            var flight = airingView.Flights.OrderBy(f => f.Start).FirstOrDefault();

            if (flight == null)
                return null;

            return flight.Start;
        }

        private static DateTime? GetLatestEndDate(AiringLong airingView)
        {
            var flight = airingView.Flights.OrderByDescending(f => f.End).FirstOrDefault();

            if (flight == null)
                return null;

            return flight.End;
        }

        private static Airing MapAiring(AiringLong airingView)
        {
            var airingStart = GetEarliestStartDate(airingView);
            var airingEnd = GetLatestEndDate(airingView);
            var linearStart = GetLinearDateTime(airingView);

            var airingData = new Airing
            {
                AiringId = airingView.AssetId,
                Name = airingView.Name,
                Type = airingView.Type,
                AiringTitles = MapTitleIds(airingView),
                AiringDestinations = MapDestinations(airingView),
                Brand = airingView.Network,
                StartDateTime = airingStart,
                EndDateTime = airingEnd,
                LinearDateTime = linearStart,
                ESTStartDateTime = airingStart.HasValue ? (DateTime?)ConvertToEst(airingStart.Value) : null,
                ESTEndDateTime = airingEnd.HasValue ? (DateTime?)ConvertToEst(airingEnd.Value) : null,
                ESTLinearDateTime = linearStart.HasValue ? (DateTime?)ConvertToEst(linearStart.Value) : null,
                BCStartDateTime = airingStart.HasValue ? (DateTime?)ConvertToBc(ConvertToEst(airingStart.Value)) : null,
                BCEndDateTime = airingEnd.HasValue ? (DateTime?)ConvertToBc(ConvertToEst(airingEnd.Value)) : null,
                BCLinearDateTime = linearStart.HasValue ? (DateTime?)ConvertToBc(ConvertToEst(linearStart.Value)) : null,
                UpdatedDateTime = DateTime.UtcNow
            };
            return airingData;
        }

        private static ICollection<AiringDestination> MapDestinations(AiringLong airing)
        {
            var destinations = new List<AiringDestination>();

            foreach (var flight in airing.Flights)
            {
                foreach (var destination in flight.Destinations)
                {
                    var existingDestination = destinations.FirstOrDefault(d => d.Destination == destination.Name);

                    if (existingDestination != null)
                    {
                        existingDestination.DestinationFlights.Add(new DestinationFlight
                        {
                            Destination = destination.Name,
                            StartDate = flight.Start,
                            EndDate = flight.End
                        });
                    }
                    else
                    {
                        destinations.Add(new AiringDestination
                        {
                            AiringId = airing.AssetId,
                            Destination = destination.Name,
                            DestinationFlights = new Collection<DestinationFlight> { new DestinationFlight
                            {
                                Destination = destination.Name,
                                StartDate = flight.Start,
                                EndDate = flight.End
                            }}
                        });
                    }
                }
            }

            return destinations;
        }

        private static ICollection<AiringTitle> MapTitleIds(AiringLong airing)
        {
            return airing.Title.TitleIds.Where(t => t.Authority.Equals("Turner")).Select(titleId => new AiringTitle
            {
                AiringId = airing.AssetId,
                TitleId = int.Parse(titleId.Value)

            }).ToList();
        }
    }
}
