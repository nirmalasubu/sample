using FluentScheduler;
using System.Collections.Generic;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using ODTPOCHarbor.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace ODTPOCHarbor
{


    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
           

            Schedule(() =>
            {

                LogzIOServiceHelper logg = new LogzIOServiceHelper();

                try
                {
                    logg.Send(new Dictionary<string, object>() { { "message", "starting queue checker" } });
                    ApplicationDbContext ap = new ApplicationDbContext();


                    var factory = new ConnectionFactory();
                    factory.Uri = "amqp://pnczwvbj:yEkp12WO-wYNOrdlmy5Qto2YhKLRUMUH@blossom-monkey.rmq.cloudamqp.com/pnczwvbj";
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {


                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            try
                            {
                                JustCheck ne = new JustCheck() { Airing = message, PostMessage = message };
                                ap.Add(ne);
                                ap.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                logg.Send(new Dictionary<string, object>() { { "message", "couldn't send message to azure sql " + ex } });

                            }

                            logg.Send(new Dictionary<string, object>() { { "message", message } });
                        };
                        channel.BasicConsume(queue: "b54bb65e-11bd-4683-9fd8-4e65c0444d81",
                                             noAck: true,
                                             consumer: consumer);
                        Thread.Sleep(Timeout.Infinite);

                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        logg.Send(new Dictionary<string, object>() { { "message", "couldn't send message to azure sql " + ex } });

                    }
                    catch (Exception)
                    {
                        logg.Send(new Dictionary<string, object>() { { "message", "error with mailbox " } });
                 
                    }

                }
                
            }).ToRunOnceIn(30).Seconds();

           
        }
    }
}
