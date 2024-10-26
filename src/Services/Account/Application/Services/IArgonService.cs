namespace Application.Services
{
    public interface IArgonService
    {
        string Hash(string password);

        bool Compare(string hash, string password);
    }
}
