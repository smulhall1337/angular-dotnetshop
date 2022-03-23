using System.Text.Json;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UserManagementController : ControllerBase
    // Note: The original app was made with its controllers extending 'Controller' rather than 'ControllerBase'
    //          This was due to it returning web page views with razor. But since our front end will be done in angular, we're not concerned with that.
    //          So instead of returning views, we can just return IActionResults and any relevant Json. See: https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0
    {

        private readonly ILogger<UserManagementController> _logger;
        private readonly UserManagementService _userManagementService;
        private readonly OacisService _oacisService;
        public UserManagementController(ILogger<UserManagementController> logger, UserManagementService userManagementService, OacisService oacisService)
        {
            _logger = logger;
            _userManagementService = userManagementService;
            _oacisService = oacisService;
        }

        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate(UserLoginDto loginDto)
        {
            try
            {
                // it shouldn't be....
#pragma warning disable CS8601 // Possible null reference assignment.
                loginDto.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.

                loginDto.PasswordHash = CryptoUtils.GetHash(loginDto.PasswordHash, HashProvider.SHA512);
                SessionDto returnDto = _userManagementService.Authenticate(loginDto);
                string jsonString = JsonSerializer.Serialize(returnDto);
                return Ok(jsonString);
            }
            catch (Exception ex)
            {
                // TODO: Log errors 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CheckPassword(UserLoginDto dto)
        {
            try
            {
                bool result = _userManagementService.CheckPassword(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(UserLoginDto dto)
        {
            try
            {
                _userManagementService.ChangePassword(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ChangePassword: " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult UpdateCaseNumber(SessionDto newSession)
        {
            try
            {
                SessionDto result = _userManagementService.UpdateCaseNumber(newSession);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateCaseNumber: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult MakeUserAdmin([FromBody] Guid userId)
        {
            try
            {
                bool result = _userManagementService.MakeUserAdmin(userId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult MakeUserNotAdmin([FromBody] Guid userId)
        {
            try
            {
                bool result = _userManagementService.MakeUserNotAdmin(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ValidateSession(SessionDto sessionId)
        {
            try
            {
                SessionDto result = _userManagementService.ValidateSession(sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UserLoginDto user)
        {
            try
            {
                UserLoginDto result = _userManagementService.CreateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(UserDto user)
        {
            try
            {
                UserDto result = _userManagementService.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateMyAlUserToMyDhr(UserDto user)
        {
            try
            {
                UserDto result = _userManagementService.UpdateMyAlUserToMyDhr(user);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchUser([FromBody] Guid id)
        {
            try
            {
                UserDto result = _userManagementService.FetchUser(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchUserSex([FromBody] Guid id)
        {
            try
            {
                String result = _userManagementService.FetchUserSex(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchUserByUsername([FromBody] String userName)
        {
            try
            {
                UserDto result = _userManagementService.FetchUserByUsername(userName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UsernameAlreadyExists([FromBody] string username)
        {
            try
            {
                bool result = _userManagementService.UsernameAlreadyExists(username);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost] 
        public IActionResult LinkUser(UserDto link)
        {
            try
            {
                UserDto result = _userManagementService.LinkUser(link);
                return Ok(result);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchUserForgotPassword([FromBody] string email)
        {
            try
            {
                UserDto result = _userManagementService.FetchUserForgotPassword(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                bool result = _userManagementService.ResetPassword(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreatePasswordResetToken(ResetPasswordDto dto)
        {
            try
            {
                bool result = _userManagementService.ResetPassword(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ValidateResetToken([FromBody] Guid resetToken)
        {
            try
            {
                bool result = _userManagementService.ValidateResetToken(resetToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetReviewStatus(ReviewStatusDto dto)
        {
            try
            {
                string result = _userManagementService.GetReviewStatus(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SearchUsers(UserSearchParametersDto dto)
        {
            // This was originally one big method that started in User management but used calls from Oacis.
            // I tried to keep most of the code in here, and only using usermanagement and oacis when needed. This will probably need to be reogranized in the future

            // TODO: Think of a better way to do thisw
            //try
            //{
            //    List<UserSearchDto> result = _userManagementService.SearchUsers(dto);
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            List<UserSearchDto> resultsList = new List<UserSearchDto>();
            List<MyDhrUser> prelimResults = new List<MyDhrUser>();
            List<MyDhrUser> tempList;

            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, MyDhrUser>());
            Mapper mapper = new Mapper(config);

            if (dto.Parameters.Count == 0)
                return Ok(null);

            if (dto.Category == "name")
            {
                if (dto.Parameters.Count != 2)
                    return Ok(resultsList);

                string firstName = dto.Parameters[0];
                string lastName = dto.Parameters[1];
                //find users that match this particular search parameter
                tempList = _userManagementService.SearchUsersFirstLastName(firstName, lastName);

                //for each user in the list, determine the match rating
                foreach (MyDhrUser? result in tempList)
                {
                    resultsList.Add(new UserSearchDto
                    {
                        User = mapper.Map<UserDto>(result),
                        CaseNumber = _oacisService.GetLinkedCase(result.Id)
                    });
                }
            }
            else
            {
                string parameter = dto.Parameters[0];
                //if parameter is an integer, i.e. potentially a casenumber or ssn
                if (dto.Category == "casenumber")
                {
                    MyDhrUser match;
                    //UserSearchDto resultsMatch;
                    //search for a case with that parameter as a case number
                    Guid matchUserIdString = _oacisService.GetUserIdByCaseNumber(parameter.Replace("-", ""));
                    // MakeAPICall<string>("GetUserIdByCaseNumber", "?caseNumber=" + parameter.Replace("-", ""));
                    Guid matchUserId;
                    // I'm not sure whats going on here but i THINK its getting the userId from oacis... converting it to a string, then parsing as a GUID????
                    Guid.TryParse(matchUserIdString.ToString().ToUpper(), out matchUserId);
                    if (matchUserId != null && matchUserId != Guid.Empty)
                    {
                        //if a matching case is found, find the mydhr user on that case
                        match = _userManagementService.GetDhrUserMatch(matchUserId);
                        // db.MyDhrUser.FirstOrDefault(u => u.Id == matchUserId);
                        if (match != null)
                        {
                            resultsList.Add(new UserSearchDto
                            {
                                User = mapper.Map<UserDto>(match),
                                CaseNumber = parameter.Replace("-", "")
                            });

                        }
                    }
                }
                //if the parameter is not a number
                else if (dto.Category == "username")
                {
                    var searchTerm = parameter.ToLower();
                    //find users that match this particular search parameter
                    tempList = _userManagementService.SearchUsersUserName(searchTerm);


                    //for each user in the list, determine the match rating
                    foreach (var result in tempList)
                    {
                        resultsList.Add(new UserSearchDto
                        {
                            User = mapper.Map<UserDto>(result),
                            CaseNumber = _oacisService.GetLinkedCase(result.Id)
                        });
                    }
                }
                else if (dto.Category == "email")
                {
                    var searchTerm = parameter.ToLower();
                    //find users that match this particular search parameter
                    tempList = _userManagementService.SearchUsersEmail(searchTerm);
                    //for each user in the list, determine the match rating
                    foreach (var result in tempList)
                    {
                        resultsList.Add(new UserSearchDto
                        {
                            User = mapper.Map<UserDto>(result),
                            CaseNumber = _oacisService.GetLinkedCase(result.Id)
                        });
                    }
                }
                else if (dto.Category == "SSN")
                {
                    var searchTerm = parameter.ToLower();
                    //find users that match this particular search parameter
                    tempList = _userManagementService.SearchUsersSsn(searchTerm);
                    //for each user in the list, determine the match rating
                    foreach (var result in tempList)
                    {
                        resultsList.Add(new UserSearchDto
                        {
                            User = mapper.Map<UserDto>(result),
                            CaseNumber = _oacisService.GetLinkedCase(result.Id)
                        });
                    }
                }
            }
            return Ok(resultsList);
        }

        [HttpPost]
        public IActionResult FetchCompletedApplicationsByUserId([FromBody] Guid userId)
        {
            try
            {
                List<CompletedApplicationSummaryDto> result = _userManagementService.FetchCompletedApplicationsByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult RemoveCompletedApplicationByWorkflowId([FromBody] string id)
        {
            try
            {
                bool result = _userManagementService.RemoveCompletedApplicationByWorkflowId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchCompletedApplicationByCaseNumber([FromBody] string caseNumber)
        {
            try
            {
                RegisterApplicationDto result = _userManagementService.FetchCompletedApplicationByCaseNumber(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchIPsWithMultipleApplications()
        {
            try
            {
                List<IPApplicationInfoDto> result = _userManagementService.FetchIPsWithMultipleApplications();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult FetchSubmittedAppsByIPAddress([FromBody] byte[] ipAddress)
        {
            try
            {
                List<CompletedApplicationSummaryDto> result = _userManagementService.FetchSubmittedAppsByIPAddress(ipAddress);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult TestAuthenticatedResponse()
        {
            return Ok("SUCCESSFUL");
        }

        [HttpPost]
        [Authorize(Roles = "Super User")]
        public IActionResult TestAuthorizedResponse()
        {
            return Ok("SUCCESSFUL");
        }

        [HttpPost]
        public IActionResult GetCompletedApplicationByCaseNumber([FromBody]string caseNumber)
        {
            try
            {

                RegisterApplicationDto result = _userManagementService.FetchCompletedApplicationByCaseNumber(caseNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// GET: UserManagementController/IsAlive
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
                // TODO: Log errors 
                return BadRequest(ex.Message);
            }
        }
    }
}
