using api.Dtos;
using api.Queue.AzureServiceBusSender;

namespace api.Services.GenericDispatcherService
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
