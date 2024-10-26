using Sodium;

namespace Application.Services
{
    public class ArgonService : IArgonService
    {
        public string Hash(string password)
        {
            return PasswordHash.ArgonHashString(password);
        }

        public bool Compare(string hash, string password)
        {
            return PasswordHash.ArgonHashStringVerify(hash, password);
        }
    }
}
