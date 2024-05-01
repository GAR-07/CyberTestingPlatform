namespace CyberTestingPlatform.Core.Shared
{
    public class CustomHttpException : Exception
    {
        public int StatusCode { get; private set; }

        public CustomHttpException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
