using System.Net;

namespace Domain.Exceptions
{
    public class BadRequestException : ErrorCodeException
    {
        public BadRequestException() 
            : base((int)HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message) 
            : base((int)HttpStatusCode.BadRequest, message)
        {
        }
    }
}
