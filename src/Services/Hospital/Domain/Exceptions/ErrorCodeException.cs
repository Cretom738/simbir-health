namespace Domain.Exceptions
{
    public class ErrorCodeException : Exception
    {
        public int ErrorCode { get; }

        public ErrorCodeException(int errorCode)
            : base(string.Empty)
        {
            ErrorCode = errorCode;
        }

        public ErrorCodeException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
