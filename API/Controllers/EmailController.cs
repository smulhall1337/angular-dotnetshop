using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController: ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly EmailService _emailService;
        private readonly OacisService _oacisService;
        public EmailController(ILogger<EmailController> logger, EmailService emailService, OacisService oacisService)
        {
            _oacisService = oacisService;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult getLinkedCase([FromBody]Guid userId)
        {
            try
            {
                string result = _oacisService.GetLinkedCase(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult WriteLogEntry(EmailDto dto)
        {
            try
            {
                _emailService.WriteLogEntry(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchEmailAttachmentsByUserId([FromBody]Guid userId)
        {
            try
            {
                List<AttachmentFileDto> result = _emailService.FetchEmailAttachmentsByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
   
        [HttpPost]
        public IActionResult FetchEmailAttachmentByAttachmentId([FromBody] Guid attachmentId)
        {
            try
            {
                AttachmentFileDto result = _emailService.FetchEmailAttachmentByAttachmentId(attachmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult FetchEmailById([FromBody] Guid emailId)
        {
            try
            {
                EmailDto result = _emailService.FetchEmailById(emailId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult GetAnnualReportsFromDate([FromBody] DateTime date)
        {
            try
            {
                IEnumerable<EmailDto> result = _emailService.GetAnnualReportsFromDate(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPost]
        public IActionResult GetSemiAnnualReportsFromDate([FromBody] DateTime date)
        {
            try
            {
                IEnumerable<EmailDto> result = _emailService.GetSemiAnnualReportsFromDate(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult RemoveEmailAttachmentByAttachmentId([FromBody] Guid attachmentId)
        {
            try
            {
                bool result = _emailService.RemoveEmailAttachmentByAttachmentId(attachmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult sendEmail(EmailDto dto)
        {
            try
            {
                bool result = _emailService.sendEmail(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
