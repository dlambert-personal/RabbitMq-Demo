using RabbitMQ.Client;
using System;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello2",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                bool stop = false;
                int id = 0;
                do 
                {
                    SendMessages(channel, id, 50);
                    id += 10;
                    System.Threading.Thread.Sleep(50);
                    stop = Console.KeyAvailable;
                } while (!stop) ;

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static void SendMessages(IModel channel, int seed, int count)
        {
            for (int i = seed; i < seed + count; i++)
            {
                string message = "Hello World " + i.ToString(); 
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello2",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
