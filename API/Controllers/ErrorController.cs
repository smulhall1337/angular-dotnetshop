using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{statusCode}")] // overwrite the rout for this controller 
    [ApiExplorerSettings(IgnoreApi = true)] // tells swagger to ignore this controller in its documentation
    public class ErrorController : BaseApiController
    {
        // Generic controller used for outputting errors that aren't thrown from our code
        // eg. A user tries to hit an endpoint that doesn't exist
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ErrorResponse(statusCode));
        }
    }
}
