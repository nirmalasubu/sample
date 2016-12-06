using FluentScheduler;
using Microsoft.AspNetCore.Hosting.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTPOCHarbor
{
    public class QueueChecker : IJob
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public QueueChecker()
        {
            
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    LogzIOServiceHelper log = new LogzIOServiceHelper();
                    log.Send(new Dictionary<string, object>() { { "message", "queue checker job stopped" } });
                    return;
                }

                try
                {
                    LogzIOServiceHelper logg = new LogzIOServiceHelper();
                    logg.Send(new Dictionary<string, object>() { { "message", "starting queue checker" } });

                    var factory = new ConnectionFactory();
                    factory.Uri = "amqp://pnczwvbj:yEkp12WO-wYNOrdlmy5Qto2YhKLRUMUH@blossom-monkey.rmq.cloudamqp.com/pnczwvbj";
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {

                        LogzIOServiceHelper log = new LogzIOServiceHelper();
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            log.Send(new Dictionary<string, object>() { { "message", message } });
                            Debug.WriteLine(" [x] Received {0}", message);
                        };
                        channel.BasicConsume(queue: "f3b775e8-1a63-49cf-8ea1-8001013efc26",
                                             noAck: true,
                                             consumer: consumer);



                        Debug.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    LogzIOServiceHelper log = new LogzIOServiceHelper();
                    log.Send(new Dictionary<string, object>() { { "message", "exception "+ex.Message } });
                    throw;
                }
               
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {

                LogzIOServiceHelper log = new LogzIOServiceHelper();
                log.Send(new Dictionary<string, object>() { { "message", "shutting down" } });
                _shuttingDown = true;
            }
          
        }
    }
}
