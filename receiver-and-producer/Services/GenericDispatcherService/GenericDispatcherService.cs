using receiver_and_producer.Dtos;
using receiver_and_producer.Queue.AzureServiceBusSender;

namespace receiver_and_producer.Services.GenericDispatcherService
{
    public class GenericDispatcherService : IGenericDispatcherService
    {
        private readonly IAzureServiceBusSender _azureServiceBusSender;
        public GenericDispatcherService(IAzureServiceBusSender AzureServiceBusSender)
        {
            _azureServiceBusSender = AzureServiceBusSender;
        }
        public async Task DispatchAsync(string partnerName, object payload)
        {
            var message = new QueueMessageDTO(partnerName, payload);
            await _azureServiceBusSender.SendMessageAsync(message);
        }
    }
}
