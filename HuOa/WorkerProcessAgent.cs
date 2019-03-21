using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuOa
{

    public class WorkerProcessAgent
    {

        private IWorkjob workJob;
        private IConsumer workConsumer;
        private bool stop;

        internal WorkerProcessAgent(IWorkjob job, IConsumer consumer)
        {
            this.workJob = job;
            this.workConsumer = consumer;
        }
        /// <summary>
        /// Lấy dữ liệu tạo task, chỉ chạy 1 lần 1 bản ghi
        /// </summary>
        public void Start()
        {
            while (!stop)
            {
                var item = workConsumer.Recieve();

            }

        }

        public void Init()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                }
            }
        }

        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = "";
                if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                {
                    Thread.CurrentThread.Name = DateTime.Now.Ticks.ToString();
                }
                message = Thread.CurrentThread.Name;
                Console.WriteLine(" [x] Received {0} {1}", message, Thread.CurrentThread.ManagedThreadId.ToString());
                Thread.Sleep(1000);
                Console.WriteLine(" [x] Done");
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);
            consumer.Shutdown += Consumer_Shutdown;
            channel.ModelShutdown += Channel_ModelShutdown;

        }

        private void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {

        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Chanel shutdown");
        }
    }
}
