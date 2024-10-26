namespace Domain.Interfaces
{
    public interface IPublisher
    {
        Task Publish<T>(T eventData) where T : class;
    }
}
