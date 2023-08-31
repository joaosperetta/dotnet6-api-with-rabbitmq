using DotNetEnv;
using Rabbit.Models;
using Rabbit.Models.Entities;
using Rabbit.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Rabbit.Consumer.Repositories
{
    public class ConsumerRepository
    {
        private readonly IConnection _connection;

        public ConsumerRepository()
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
            };

            _connection = factory.CreateConnection();
        }

        public RabbitMessage GetMessageFromQueue(string queue)
        {
            var channel = _connection.CreateModel();
            RabbitMessage? message = new RabbitMessage();

            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var stringMessage = Encoding.UTF8.GetString(body);

                message = JsonSerializer.Deserialize<RabbitMessage>(stringMessage);
            };

            return message;
        }
    }

}

