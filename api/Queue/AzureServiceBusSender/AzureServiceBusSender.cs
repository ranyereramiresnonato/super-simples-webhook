using Azure.Messaging.ServiceBus;
using api.Dtos;
using System.Text.Json;

namespace api.Queue.AzureServiceBusSender
{
    public class AzureServiceBusSender : IAzureServiceBusSender
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public AzureServiceBusSender(ServiceBusClient client, IConfiguration config)
        {
            _client = client;
            var queue = config.GetValue<string>("AzureServiceBus:QueueName");
            _sender = _client.CreateSender(queue);
        }

        public async Task SendMessageAsync(QueueMessageDTO message)
        {
            string json = JsonSerializer.Serialize(message);
            await _sender.SendMessageAsync(new ServiceBusMessage(json));
        }
    }
}
