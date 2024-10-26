using System.Net;

namespace Domain.Exceptions
{
    public class ForbiddenException : ErrorCodeException
    {
        public ForbiddenException()
            : base((int)HttpStatusCode.Forbidden)
        {
        }

        public ForbiddenException(string message)
            : base((int)HttpStatusCode.Forbidden, message)
        {
        }
    }
}
