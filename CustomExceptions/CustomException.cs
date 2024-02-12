namespace NetCore.CustomExceptions
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; set; }

        public CustomException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}