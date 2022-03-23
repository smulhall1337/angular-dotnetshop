using System.Net;
using Core.Interfaces;

namespace Infrastructure.Services
{

    public class UserManagementService: IUserManagementService
    {
        // private static HttpClient client = new HttpClient();
        // string APIUrl = System.Configuration.ConfigurationManager.AppSettings["OacisServicesAPIUrl"] + "/";
        public SessionDto Authenticate(UserLoginDto loginDto)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.UserName == loginDto.UserName && u.PasswordHash == loginDto.PasswordHash);
            if (user == null)
                return null;


            // Force expiration of any previous sessions that have not already expired
            var oldSessions = user.MyDhrSession.Where(s => s.ExpireDate > DateTime.Now);
            foreach (var oldSession in oldSessions)
            {
                oldSession.ExpireDate = DateTime.Now;
            }

            var caseNumber = "";

            try
            {
                // Get old linked case number. If the API call fails later, this will be set to null in persistent storage.
                // Grabbing it here will ensure that the correct value persists and it does not become erroneously set to null.
                caseNumber =
                user.MyDhrSession.OrderByDescending(s => s.StartDate).First(c => c.CaseNumber != null).CaseNumber;
            }
            catch (Exception e)
            {
                // Users that just created an account or have not linked
                // a case will have no linked cases, and should
                // have a null caseNumber
                caseNumber = null;
            }
            MyDhrSession session = new MyDhrSession();
            session.CaseNumber = caseNumber;
            session.Id = Guid.NewGuid();
            session.User = user;
            session.UserId = user.Id;
            session.StartDate = DateTime.Now;
            session.ExpireDate = session.StartDate.AddMinutes(30);
            IPAddress ipAddress;
            IPAddress.TryParse(loginDto.IPAddress, out ipAddress);
            if (ipAddress != null)
            {
                var ipv6 = ipAddress.MapToIPv6();
                session.IpAddress = ipv6.GetAddressBytes();
            }

            //TO - DO: Add IP Address Field to table and add here and to AuthenticateByMyAlabama

