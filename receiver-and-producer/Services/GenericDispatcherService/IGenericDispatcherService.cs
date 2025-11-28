namespace receiver_and_producer.Services.GenericDispatcherService
{
    public interface IGenericDispatcherService
    {
        Task DispatchAsync(string partnerName, object payload);
    }
}
