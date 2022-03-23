using System.Security.Policy;
using System.Text;
using System.Xml.Linq;
using API.Dto;
using API.Dto.Application;
using AutoMapper;
using Core.Entities.Dhr;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;


/**
 * TODO:
 *  1. Create mapper profiles and reduce clutter
 *  2. Maybe simplify DTOs to be simpler on frontend and flatten
 */

namespace API.Controllers
{
    [ApiController]
    public class ApplicationController : BaseApiController
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ApplicationService applicationService, ILogger<ApplicationController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        // string APIUrl = System.Configuration.ConfigurationManager.AppSettings["OacisServicesAPIUrl"] + "/";

        [HttpPost]
        public IActionResult HouseholdContactGetCounty([FromBody]GeoCodeAddress address)
        {
            try
            {
                if (address != null)
                {
                    GoogleGeoCoder geocoder = new GoogleGeoCoder();
                    string county = geocoder.GetCounty(address);
                    return Ok(county);
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpGet("completedApplications")]
        public ActionResult<CompletedApplicationDto[]> GetCompletedApplications()
        {
            try
            {
                IEnumerable <CompletedApplication>  result = _applicationService.GetCompletedApplications();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CompletedApplicationDto[], IEnumerable<CompletedApplication>>());
                Mapper mapper = new Mapper(config);
                return Ok(mapper.Map<IEnumerable<CompletedApplication>, CompletedApplicationDto[]>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetCompletedApplications: "+ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("completedApplicationSummary")]
        public ActionResult<IEnumerable<GetCompletedApplicationSummaryDto>> GetCompletedApplicationSummary([FromBody]GetCompletedApplicationSummary args)
        {
            try
            {
                IEnumerable<CompletedApplicationSummary> result = _applicationService.GetCompletedApplicationSummary(args);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<IEnumerable<GetCompletedApplicationSummaryDto>, IEnumerable<CompletedApplicationSummary>>());
                Mapper mapper = new Mapper(config);
                return Ok(mapper.Map<IEnumerable<CompletedApplicationSummary>, IEnumerable<GetCompletedApplicationSummaryDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetCompletedApplicationSummary: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("completedApplication")]
        public ActionResult<CompletedApplicationDto> GetCompletedApplication([FromBody] Guid workflowId)
        {
            try
            {
                CompletedApplication result = _applicationService.GetCompletedApplication(workflowId);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CompletedApplication, CompletedApplicationDto>());
                Mapper mapper = new Mapper(config);
                return Ok(mapper.Map<CompletedApplication, CompletedApplicationDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetCompletedApplication: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("totalCompletedApplicationCount")]
        public ActionResult<int> GetTotalCompletedAppCount()
        {
            try
            {
                int result = _applicationService.GetTotalCompletedAppCount();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetTotalCompletedAppCount: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("totalAnnualReportCount")]
        public ActionResult<int> GetTotalAnnualReportCount()
        {
            try
            {
                int result = _applicationService.GetTotalAnnualReportCount();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetTotalAnnualReportCount: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("totalSemiAnnualReportCount")]
        public ActionResult<int> GetTotalSemiannualReportCount()
        {
            try
            {
                int result = _applicationService.GetTotalSemiannualReportCount();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetTotalSemiannualReportCount: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("countCompletedApplicationsInDateSpan")]
        public ActionResult<Dictionary<string, int>> CountCompletedApplicationsInDateSpan([FromBody] DateTime[] times)
        {
            try
            {
                Dictionary<string, int> result = _applicationService.CountCompletedApplicationsInDateSpan(times);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CountCompletedApplicationsInDateSpan: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("completedApplicationsFromDate")]
        public ActionResult<IEnumerable<CompletedApplicationSummary>> GetCompletedApplicationsFromDate([FromBody] DateTime date)
        {
            try
            {
                IEnumerable<CompletedApplicationSummary> result = _applicationService.GetCompletedApplicationsFromDate(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetCompletedApplicationsFromDate: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("countAnnualReportsInDateSpan")]
        public ActionResult<Dictionary<string, int>> CountAnnualReportsInDateSpan([FromBody] DateTime[] times)
        {
            try
            {
                Dictionary<string, int> result = _applicationService.CountAnnualReportsInDateSpan(times);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CountAnnualReportsInDateSpan: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
      
        [HttpPost("countSemiAnnualReportsInDateSpan")]
        public ActionResult<Dictionary<string, int>> CountSemiAnnualReportsInDateSpan([FromBody] DateTime[] times)
        {
            try
            {
                Dictionary<string, int> result = _applicationService.CountSemiAnnualReportsInDateSpan(times);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CountAnnualReportsInDateSpan: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("completedApplicationByWorkflowId")]
        public IActionResult FetchCompletedApplicationByWorkflowId([FromBody] Guid id)
        {
            try
            {
                CompletedApplication result = _applicationService.FetchCompletedApplicationByWorkflowId(id);
                MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<CompletedApplication, CompletedApplicationDto>());
                Mapper mapper = new Mapper(config);
                return Ok(mapper.Map<CompletedApplication, CompletedApplicationDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FetchCompletedApplicationByWorkflowId: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("failedApps")]
        public ActionResult<List<ApplicationInfo>> GetFailedApps()
        {
            try
            {
                List<ApplicationInfo> appList = _applicationService.GetFailedApps();

                return Ok(appList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetFailedApps: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("newFailedApps")]
        public ActionResult<List<ApplicationInfo>> GetNewFailedApps()
        {
            try
            {
                List<ApplicationInfo> result = _applicationService.GetNewFailedApps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetNewFailedApps: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("completedApplicationByUser")]
        public ActionResult<CompletedApplicationDto> FetchCompletedApplicationByUser([FromBody] Guid userId)
        {
            try
            {
                CompletedApplication result = _applicationService.FetchCompletedApplicationByUser(userId);
                MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<CompletedApplication, CompletedApplicationDto>());
                Mapper mapper = new Mapper(config);
                return Ok(mapper.Map<CompletedApplication, CompletedApplicationDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FetchCompletedApplicationByUser: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deleteApp")]
        public IActionResult DeleteApplication([FromBody] Guid workflowId)
        {
            try
            {
                _applicationService.DeleteApplication(workflowId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DeleteApplication: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addApp")]
        public IActionResult AddCompletedApplication([FromBody] CompletedApplication completedApplication)
        {
            try
            {
                _applicationService.AddCompletedApplication(completedApplication);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in AddCompletedApplication: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateApp")]
        public IActionResult UpdateCompletedApplication(CompletedApplication completedApplication)
        {
            try
            {
                _applicationService.UpdateCompletedApplication(completedApplication);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateCompletedApplication: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateFailedApp")]
        public IActionResult UpdateNewFailedApps(ApplicationInfo app)
        {
            try
            {
                _applicationService.UpdateNewFailedApps(app);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateNewFailedApps: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SubmitApplication([FromBody] string workflowXml)
        {
            try
            {
                _applicationService.SubmitApplication(workflowXml);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SubmitApplication: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private string CheckUserLexisNexisApplication(string applicationXml)
        {
            string result;

            var userId = GetUserId(applicationXml);
            // User did NOT use LN Application. Return the normal appXml
            if (userId == new Guid().ToString())
            {
                return applicationXml;
            }

            var entry = GetLnDbEntry(userId);

            if (entry != null && entry.UserId != new Guid())
            {
                result = SerializeLnDataToApplication(applicationXml, entry);
            }
            else
            {
                result = SerializeLnOptOutToApplication(applicationXml);
            }

            //LogMsg(result, $"xml_with_ln_data_no_matter_what.{DateTime.Now.Ticks}");

            // Application has been created in the last hour
            //if (entry.AppStartDate < DateTime.Now && entry.AppStartDate > DateTime.Now.AddHours(-1))
            //{
            //    // Add Ln Db Entry Data to respective fields in AppXML
            //    result = SerializeLnDataToApplication(applicationXml, entry);
            //}

            return result;
        }

        private string SerializeLnDataToApplication(string appXml, LnDbEntryDto entry)
        {

            XDocument doc = XDocument.Parse(appXml);

            var lnNode = doc.Descendants("lexisNexis").ToList();

            if (lnNode != null)
            {
                foreach (var element in lnNode)
                {
                    element.Element("accountEmail").ReplaceNodes(new XCData(entry.AccountEmail));
                    element.Element("accountFirstName").ReplaceNodes(new XCData(entry.AccountFirstName));
                    element.Element("accountLastName").ReplaceNodes(new XCData(entry.AccountLastName));
                    element.Element("accountTelephone").ReplaceNodes(new XCData(entry.AccountTelephone));
                    element.Element("accountAddressStreet1").ReplaceNodes(new XCData(entry.AccountAddressStreet1));
                    element.Element("accountAddressStreet2").ReplaceNodes(new XCData(entry.AccountAddressStreet2));
                    element.Element("accountAddressCity").ReplaceNodes(new XCData(entry.AccountAddressCity));
                    element.Element("accountAddressZip").ReplaceNodes(new XCData(entry.AccountAddressZip));
                    element.Element("accountAddressState").ReplaceNodes(new XCData(entry.AccountAddressState));
                    element.Element("accountAddressCountry").ReplaceNodes(new XCData(entry.AccountAddressCountry));
                    element.Element("accountDateOfBirth").ReplaceNodes(new XCData(entry.AccountDateOfBirth.ToString()));
                    element.Element("ssnRaw").ReplaceNodes(new XCData(entry.SsnRaw));
                    element.Element("cvi").ReplaceNodes(new XCData(entry.CVI.ToString()));
                    element.Element("potentiallyFraudulent").ReplaceNodes(new XCData(entry.PotentiallyFraudulent.ToString()));
                    element.Element("riskData").ReplaceNodes(new XCData(entry.RiskData));
                    element.Element("appStartDate").ReplaceNodes(new XCData(entry.AppStartDate.ToString()));
                    element.Element("submitted").ReplaceNodes(new XCData(entry.Submitted.ToString()));
                    element.Element("accountOptOut").ReplaceNodes(new XCData("false"));
                }
            }

            //LogMsg(doc.ToString(), "xml_after_ln_added");

            return doc.ToString();
        }

        private string SerializeLnOptOutToApplication(string appXml)
        {
            XDocument doc = XDocument.Parse(appXml);

            var lnNode = doc.Descendants("lexisNexis").ToList();

            if (lnNode != null)
            {
                foreach (var element in lnNode)
                {
                    //element.Element("accountEmail").ReplaceNodes(new XCData(entry.AccountEmail));
                    //element.Element("accountFirstName").ReplaceNodes(new XCData(entry.AccountFirstName));
                    //element.Element("accountLastName").ReplaceNodes(new XCData(entry.AccountLastName));
                    //element.Element("accountTelephone").ReplaceNodes(new XCData(entry.AccountTelephone));
                    //element.Element("accountAddressStreet1").ReplaceNodes(new XCData(entry.AccountAddressStreet1));
                    //element.Element("accountAddressStreet2").ReplaceNodes(new XCData(entry.AccountAddressStreet2));
                    //element.Element("accountAddressCity").ReplaceNodes(new XCData(entry.AccountAddressCity));
                    //element.Element("accountAddressZip").ReplaceNodes(new XCData(entry.AccountAddressZip));
                    //element.Element("accountAddressState").ReplaceNodes(new XCData(entry.AccountAddressState));
                    //element.Element("accountAddressCountry").ReplaceNodes(new XCData(entry.AccountAddressCountry));
                    //element.Element("accountDateOfBirth").ReplaceNodes(new XCData(entry.AccountDateOfBirth.ToString()));
                    //element.Element("ssnRaw").ReplaceNodes(new XCData(entry.SsnRaw));
                    //element.Element("cvi").ReplaceNodes(new XCData(entry.CVI.ToString()));
                    element.Element("potentiallyFraudulent").ReplaceNodes(new XCData("true"));
                    //element.Element("riskData").ReplaceNodes(new XCData(entry.RiskData));
                    //element.Element("appStartDate").ReplaceNodes(new XCData(entry.AppStartDate.ToString()));
                    //element.Element("submitted").ReplaceNodes("false");
                    element.Element("accountOptOut").ReplaceNodes(new XCData("true"));
                }
            }

            return doc.ToString();
        }

        private LnDbEntryDto GetLnDbEntry(string Id)
        {
            var entry = new LnDbEntryDto();

            //var CONN_STRING = ConfigurationManager.AppSettings["MyDhrDatabase"];
            //int pos = _CONN_STRING.IndexOf("connection string=");
            //var CONN_STRING = _CONN_STRING.Substring(pos + 19);
            //CONN_STRING = CONN_STRING.Remove(CONN_STRING.Length - 1);
            //LogMsg(CONN_STRING, $"connection_string_{DateTime.Now.Ticks}");

            //using (var conn = new SqlConnection(CONN_STRING))
            //{
            //    var cmd = new SqlCommand($"SELECT * FROM [dbo].[LexisNexis] WHERE [UserId] = '{Id}'", conn) { CommandType = CommandType.Text };
            //    conn.Open();
            //    var rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo);

            //    while (rdr.Read())
            //    {
            //        entry.UserId = rdr.IsDBNull(0) ? new Guid() : rdr.GetGuid(0);
            //        entry.AppId = rdr.IsDBNull(1) ? "" : rdr.GetString(1).Trim();
            //        entry.RequestData = rdr.IsDBNull(2) ? "" : rdr.GetString(2).Trim();
            //        entry.ResponseData = rdr.IsDBNull(3) ? "" : rdr.GetString(3).Trim();
            //        entry.CVI = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4);
            //        entry.RiskData = rdr.IsDBNull(5) ? "" : rdr.GetString(5).Trim();
            //        entry.PotentiallyFraudulent = rdr.IsDBNull(6) ? true : rdr.GetBoolean(6);
            //        entry.AppStartDate = rdr.IsDBNull(7) ? DateTime.MinValue : rdr.GetDateTime(7);

            //        entry.AccountEmail = rdr.IsDBNull(8) ? "" : rdr.GetString(8).Trim();
            //        entry.AccountTelephone = rdr.IsDBNull(9) ? "" : rdr.GetString(9).Trim();
            //        entry.AccountAddressStreet1 = rdr.IsDBNull(10) ? "" : rdr.GetString(10).Trim();
            //        entry.AccountAddressStreet2 = rdr.IsDBNull(11) ? "" : rdr.GetString(11).Trim();
            //        entry.AccountAddressCity = rdr.IsDBNull(12) ? "" : rdr.GetString(12).Trim();
            //        entry.AccountAddressZip = rdr.IsDBNull(13) ? "" : rdr.GetString(13).Trim();
            //        entry.AccountAddressState = rdr.IsDBNull(14) ? "" : rdr.GetString(14).Trim();
            //        entry.AccountAddressCountry = rdr.IsDBNull(15) ? "" : rdr.GetString(15).Trim();
            //        entry.AccountDateOfBirth = rdr.IsDBNull(16) ? DateTime.MinValue : rdr.GetDateTime(16);
            //        entry.AccountFirstName = rdr.IsDBNull(17) ? "" : rdr.GetString(17).Trim();
            //        entry.AccountLastName = rdr.IsDBNull(18) ? "" : rdr.GetString(18).Trim();
            //        entry.SsnRaw = rdr.IsDBNull(19) ? "" : rdr.GetString(19).Trim();
            //        entry.Submitted = rdr.IsDBNull(20) ? false : rdr.GetBoolean(20);
            //        entry.Approved = rdr.IsDBNull(21) ? false : rdr.GetBoolean(21);
            //    }

            //    conn.Close();
            //}
            //return entry;
            return null;
        }

        private string GetUserId(string applicationXml)
        {
            XDocument doc = XDocument.Parse(applicationXml);
            var instance = doc.Descendants("instance").Select(ins => new
            {
                id = ins.Element("id").Value,
                workflowTemplateID = ins.Element("WorkflowTemplateID").Value,
                UserID = ins.Element("UserID").Value,
                StartDate = ins.Element("StartDate").Value,
                LastModifiedDate = ins.Element("LastModifiedDate").Value,
                StatusMessage = ins.Element("StatusMessage").Value,
                CurrentDate = ins.Element("CurrentDate").Value
            }).First();

            if (!String.IsNullOrEmpty(instance.UserID))
            {
                return instance.UserID;
            }
            else
            {
                return new Guid().ToString();
            }
        }

        private void LogMsg(string msg, string fileName)
        {
            var sb = new StringBuilder();
            sb.Append(msg);
            //File.WriteAllText($"{_env.ContentRootPath}/Logs/" + $"{fileName}.txt", sb.ToString());
            System.IO.File.WriteAllText($"C:/TEMP/" + $"{fileName}.{DateTime.Now.Ticks.ToString()}.txt", sb.ToString());
            sb.Clear();
        }
    }
}
