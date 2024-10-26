namespace Domain.Interfaces
{
    public interface IRequester
    {
        Task<T> GetResponse<TRequest, T>(TRequest request)
            where TRequest : class
            where T : class;
    }
}