            db.MyDhrSession.Add(session);
            db.SaveChanges();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrSession, SessionDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<SessionDto>(session);
        }

        public bool CheckPassword(UserLoginDto dto)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.UserName == dto.UserName && u.PasswordHash == dto.PasswordHash);
            if (user == null)
                return false;
            else
                return true;
        }

        public void ChangePassword(UserLoginDto dto)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.UserName == dto.UserName);
            if (user == null)
                return;
            else
            {
                user.PasswordHash = dto.PasswordHash;
                db.SaveChanges();
            }
        }

        public SessionDto UpdateCaseNumber(SessionDto newSession)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            MyDhrSession? oldSession = db.MyDhrSession.FirstOrDefault(s => s.Id == newSession.Id);

            oldSession.CaseNumber = newSession.CaseNumber;
            db.SaveChanges();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrSession, SessionDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<SessionDto>(oldSession);
        }

        public bool MakeUserAdmin(Guid userId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.Id == userId);
            var role = db.MyDhrRole.FirstOrDefault(r => r.Role == "Super User");

            if (role != null && user != null)
            {
                if (!user.Role.Contains(role))
                {
                    user.Role.Add(role);
                    db.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public bool MakeUserNotAdmin(Guid userId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.Id == userId);
            var role = db.MyDhrRole.FirstOrDefault(r => r.Role == "Super User");

            if (role != null && user != null)
            {
                if (user.Role.Contains(role))
                {
                    user.Role.Remove(role);
                    db.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public SessionDto ValidateSession(SessionDto sessionId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var session = db.MyDhrSession.FirstOrDefault(s => s.Id == sessionId.Id);

            if (session == null || DateTime.Now > session.ExpireDate)
            {
                //if (session != null) { db.MyDhrSession.Remove(session); db.SaveChanges(); } //delete stale sessions
                return null;
            }

            session.ExpireDate = DateTime.Now.AddMinutes(30);
            db.SaveChanges();
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrSession, SessionDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<SessionDto>(session);
        }

        public UserLoginDto CreateUser(UserLoginDto user)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            //Assumes a valid user object coming into it

            // HACK:
            //      This code was changed substatntially from the old app due to what .Map() returns. Gotta make sure this works. Also..
            // TODO:
            //      Give these variables better names. And TEST!

            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<UserLoginDto, MyDhrUser>());
            Mapper mapper = new Mapper(config);
            var newUser = db.MyDhrUser.Add(mapper.Map<MyDhrUser>(user));
            db.SaveChanges();


            MapperConfiguration? config2 = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserLoginDto>());
            Mapper mapper2 = new Mapper(config2);
            return mapper2.Map<UserLoginDto>(newUser);
        }

        public UserDto UpdateUser(UserDto user)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
                //Assumes a valid user object coming into it
                var dbUser = db.MyDhrUser.FirstOrDefault(u => u.Id == user.Id);
                dbUser.FirstName = user.FirstName;
                dbUser.MiddleName = user.MiddleName;
                dbUser.LastName = user.LastName;
                if (user.Dob == DateTime.MinValue)
                    dbUser.Dob = null;
                else
                    dbUser.Dob = user.Dob;
                dbUser.Sex = user.Sex;
                dbUser.Email = user.Email;
                dbUser.SecurityAnswer1 = user.SecurityAnswer1;
                dbUser.SecurityAnswer2 = user.SecurityAnswer2;
                dbUser.SecurityAnswer3 = user.SecurityAnswer3;
                dbUser.SecurityQuestion1 = user.SecurityQuestion1;
                dbUser.SecurityQuestion2 = user.SecurityQuestion2;
                dbUser.SecurityQuestion3 = user.SecurityQuestion3;

                db.SaveChanges();
                MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
                Mapper mapper = new Mapper(config);
                return mapper.Map<UserDto>(dbUser);
        }

        public UserDto UpdateMyAlUserToMyDhr(UserDto user)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            //Assumes a valid user object coming into it
            var dbUser = db.MyDhrUser.FirstOrDefault(u => u.Id == user.Id);
            dbUser.UserName = user.UserName;
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.FirstName = user.FirstName;
            dbUser.MiddleName = user.MiddleName;
            dbUser.LastName = user.LastName;
            dbUser.Dob = user.Dob;
            dbUser.Sex = user.Sex;
            dbUser.Email = user.Email;
            dbUser.SecurityAnswer1 = user.SecurityAnswer1;
            dbUser.SecurityAnswer2 = user.SecurityAnswer2;
            dbUser.SecurityAnswer3 = user.SecurityAnswer3;
            dbUser.SecurityQuestion1 = user.SecurityQuestion1;
            dbUser.SecurityQuestion2 = user.SecurityQuestion2;
            dbUser.SecurityQuestion3 = user.SecurityQuestion3;
            dbUser.MyAlabamaUserId = Guid.Empty;

            db.SaveChanges();
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<UserDto>(dbUser);
        }

        public UserDto FetchUser(Guid id)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbUser = db.MyDhrUser.FirstOrDefault(u => u.Id == id);
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<UserDto>(dbUser);
        }
        
        public String FetchUserSex(Guid id)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbUser = db.MyDhrUser.FirstOrDefault(u => u.Id == id);
            if (dbUser != null && dbUser.Sex != null)
                return dbUser.Sex;

            Guid? uID = dbUser.MyAlabamaUserId;
            if (uID != null && uID != Guid.Empty)
            {
                Guid userID = (Guid)uID;
                //return MyAlabamaService.GetMyAlabamaFieldData(userID, "Sex");
            }

            return "";
        }

        public PrefillInfoDto PrefillUserInfo(Guid id)
        {
            //using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            //PrefillInfoDto dto = new PrefillInfoDto();
            //var dbUser = db.MyDhrUser.FirstOrDefault(u => u.Id == id);
            //dto.Email = dbUser.Email;
            //dto.LastName = dbUser.LastName;
            //dto.MiddleName = dbUser.MiddleName;
            //dto.FirstName = dbUser.FirstName;
            //if (dbUser.Dob != null)
            //{
            //    dto.Dob = ((DateTime)dbUser.Dob).ToShortDateString();
            //}
            //if (dbUser.Ssn != null)
            //{
            //    dto.SSN = dbUser.Ssn;
            //    if (dto.SSN.Length == 9)
            //        dto.SSN = dto.SSN.Insert(5, "-").Insert(3, "-");
            //}

            //Guid? uID = dbUser.MyAlabamaUserId;

            //if (uID != null && uID != Guid.Empty)
            //{
            //    Guid userID = (Guid)uID;
            //    string MyAlHomeAddressLine1 = MyAlabamaService.GetMyAlabamaFieldData(userID, "StreetAddressStreet1");
            //    string MyAlHomeAddressLine2 = MyAlabamaService.GetMyAlabamaFieldData(userID, "StreetAddressStreet2");
            //    string MyAlHomeCity = MyAlabamaService.GetMyAlabamaFieldData(userID, "StreetAddressCity");
            //    string MyAlHomeState = MyAlabamaService.GetMyAlabamaFieldData(userID, "StreetAddressState");
            //    string MyAlHomeZip = MyAlabamaService.GetMyAlabamaFieldData(userID, "StreetAddressZIP");

            //    string MyAlMailAddressLine1 = MyAlabamaService.GetMyAlabamaFieldData(userID, "MailingAddressStreet1");
            //    string MyAlMailAddressLine2 = MyAlabamaService.GetMyAlabamaFieldData(userID, "MailingAddressStreet2");
            //    string MyAlMailCity = MyAlabamaService.GetMyAlabamaFieldData(userID, "MailingAddressCity");
            //    string MyAlMailState = MyAlabamaService.GetMyAlabamaFieldData(userID, "MailingAddressState");
            //    string MyAlMailZip = MyAlabamaService.GetMyAlabamaFieldData(userID, "MailingAddressZIP");

            //    string MyAlPhone = MyAlabamaService.GetMyAlabamaFieldData(userID, "PhoneNumber");

            //    string MyAlSSN = MyAlabamaService.GetMyAlabamaFieldData(userID, "SSN");

            //    dto.PhoneNumber = MyAlPhone;
            //    MailingAddress home = new MailingAddress();
            //    home.Address1 = MyAlHomeAddressLine1;
            //    home.Address2 = MyAlHomeAddressLine2;
            //    home.City = MyAlHomeCity;
            //    home.State = MyAlHomeState;
            //    home.ZipCode = MyAlHomeZip;
            //    dto.HomeAddress = home;

            //    MailingAddress mail = new MailingAddress();
            //    mail.Address1 = MyAlMailAddressLine1;
            //    mail.Address2 = MyAlMailAddressLine2;
            //    mail.City = MyAlMailCity;
            //    mail.State = MyAlMailState;
            //    mail.ZipCode = MyAlMailZip;
            //    dto.MailingAddress = mail;

            //    if (MyAlSSN != "")
            //    {
            //        dto.SSN = MyAlSSN;
            //        if (dto.SSN.Length == 9)
            //            dto.SSN = dto.SSN.Insert(5, "-").Insert(3, "-");
            //    }
            //}

            // return dto;
            return null;
        }

        public UserDto FetchUserByUsername(string username)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbUser = db.MyDhrUser.FirstOrDefault(u => u.UserName == username);
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<UserDto>(dbUser);
        }

        public bool UsernameAlreadyExists(string username)
        {
            //var sb = new StringBuilder();
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            try
            {
                if (!String.IsNullOrWhiteSpace(username))
                {
                    //sb.Append($"{db.MyDhrUser.ToString()} -----  ------ {username}");
                    //File.WriteAllText($"/TEMP/" + $"service.user_already_exists.{DateTime.Now.Ticks.ToString()}.txt", sb.ToString());
                    //sb.Clear();

                    var users = db.MyDhrUser.Where(u => u.UserName == username);

                    //sb.Append($"{users.ToString()}");
                    //File.WriteAllText("/TEMP/" + "service.user_firstordefault.txt", sb.ToString());
                    //sb.Clear();

                    var user = users.FirstOrDefault();

                    if (user == null)
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                //sb.Append($"{ex.Message}    ------------------------------------------    {ex.StackTrace}");
                //File.WriteAllText("/TEMP/" + "service.user_error.txt", sb.ToString());
                //sb.Clear();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        public UserDto LinkUser(UserDto link)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var user = db.MyDhrUser.FirstOrDefault(u => u.UserName == link.UserName && u.PasswordHash == link.PasswordHash);
            if (user == null)
                return null;

            if (user.MyAlabamaUserId != null && user.MyAlabamaUserId != Guid.Empty)
                return null;

            user.MyAlabamaUserId = link.MyAlabamaUserId;
            user.MyAlabamaLinkDate = DateTime.Today;
            db.SaveChanges();
            MapperConfiguration? config = new(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<UserDto>(user);
        }

        public UserDto FetchUserForgotPassword(string email)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            IEnumerable<MyDhrUser> users;
            users = db.MyDhrUser.Where(u => u.Email == email);
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, UserDto>());
            Mapper mapper = new Mapper(config);
            if (users.Count() != 1)
                return null;
            else
                return mapper.Map<UserDto>(users.First());
        }

        public bool ResetPassword(ResetPasswordDto dto)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            MyDhrUser user;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            user = db.MyDhrUser.FirstOrDefault(u => u.Id == dto.UserId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (user != null)
            {
                // updates user 
                user.PasswordHash = dto.Password;       // Password was hashed in FoodAssistanceUserManager
                user.PasswordLastReset = DateTime.Now;

                // deletes password token that was used
                var token = db.PasswordToken.FirstOrDefault(t => t.UserId == user.Id);
                db.PasswordToken.Remove(token);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool CreatePasswordResetToken(PasswordResetTokenDto dto)
        {
            // Assumes a valid object coming into it

            deleteExpiredResetTokens();

            // Checks for existing unexpired token with same userId
            //var existingToken = db.PasswordToken.FirstOrDefault(t => t.UserId == dto.UserId);
            //if (existingToken != null)
            //    return false;
            //else
            //{
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<PasswordResetTokenDto, PasswordToken>());
            Mapper mapper = new Mapper(config);
            db.PasswordToken.Add(mapper.Map<PasswordToken>(dto));
            db.SaveChanges();
            return true;
            //}
        }
        private void deleteExpiredResetTokens()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var tokens = db.PasswordToken.Where(t => t.ExpirationDate < DateTime.Now);
            foreach (var item in tokens)
            {
                db.PasswordToken.Remove(item);
            }
            db.SaveChanges();
        }

        public bool ValidateResetToken(Guid resetToken)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            deleteExpiredResetTokens();
            var token = db.PasswordToken.FirstOrDefault(t => t.Id == resetToken);
            if (token != null)
                return true;
            else
                return false;
        }

        //public string GetReviewStatus(ReviewStatusDto dto)
        //{
        //    string reviewStatus;
        //    bool submittedDuringReviewPeriod = false;
        //    using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
        //    string APIUrl = "";
        //    var latestEmail = db.Email.Where(e => e.Subject.Contains(dto.CaseNumber) && e.Subject.Contains("annual")).OrderByDescending(e => e.Date).FirstOrDefault();

        //    if (latestEmail != null && dto.BeginDate < latestEmail.Date && latestEmail.Date < dto.EndDate)
        //        submittedDuringReviewPeriod = true;

        //    if (dto.BeginDate < DateTime.Now && DateTime.Now < dto.EndDate)
        //    {
        //        HttpResponseMessage response = client.PostAsJsonAsync<ReviewStatusDto>(APIUrl + "Oacis/IsReviewComplete", dto).Result;
        //        response.EnsureSuccessStatusCode();

        //        bool isComplete = bool.Parse(response.Content.ReadAsStringAsync().Result);

        //        if (isComplete && submittedDuringReviewPeriod)
        //            reviewStatus = "COMPLETED";
        //        else if (submittedDuringReviewPeriod)
        //            reviewStatus = "SUBMITTED";
        //        else
        //            reviewStatus = "NOT COMPLETED";
        //    }
        //    else
        //    {
        //        reviewStatus = "NOT IN REVIEW PERIOD";
        //    }

        //    return reviewStatus;
        //}

        public List<MyDhrUser> SearchUsersFirstLastName(string firstName, string lastName)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<MyDhrUser> result = db.MyDhrUser.Where(u => u.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                                                 && u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)).ToList();
            return result;
        }

        public List<MyDhrUser> SearchUsersUserName(string searchTerm)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<MyDhrUser>  result = db.MyDhrUser.Where(u => u.UserName.Contains(searchTerm)
                                           || u.MyAlabamaUserName.Contains(searchTerm)).ToList();
            return result;
        }

        public List<MyDhrUser> SearchUsersEmail(string searchTerm)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<MyDhrUser> result = db.MyDhrUser.Where(u => u.Email == searchTerm).ToList();
            return result;
        }

        public List<MyDhrUser> SearchUsersSsn(string searchTerm)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<MyDhrUser> result = db.MyDhrUser.Where(u => u.Ssn.Replace("-", "") == searchTerm).ToList();
            return result;
        }

        public MyDhrUser GetDhrUserMatch(Guid matchUserId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            MyDhrUser result = db.MyDhrUser.FirstOrDefault(u => u.Id == matchUserId);
            return result;
        }

        //public List<UserSearchDto> SearchUsers(UserSearchParametersDto dto)
        //{
        //    using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
        //    MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, MyDhrUser>());
        //    Mapper mapper = new Mapper(config);

        //    List<UserSearchDto> resultsList = new List<UserSearchDto>();
        //    List<MyDhrUser> prelimResults = new List<MyDhrUser>();
        //    if (dto.Parameters.Count == 0)
        //        return null;

        //    List<MyDhrUser> tempList;

        //    if (dto.Category == "name")
        //    {
        //        if (dto.Parameters.Count != 2)
        //            return resultsList;

        //        string firstName = dto.Parameters[0];
        //        string lastName = dto.Parameters[1];
        //        //find users that match this particular search parameter
        //        tempList = db.MyDhrUser.Where(u => u.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
        //                                         && u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)).ToList();
        //        //for each user in the list, determine the match rating
        //        foreach (MyDhrUser? result in tempList)
        //        {
        //            resultsList.Add(new UserSearchDto
        //            {
        //                User = mapper.Map<UserDto>(result),
        //                CaseNumber = MakeAPICall<string>("GetLinkedCase", "?userId=" + result.Id)
        //            });
        //        }
        //    }
        //    else
        //    {
        //        string parameter = dto.Parameters[0];
        //        //if parameter is an integer, i.e. potentially a casenumber or ssn
        //        if (dto.Category == "casenumber")
        //        {
        //            MyDhrUser match;
        //            //UserSearchDto resultsMatch;
        //            //search for a case with that parameter as a case number
        //            var matchUserIdString = MakeAPICall<string>("GetUserIdByCaseNumber", "?caseNumber=" + parameter.Replace("-", ""));
        //            Guid matchUserId;
        //            Guid.TryParse(matchUserIdString, out matchUserId);
        //            if (matchUserId != null && matchUserId != Guid.Empty)
        //            {
        //                //if a matching case is found, find the mydhr user on that case
        //                match = db.MyDhrUser.FirstOrDefault(u => u.Id == matchUserId);
        //                if (match != null)
        //                {
        //                    resultsList.Add(new UserSearchDto
        //                    {
        //                        User = mapper.Map<UserDto>(match),
        //                        CaseNumber = parameter.Replace("-", "")
        //                    });

        //                }
        //            }
        //        }
        //        //if the parameter is not a number
        //        else if (dto.Category == "username")
        //        {
        //            var searchTerm = parameter.ToLower();
        //            //find users that match this particular search parameter
        //            tempList = db.MyDhrUser.Where(u => u.UserName.Contains(searchTerm)
        //                                                || u.MyAlabamaUserName.Contains(searchTerm)).ToList();
        //            //for each user in the list, determine the match rating
        //            foreach (var result in tempList)
        //            {
        //                resultsList.Add(new UserSearchDto
        //                {
        //                    User = mapper.Map<UserDto>(result),
        //                    CaseNumber = MakeAPICall<string>("GetLinkedCase", "?userId=" + result.Id)
        //                });
        //            }
        //        }
        //        else if (dto.Category == "email")
        //        {
        //            var searchTerm = parameter.ToLower();
        //            //find users that match this particular search parameter
        //            tempList = db.MyDhrUser.Where(u => u.Email == searchTerm).ToList();
        //            //for each user in the list, determine the match rating
        //            foreach (var result in tempList)
        //            {
        //                resultsList.Add(new UserSearchDto
        //                {
        //                    User = mapper.Map<UserDto>(result),
        //                    CaseNumber = MakeAPICall<string>("GetLinkedCase", "?userId=" + result.Id)
        //                });
        //            }
        //        }
        //        else if (dto.Category == "SSN")
        //        {
        //            var searchTerm = parameter.ToLower();
        //            //find users that match this particular search parameter
        //            tempList = db.MyDhrUser.Where(u => u.Ssn.Replace("-", "") == searchTerm).ToList();
        //            //for each user in the list, determine the match rating
        //            foreach (var result in tempList)
        //            {
        //                resultsList.Add(new UserSearchDto
        //                {
        //                    User = mapper.Map<UserDto>(result),
        //                    CaseNumber = MakeAPICall<string>("GetLinkedCase", "?userId=" + result.Id)
        //                });
        //            }
        //        }
        //    }
        //    return resultsList;
        //}

        public List<CompletedApplicationSummaryDto> FetchCompletedApplicationsByUserId(Guid userId)
        {
            if (userId == null)
                return null;

            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var apps = db.CompletedApplication.Where(ca => ca.UserId == userId).
                Select(b => new CompletedApplicationSummaryDto()
                {
                    User = b.MyDhrUser.UserName,
                    CompletedDate = b.CompletedDate,
                    Submitted = b.Submitted,
                    SubmittedDate = b.SubmittedDate,
                    UserId = b.UserId,
                    CaseNumber = b.CaseNumber,
                    WorkflowId = b.WorkflowId
                })
                .OrderByDescending(ca => ca.CompletedDate);

            if (apps == null)
                return null;

            List<CompletedApplicationSummaryDto> appDtos = new List<CompletedApplicationSummaryDto>();

            foreach (var app in apps)
            {
                var session = db.MyDhrSession.FirstOrDefault(a => a.StartDate < app.CompletedDate
                                                                  && a.ExpireDate > app.CompletedDate
                                                                  && a.UserId == app.UserId);
                if (session != null && session.IpAddress != null)
                {
                    IPAddress ip = new IPAddress(session.IpAddress);
                    app.IPAddress = ip.ToString();
                    if (app.IPAddress.StartsWith("::ffff:"))
                        app.IPAddress = app.IPAddress.Replace("::ffff:", "");
                }
                appDtos.Add(app);
            }
            return appDtos;
        }

        public bool RemoveCompletedApplicationByWorkflowId(string id)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            if (id == null)
                return false;

            Guid workflowId = new Guid(id);
            if (workflowId == null || workflowId == Guid.Empty)
                return false;

            CompletedApplication row = db.CompletedApplication.FirstOrDefault(ca => ca.WorkflowId == workflowId);

            if (row == null)
                return false;

            db.CompletedApplication.Remove(row);
            db.SaveChanges();

            return true;
        }

        public RegisterApplicationDto FetchCompletedApplicationByCaseNumber(string caseNumber)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            if (caseNumber == "NOT LINKED")
                return null;

            var app = db.CompletedApplication.Where(ca => ca.CaseNumber == caseNumber)
                    .OrderByDescending(ca => ca.CompletedDate).FirstOrDefault();

            if (app == null)
                return null;

            else
            {
                RegisterApplicationDto dto = new RegisterApplicationDto();
                dto.CaseNumber = app.CaseNumber;
                dto.DateCreated = app.CompletedDate;
                dto.UserId = app.UserId;
                return dto;
            }
        }

        public List<IPApplicationInfoDto> FetchIPsWithMultipleApplications()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            FoodAssistanceDatabaseContextProcedures procs = new FoodAssistanceDatabaseContextProcedures(db);
            List<GetIPsWithMultipleApplicationsResult> spResults = procs.GetIPsWithMultipleApplicationsAsync().Result;

            List<IPApplicationInfoDto> ipList = new List<IPApplicationInfoDto>();
            foreach (var result in spResults)
            {
                IPAddress ip = new IPAddress(result.IPAddress);
                var ipString = ip.ToString();
                if (ipString.StartsWith("::ffff:"))
                    ipString = ipString.Replace("::ffff:", "");

                IPApplicationInfoDto dto = new IPApplicationInfoDto
                {
                    IPAddress = ipString,
                    ApplicationCount = result.TotalCount ?? 0
                };
                ipList.Add(dto);
            }
            return ipList;
        }

        public List<CompletedApplicationSummaryDto> FetchSubmittedAppsByIPAddress(byte[] ipAddress)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            FoodAssistanceDatabaseContextProcedures procs = new FoodAssistanceDatabaseContextProcedures(db);

            List<GetSubmittedAppsByIPAddressResult> appWorkflows = procs.GetSubmittedAppsByIPAddressAsync(ipAddress).Result;

            List<CompletedApplicationSummaryDto> submittedApps = new List<CompletedApplicationSummaryDto>();
            foreach (var workflow in appWorkflows)
            {
                if (workflow != null)
                    submittedApps.Add(new CompletedApplicationSummaryDto
                    {
                        UserId = workflow.UserId,
                        CompletedDate = workflow.CompletedDate,
                        Submitted = workflow.Submitted,
                        SubmittedDate = workflow.SubmittedDate ?? DateTime.MinValue,
                        WorkflowId = workflow.WorkflowId,
                        CaseNumber = workflow.CaseNumber
                    });
            }
            return submittedApps;
        }

        public string GetReviewStatus(ReviewStatusDto dto)
        {
            throw new NotImplementedException();
        }

        public List<UserSearchDto> SearchUsers(UserSearchParametersDto dto)
        {
            throw new NotImplementedException();
        }
    }// /class
}// /namespace
