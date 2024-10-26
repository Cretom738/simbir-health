using System.Net;

namespace Domain.Exceptions
{
    public class ConflictException : ErrorCodeException
    {
        public ConflictException() 
            : base((int)HttpStatusCode.Conflict)
        {
        }

        public ConflictException(string message)
            : base((int)HttpStatusCode.Conflict, message)
        {
        }
    }
}
