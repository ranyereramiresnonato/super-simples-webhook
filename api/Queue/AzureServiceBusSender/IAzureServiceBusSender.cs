using api.Dtos;

namespace api.Queue.AzureServiceBusSender
{
    public interface IAzureServiceBusSender
    {
        Task SendMessageAsync(QueueMessageDTO message);
    }
}
