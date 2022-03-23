using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppSubmitterLoggerController : ControllerBase
    {
        private FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
        private readonly ILogger<AppSubmitterLoggerController> _logger;
        private readonly AppSubmitterLoggerService _appSubmitterLoggerService;

        public AppSubmitterLoggerController (ILogger<AppSubmitterLoggerController> logger, AppSubmitterLoggerService appSubmitterLogger)
        {
            _logger = logger;
            _appSubmitterLoggerService = appSubmitterLogger;
        }

        [HttpGet]
        public IActionResult GetApplicationSubmitterLoggers()
        {
            try
            {
                IEnumerable<ApplicationSubmitterLogger> result = _appSubmitterLoggerService.GetApplicationSubmitterLoggers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetApplicationSubmitterLoggers: "+ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ApplicationSubmitterLogger GetApplicationSubmitterLogger(int id)
        {
            ApplicationSubmitterLogger applicationsubmitterlogger = db.ApplicationSubmitterLogger.Find(id);
            if (applicationsubmitterlogger == null)
            {
               // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return applicationsubmitterlogger;
        }

        [HttpGet]
        public ApplicationSubmitterLogger GetLastApplication()
        {
            DateTime today = DateTime.Now;
            ApplicationSubmitterLogger applicationsubmitterlogger = db.ApplicationSubmitterLogger.Where(x => x.EntryDate <= today).FirstOrDefault();
            if (applicationsubmitterlogger == null)
            {
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return applicationsubmitterlogger;
        }

        [HttpGet]
        public IEnumerable<ApplicationSubmitterLogger> GetLogsBySourceName(string source)
        {
            DateTime today = DateTime.Now;
            IEnumerable<ApplicationSubmitterLogger> applicationsubmitterlogger = db.ApplicationSubmitterLogger.Where(x => x.AppSource == source).AsEnumerable();
            if (applicationsubmitterlogger == null)
            {
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return applicationsubmitterlogger;
        }
    }
}

