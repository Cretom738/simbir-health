using System.Net;

namespace Domain.Exceptions
{
    public class NotFoundException : ErrorCodeException
    {
        public NotFoundException()
            : base((int)HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string message)
            : base((int)HttpStatusCode.NotFound, message)
        {
        }
    }
}
