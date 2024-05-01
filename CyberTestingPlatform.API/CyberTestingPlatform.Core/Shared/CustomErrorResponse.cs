namespace CyberTestingPlatform.Core.Shared
{
    public class CustomErrorResponse(string message, int statusCode)
    {
        public string Message { get; set; } = message;
        public int StatusCode { get; set; } = statusCode;
    }
}
