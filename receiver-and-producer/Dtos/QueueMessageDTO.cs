namespace receiver_and_producer.Dtos
{
    public sealed record QueueMessageDTO(string DestinationIdentifier, object Payload);
}
