using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // This class will handle any exception that is thrown during production
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // this method is pretty much getting called at all points of a request
            // when a request comes through, we'll chck for an exception at that step.
            //
            // If no exception is thrown, we move onto the next step
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // any exception that arises will be caught here and handled wiht our ErrorHandling class
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                ApiException response = _env.IsDevelopment()
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    // If we're in development, send back the internalServerError code, the message and the stack trace
                    ? new ApiException((int) HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int) HttpStatusCode.InternalServerError, ex.Message);
                    // if not in dev mode, send back the same thing without a stack trace
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    // default is pascal case, but we want to be consistent with other responses 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
