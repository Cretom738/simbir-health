using System.Net;

namespace Domain.Exceptions
{
    public class UnauthorizedException : ErrorCodeException
    {
        public UnauthorizedException()
            : base((int)HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message)
            : base((int)HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
