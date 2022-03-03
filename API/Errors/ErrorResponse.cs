using Microsoft.EntityFrameworkCore.Query.Internal;

namespace API.Errors
{
    // Generic error handling class 
    public class ErrorResponse
    {
        public ErrorResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            // Null coalescing operator
            // if null, execute whatever is on the right hand side of ??
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
#pragma warning disable CS8603 // Possible null reference return. 
            // I dont know how this could be null but go off i guess...
            return statusCode switch
            {
                // yoda talks funny
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. " +
                       "Anger leads to hate. Hate leads to Career change.",
                _ => null
            };
#pragma warning restore CS8603 // Possible null reference return.
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
