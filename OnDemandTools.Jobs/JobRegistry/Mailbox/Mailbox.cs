using Newtonsoft.Json;
using OnDemandTools.Business.Modules.AiringPublisher.Models;
using OnDemandTools.Common.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        AppSettings appsettings;
        Serilog.ILogger logger;
        Delivery _delivery;

        public Mailbox(AppSettings appsettings,
                       Serilog.ILogger logger)
        {
            this.appsettings = appsettings;
            this.logger = logger;
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
            //using (var db = new OnDemandReportingEntities())
            //{
            //    if (airingMessage.Action.Equals("Delete"))
            //    {
            //        var existingAiring = db.Airings.SingleOrDefault(a => a.AiringId == airingMessage.AiringId);

            //        if (existingAiring != null)
            //        {
            //            db.Airings.Remove(existingAiring);
            //        }
            //    }
            //    else if (airingMessage.Action.Equals("Modify"))
            //    {
            //        var airingView = GetAiringBy(airingMessage.AiringId);

            //        if (airingView == null)
            //        {
            //            Logger.Error(string.Format("Airing not found: {0}", airingMessage.AiringId));
            //            return;
            //        }

            //        var airingData = MapAiring(airingView);

            //        var existingAiring = db.Airings.SingleOrDefault(a => a.AiringId == airingView.AiringId);

            //        if (existingAiring != null)
            //        {
            //            db.Airings.Remove(existingAiring);
            //        }

            //        db.Airings.Add(airingData);
            //    }

            //    db.SaveChanges();
            //}
        }
    }
}
