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
using Microsoft.EntityFrameworkCore;
using System.Text;
using OnDemandTools.Common.Extensions;

namespace OnDemandTools.Jobs.JobRegistry.Mailbox
{

    public class Mailbox
    {
        StringBuilder jobLogs = new StringBuilder();
        IAiringService airingSvc;
        AppSettings appsettings;
        Serilog.ILogger logger;
        public TimeZoneInfo TimeZoneInfo;

        public Mailbox(AppSettings appsettings,
                       Serilog.ILogger logger,
                       IAiringService _airingSvc)
        {
            this.appsettings = appsettings;
            this.logger = logger;
            airingSvc = _airingSvc;
            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);
        }

        public void Execute()
        {
            ProcessQueue();
        }

        private void ProcessQueue(bool isPriorityQueue = false)
        {
            try
            {
                AppendLogInfo(string.Format("Mailbox started processing queue {0}", appsettings.CloudQueue.ReportingQueueID));
                AppendLogInfo("Instantiating connection to rabbitmq");
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = appsettings.CloudQueue.MqUrl;
                using (IConnection conn = factory.CreateConnection())
                {
                    try
                    {
                        AppendLogInfo("Opening a channel");
                        using (var channel = conn.CreateModel())
                        {
                            try
                            {
                                AppendLogInfo(string.Format("Getting message count from queue {0}", appsettings.CloudQueue.ReportingQueueID));
                                // Get a snap shot of the message count at this given time. 
                                // Then sequentially  consume and process each message                                
                                uint messageCount = channel.MessageCount(appsettings.CloudQueue.ReportingQueueID);

                                if (messageCount <= 0)
                                    AppendLogInfo(string.Format("{0} messages in queue {1}. Nothing to process. Finishing the job.", messageCount, appsettings.CloudQueue.ReportingQueueID));
                                else
                                    AppendLogInfo(string.Format("{0} messages in queue {1}. Sequentially consuming and processing each message", messageCount, appsettings.CloudQueue.ReportingQueueID));

                                for (int i = 1; i <= messageCount; i++)
                                {
                                    // Fetching Individual Messages ("pull API")
                                    bool noAck = false;
                                    BasicGetResult result = channel.BasicGet(appsettings.CloudQueue.ReportingQueueID, noAck);
                                    IBasicProperties props = result.BasicProperties;
                                    byte[] body = result.Body;

                                    if (!result.Body.IsNullOrEmpty())
                                    {
                                        ProcessMessage(result.Body, result.BasicProperties.Priority);
                                        channel.BasicAck(result.DeliveryTag, false);
                                    }
                                }

                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                AppendLogInfo("Closing the channel");
                                channel.Close();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        AppendLogInfo("Closing the connection");
                        conn.Close();
                    }
                }

                AppendLogInfo(string.Format("Mailbox completed processing queue {0}", appsettings.CloudQueue.ReportingQueueID));
            }
            catch (Exception ex)
            {
                AppendLogInfo(string.Format("{0}. Mailbox abruptly stopped processing queue: {1}", ex, appsettings.CloudQueue.ReportingQueueID));
                logger.Error(ex, string.Format("{0}. Mailbox abruptly stopped processing queue: {1}", ex, appsettings.CloudQueue.ReportingQueueID));
            }
            finally
            {
                logger.Information(jobLogs.ToString());
                jobLogs.Clear();
            }
        }

        // Process message that was retrieved from queue
        private void ProcessMessage(byte[] message, byte priority)
        {
            var messageInString = System.Text.Encoding.UTF8.GetString(message);
            var actualMessage = messageInString;

            try
            {
                if (messageInString.Contains("\\"))
                    messageInString = JsonConvert.DeserializeObject<string>(messageInString);

                var airingMessage = JsonConvert.DeserializeObject<QueueAiring>(messageInString);
                DeliverToSqlDatabase(airingMessage);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, string.Format("{0}. Failure in Mailbox while processing message {1} from queue {2}", ex, messageInString, appsettings.CloudQueue.ReportingQueueID));
            }


        }

        // Append log messages for finally processing
        private void AppendLogInfo(string message)
        {
            jobLogs.AppendWithTime(message);
        }


        private void DeliverToSqlDatabase(QueueAiring airingMessage)
        {
            using (var db = new OnDemandReportingContext(appsettings.ReportingSqlDB.ConnectionString))
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
                        return;
                    }

                    var airingData = MapAiring(airingView);

                    var existingAiring = db.Airing.SingleOrDefault(a => a.AiringId == airingView.AssetId);

                    if (existingAiring != null)
                    {
                        db.Airing.Remove(existingAiring);
                        db.SaveChanges();
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


        private DateTime ConvertToBc(DateTime dateTime)
        {
            if (dateTime >= dateTime.Date && dateTime < dateTime.Date.AddHours(6))
                return dateTime.AddDays(-1);

            return dateTime;
        }

        private DateTime ConvertToEst(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo);
        }

        private DateTime? GetLinearDateTime(AiringLong airingView)
        {
            return (airingView.Airings.Any() && airingView.Airings.First().Linked) ? airingView.Airings.First().Date : null;
        }

        private DateTime? GetEarliestStartDate(AiringLong airingView)
        {
            var flight = airingView.Flights.OrderBy(f => f.Start).FirstOrDefault();

            if (flight == null)
                return null;

            return flight.Start;
        }

        private DateTime? GetLatestEndDate(AiringLong airingView)
        {
            var flight = airingView.Flights.OrderByDescending(f => f.End).FirstOrDefault();

            if (flight == null)
                return null;

            return flight.End;
        }

        private Airing MapAiring(AiringLong airingView)
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

        private ICollection<AiringDestination> MapDestinations(AiringLong airing)
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

        private ICollection<AiringTitle> MapTitleIds(AiringLong airing)
        {
            return airing.Title.TitleIds.Where(t => t.Authority.Equals("Turner")).Select(titleId => new AiringTitle
            {
                AiringId = airing.AssetId,
                TitleId = int.Parse(titleId.Value)

            }).ToList();
        }



        private void WaitForExitCommand()
        {
            Console.WriteLine("Type exit then press enter to close the app.");

            var command = string.Empty;

            while (!command.Equals("exit"))
            {
                command = Console.ReadLine() ?? string.Empty;
            }
        }
    }
}
