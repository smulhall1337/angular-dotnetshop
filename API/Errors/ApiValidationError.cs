namespace API.Errors
{
    // This error will be thrown when the user provides incorrect parameters in a request
    // i.e. a user, when logging in, does not provide a user name or password (or both)
    public class ApiValidationError : ApiException
    {
        public ApiValidationError() : base(400)
        {

        }

        public IEnumerable<string> Errors { get; set; }
    }
}
