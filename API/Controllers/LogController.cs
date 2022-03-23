using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/logging")]
    [ApiController]
    public class LogController : ControllerBase
    {
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] LogEntry value)
        {
            IActionResult ret;

            // TODO: Write code to store logging data in a database table

            // Return OK for now
            ret = Ok(true);

            return ret;
        }
    }
}
