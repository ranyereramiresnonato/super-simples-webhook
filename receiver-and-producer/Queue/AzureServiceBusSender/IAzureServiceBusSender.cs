using receiver_and_producer.Dtos;

namespace receiver_and_producer.Queue.AzureServiceBusSender
{
    public interface IAzureServiceBusSender
    {
        Task SendMessageAsync(QueueMessageDTO message);
    }
}
