using DotNetEnv;
using Rabbit.Consumer.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

Env.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env"));
var consumerRepository = new ConsumerRepository();

string message = JsonSerializer.Serialize(consumerRepository.GetMessageFromQueue("rabbitMessagesQueue"));

Console.WriteLine(message);