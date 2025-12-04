namespace api.Services.GenericDispatcherService
{
    public interface IGenericDispatcherService
    {
        Task DispatchAsync(string partnerName, object payload);
    }
}
