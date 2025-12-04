namespace api.Dtos
{
    public sealed record QueueMessageDTO(string DestinationIdentifier, object Payload);
}
