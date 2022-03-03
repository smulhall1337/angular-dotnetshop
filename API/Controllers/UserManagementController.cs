
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Snap.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserManagementController : ControllerBase
    // Note: The original app was made with its controllers extending 'Controller' rather than 'ControllerBase'
    //          This was due to it returning web page views with razor. But since our front end will be done in angular, we're not concerned with that.
    //          So instead of returning views, we can just return IActionResults and any relevant Json. See: https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0
    {

        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(ILogger<UserManagementController> logger)
        {
        }

        /// GET: UserManagement/IsAlive
        [AllowAnonymous]
        [HttpGet]
        public IActionResult IsAlive()
        {
            try
            {
                return Ok("I'm alive!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
