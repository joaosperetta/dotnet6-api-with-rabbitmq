using DotNetEnv;
using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rabbit.Repositories
{
    public class RabbitMessageRepository : IRabbitMessageRepository
    {
        private readonly IConnection _connection;

        public RabbitMessageRepository()
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
            };

            _connection = factory.CreateConnection();

        }

        public void SendMessage(RabbitMessage message)
        {

            using var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: "rabbitMessagesQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "rabbitMessagesQueue",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
