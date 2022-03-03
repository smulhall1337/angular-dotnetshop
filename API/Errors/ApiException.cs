namespace API.Errors
{
    // If a server side exception is generated (50x), we want to return something that looks
    // a little better than a huge stack trance 
    public class ApiException : ErrorResponse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}
