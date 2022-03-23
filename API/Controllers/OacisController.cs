using API.Dto;
using API.Dto.Oacis;
using Core.Entities.Dhr;
using Core.Entities.Oacis;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OacisController : ControllerBase
    {
        string APIUrl = "";
        //System.Configuration.ConfigurationManager.AppSettings["OacisServicesAPIUrl"] + "/";

        private readonly ILogger<OacisController> _logger;
        private readonly OacisService _oacisService;

        public OacisController(ILogger<OacisController> logger, OacisService oacisService)
        { 
            _logger = logger;
            _oacisService = oacisService;
        }

        [HttpPost]
        public IActionResult GetLinkedCase([FromBody] Guid userId)
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
        public IActionResult LinkCase(OacisLinkCase dto)
        {
            try
            {
                string result = _oacisService.LinkCase(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UnlinkCase(OacisLinkCaseDto dto)
        {
            try
            {
                string result = _oacisService.UnlinkCase(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost]
        public IActionResult RepresentativeCanViewCase(RepresentativeCanViewCaseDto dto)
        {
            try
            {
                string result = _oacisService.RepresentativeCanViewCase(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        public IActionResult GetCaseFoodAssistanceWorkerEmail([FromBody] string caseNumber)
        {
            try
            {
                string result = _oacisService.GetCaseFoodAssistanceWorkerEmail(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
        [HttpPost]
        public IActionResult GetCountyOffice([FromBody]string caseNumber)
        {
            try
            {
                MailingAddress result = _oacisService.GetCountyOffice(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCountyOfficePhone([FromBody]string caseNumber)
        {
            try
            {
                string result = _oacisService.GetCountyOfficePhone(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCaseAddresses(GetCaseAddressesDto dto)
        {
            try
            {
                MailingAddress result = _oacisService.GetCaseAddresses(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCaseStatus(string caseNumber)
        {
            try
            {
                string result = _oacisService.GetCaseStatus(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetNextRecertificationDate(string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetNextRecertificationDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCertificationStartDate([FromBody]string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetCertificationStartDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCertificationEndDate([FromBody]string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetCertificationEndDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetHeadOfHouseholdName([FromBody]string caseNumber)
        {
            try
            {
                string result = _oacisService.GetHeadOfHouseholdName(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetCaseCounty([FromBody]string caseNumber)
        {
            try
            {
                string result = _oacisService.GetCaseCounty(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public IActionResult GetVerificationItems([FromBody]string caseNumber)
        {
            try
            {
                VerificationItemDto[] result = _oacisService.GetVerificationItems(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetAuthorizedReps([FromBody] string caseNumber)
        {
            try
            {
                AuthRepDto[] result = _oacisService.GetAuthorizedReps(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetAllotmentHistory([FromBody]string caseNumber)
        {
            try
            {
                AllotmentDto[] result = _oacisService.GetAllotmentHistory(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetIncomeLimit([FromBody] string caseNumber)
        {
            try
            {
                decimal result = _oacisService.GetIncomeLimit(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetTotalChildSupportPayments([FromBody] string caseNumber)
        {
            try
            {
                decimal result = _oacisService.GetTotalChildSupportPayments(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetIncomesAndChildSupport([FromBody] string caseNumber)
        {
            try
            {
                IncomeExpenseEntryDto[] result = _oacisService.GetIncomesAndChildSupport(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetNextAppointment(string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetNextAppointment(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetOfficePhone(string caseNumber)
        {
            try
            {
                string result = _oacisService.GetOfficePhone(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetSemiAnnualReviewDate([FromBody] string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetSemiAnnualReviewDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetStatusDate(string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetStatusDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetAnnualReviewDate(string caseNumber)
        {
            try
            {
                DateTime result = _oacisService.GetAnnualReviewDate(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetHouseholdNames(string caseNumber)
        {
            try
            {
                string[] result = _oacisService.GetHouseholdNames(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetReviewCode(string caseNumber)
        {
            try
            {
                string result = _oacisService.GetReviewCode(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetClosureCode(string caseNumber)
        {
            try
            {
                string result = _oacisService.GetClosureCode(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UserCanAccessVerifications([FromBody]string caseNumber)
        {
            try
            {
                bool result = _oacisService.UserCanAccessVerifications(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetSNAPVerification([FromBody] string caseNumber)
        {
            try
            {
                AllotmentDto result = _oacisService.GetSNAPVerification(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult UserCanAccessComplaint(string caseNumber)
        {
            try
            {
                bool result = _oacisService.UserCanAccessComplaint(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult UserIsOnDNAList(string SSN)
        {
            try
            {
                bool result = _oacisService.UserIsOnDNAList(SSN);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetUserIdByCaseNumber(string caseNumber)
        {
            try
            {
                Guid result = _oacisService.GetUserIdByCaseNumber(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //checkDate should be in MM/dd/yyyy format
        [HttpPost]
        public IActionResult CheckHoliday(string checkDate)
        {
            try
            {
                string result = _oacisService.CheckHoliday(checkDate).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
