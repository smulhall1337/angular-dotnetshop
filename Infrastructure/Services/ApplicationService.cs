using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AutoMapper;
using Core.Entities.Dhr;
using Core.Entities.Oacis;
using Infrastructure.Data.Dhr;
using Infrastructure.Data.Oacis;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace Infrastructure.Services
{
    public class ApplicationService
    {
        /**
         * TODO: Implement unit of work
         */
        public IEnumerable<CompletedApplication> GetCompletedApplications()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbApps = db.CompletedApplication.Where(a => a.Submitted == false && a.Attempts < 6).ToArray();
            return dbApps;
        }

        public IEnumerable<CompletedApplicationSummary> GetCompletedApplicationSummary(
            GetCompletedApplicationSummary dto)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbApps = db.CompletedApplication
                .Where(a => a.Submitted == dto.Submitted && a.Attempts < dto.Attempts)
                .Select(b => new CompletedApplicationSummary()
                {
                    Attempts = b.Attempts,
                    User = b.MyDhrUser.UserName,
                    CompletedDate = b.CompletedDate,
                    Submitted = b.Submitted,
                    SubmittedDate = b.SubmittedDate,
                    UserId = b.UserId,
                    WorkflowId = b.WorkflowId
                });
            return dbApps;
        }

        public CompletedApplication GetCompletedApplication(Guid workflowId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var app = db.CompletedApplication.FirstOrDefault(a => a.WorkflowId == workflowId);
            return app;
        }

        public int GetTotalCompletedAppCount()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var appCount = db.CompletedApplication.Where(app => app.SubmittedDate != null).Count();
            return appCount;
        }

        public int GetTotalAnnualReportCount()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var reportCount = db.Email.Where(a => a.Subject.StartsWith("Annual Report")).Count();
            return reportCount;
        }

        public int GetTotalSemiannualReportCount()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var reportCount = db.Email.Where(a => a.Subject.StartsWith("Semi-Annual Report")).Count();
            return reportCount;
        }


        public Dictionary<string, int> CountCompletedApplicationsInDateSpan(DateTime[] times)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime first = times[0];
            DateTime last = times[1].AddDays(1);
            Dictionary<string, int> counts = new Dictionary<string, int>();
            var apps = db.CompletedApplication.Where(a => a.SubmittedDate < last && a.SubmittedDate > first);
            foreach (CompletedApplication app in apps)
            {
                DateTime day = app.SubmittedDate.Value;
                if (!counts.ContainsKey(day.ToShortDateString()))
                    counts.Add(day.ToShortDateString(), 0);
                counts[day.ToShortDateString()]++;
            }

            return counts;
        }

        public IEnumerable<CompletedApplicationSummary> GetCompletedApplicationsFromDate(DateTime date)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime next = date.AddDays(1);
            var apps = db.CompletedApplication.Where(a => a.SubmittedDate > date && a.SubmittedDate < next).Select(b =>
                new CompletedApplicationSummary()
                {
                    User = b.MyDhrUser.MyAlabamaUserId == Guid.Empty
                        ? b.MyDhrUser.UserName
                        : "MyAlabama User: " + b.MyDhrUser.MyAlabamaUserName,
                    CompletedDate = b.CompletedDate,
                    Submitted = b.Submitted,
                    SubmittedDate = b.SubmittedDate,
                    UserId = b.UserId,
                    WorkflowId = b.WorkflowId
                });
            var results = new List<CompletedApplicationSummary>();
            foreach (var app in apps)
            {
                var session = db.MyDhrSession.FirstOrDefault(a => a.StartDate < app.CompletedDate
                                                                  && a.ExpireDate > app.CompletedDate
                                                                  && a.UserId == app.UserId);

                // Get LexisNexis DB Entry...
                var entry = GetLnDbEntry(app.UserId.ToString());
                //LogMsg(entry.UserId.ToString(), $"getting_completed_apps_userid_{DateTime.Now.Ticks}");
                // Check if entry is null or does not exist
                if (entry != null && entry.UserId != new Guid())
                {
                    app.PotentiallyFraudulent = entry.PotentiallyFraudulent;
                    app.OptedOut = false;
                }
                else
                {
                    // If User Opt's Out, they are automatically marked as potentially fraudulent
                    app.OptedOut = true;
                    app.PotentiallyFraudulent = true;
                }

                if (session != null && session.IpAddress != null)
                {
                    IPAddress ip = new IPAddress(session.IpAddress);
                    app.IPAddress = ip.ToString();
                    if (app.IPAddress.StartsWith("::ffff:"))
                        app.IPAddress = app.IPAddress.Replace("::ffff:", "");
                }

                results.Add(app);
            }

            return results;
        }

        public Dictionary<string, int> CountAnnualReportsInDateSpan(DateTime[] times)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime first = times[0];
            DateTime last = times[1].AddDays(1);
            Dictionary<string, int> counts = new Dictionary<string, int>();
            var reports = db.Email.Where(a => a.Subject.StartsWith("Annual Report") && a.Date < last && a.Date > first);
            foreach (Email report in reports)
            {
                DateTime day = report.Date.Value;
                if (!counts.ContainsKey(day.ToShortDateString()))
                    counts.Add(day.ToShortDateString(), 0);
                counts[day.ToShortDateString()]++;
            }

            return counts;
        }

        public Dictionary<string, int> CountSemiAnnualReportsInDateSpan(DateTime[] times)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime first = times[0];
            DateTime last = times[1].AddDays(1);
            Dictionary<string, int> counts = new Dictionary<string, int>();
            var reports = db.Email.Where(a =>
                a.Subject.StartsWith("Semi-Annual Report") && a.Date < last && a.Date > first);
            foreach (Email report in reports)
            {
                DateTime day = report.Date.Value;
                if (!counts.ContainsKey(day.ToShortDateString()))
                    counts.Add(day.ToShortDateString(), 0);
                counts[day.ToShortDateString()]++;
            }

            return counts;
        }


        public CompletedApplication FetchCompletedApplicationByWorkflowId(Guid id)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbApps = db.CompletedApplication.Where(a => a.WorkflowId == id);
            if (dbApps.Any())
            {
                var dbApp = dbApps.First();

            }

            return null;
        }

        public List<ApplicationInfo> GetFailedApps()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<CompletedApplication> appList = db.CompletedApplication
                .Where(ca => ca.Attempts == 6 && ca.Submitted == false).OrderByDescending(a => a.CompletedDate)
                .ToList();

            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, User>());
            Mapper mapper = new Mapper(config);

            /**
             * TODO: Think of a better way to do this.
             * We're essentially returning a list of DTOs with a DTO in it. There's probably a way to simplify this 
             * and flatten it so its easier to deal with on the client side.
             *
             * but maybe there isn't... i didnt write this. and if it works, it works
             */

            var failedAppList = new List<ApplicationInfo>();
            foreach (var app in appList)
            {
                ApplicationInfo dto = new ApplicationInfo();
                dto.WorkflowId = app.WorkflowId;
                dto.User = mapper.Map<User>(app.MyDhrUser);
                dto.UserId = app.UserId;
                dto.Submitted = app.Submitted;
                dto.SubmittedDate = app.SubmittedDate;
                dto.CompletedDate = app.CompletedDate;
                dto.CaseNumber = app.CaseNumber;
                dto.County = GetCounty(app.WorkflowXml);
                failedAppList.Add(dto);
            }

            return failedAppList;
        }

        private string GetCounty(string workflowXml)
        {
            try
            {
                XDocument doc = XDocument.Parse(workflowXml);
                string countyCode = doc.Descendants("householdContact")
                    .Descendants("contactCounty").FirstOrDefault().Value.ToString().Split().First().Trim();

                decimal dCountyCode = 0;

                if (!Decimal.TryParse(countyCode, out dCountyCode))
                    return "";

                using (MyDhrOacisContext ctx = new MyDhrOacisContext())
                {

                    var county = ctx.REF_COUNTY_STATE.Find(dCountyCode);
                    // TODO: this might not work but the compiler likes it
                    //.FirstOrDefaultAsync(c => c.CD_CNTY == dCountyCode);
                    if (county != null)
                        return county.CD_CNTY_DSCR.ToString();
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ApplicationInfo> GetNewFailedApps()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            List<CompletedApplication> appList = db.CompletedApplication
                .Where(ca => ca.Attempts == 6
                             && ca.Submitted == false
                             && (ca.FailedNotificationSent == null || ca.FailedNotificationSent == false))
                .OrderByDescending(a => a.CompletedDate).ToList();

            List<ApplicationInfo> failedAppList = new List<ApplicationInfo>();

            MapperConfiguration? config = new MapperConfiguration(cfg => cfg.CreateMap<MyDhrUser, User>());
            Mapper mapper = new Mapper(config);

            foreach (var app in appList)
            {
                ApplicationInfo dto = new ApplicationInfo();
                dto.WorkflowId = app.WorkflowId;
                dto.User = mapper.Map<User>(app.MyDhrUser);
                dto.UserId = app.UserId;
                dto.Submitted = app.Submitted;
                dto.SubmittedDate = app.SubmittedDate;
                dto.CompletedDate = app.CompletedDate;
                dto.CaseNumber = app.CaseNumber;
                dto.County = GetCounty(app.WorkflowXml);
                failedAppList.Add(dto);
            }

            return failedAppList;
        }

        public CompletedApplication FetchCompletedApplicationByUser(Guid userId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            CompletedApplication? dbApp = db.CompletedApplication.Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CompletedDate).FirstOrDefault();
            return dbApp;
        }

        public void DeleteApplication(Guid workflowId)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var app = db.CompletedApplication.First(a => a.WorkflowId == workflowId);
            db.CompletedApplication.Remove(app);
            //TODO: delete from phoenix working
        }

        public void AddCompletedApplication(CompletedApplication cad)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            if (cad.WorkflowXml != null)
            {
                cad.WorkflowXml = CheckUserLexisNexisApplication(cad.WorkflowXml);
            }

            var app = new CompletedApplication();
            app.UserId = cad.UserId;
            app.WorkflowId = cad.WorkflowId;
            app.WorkflowXml = cad.WorkflowXml;
            app.CompletedDate = DateTime.Now;
            app.Attempts = 0;
            app.Submitted = false;
            app.CaseNumber = cad.CaseNumber;
            db.CompletedApplication.Add(app);
            db.SaveChanges();
        }

        public void UpdateCompletedApplication(CompletedApplication cad)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var app = db.CompletedApplication.First(a => a.WorkflowId == cad.WorkflowId);
            app.UserId = cad.UserId;
            app.WorkflowXml = cad.WorkflowXml;
            app.CompletedDate = cad.CompletedDate;
            if (cad.SubmittedDate == DateTime.MinValue)
                app.SubmittedDate = null;
            else
                app.SubmittedDate = cad.SubmittedDate;
            app.Attempts = cad.Attempts;
            app.Submitted = cad.Submitted;
            db.SaveChanges();
        }


        public void UpdateNewFailedApps(ApplicationInfo app)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbApp = db.CompletedApplication.FirstOrDefault(a => a.WorkflowId == app.WorkflowId);
            if (dbApp != null)
            {
                dbApp.FailedNotificationSent = true;
            }

            db.SaveChanges();
        }

        public void SubmitApplication(string workflowXml)
        {
            workflowXml = CheckUserLexisNexisApplication(workflowXml);


            var client = new HttpClient();
            string apiUrl = "";
            //System.Configuration.ConfigurationManager.AppSettings["OacisServicesAPIUrl"] + "/";
            client.BaseAddress = new Uri(apiUrl);

            SubmitApplicationToDHR(workflowXml);
        }
        private void SubmitApplicationToDHR(string applicationXml)
    {
        LogMsg(applicationXml, "application_xml");
        using (var ctx = new MyDhrOacisContext())
        {
            // Populated by the PK returned by ApplicationEntry, which is the CaseID for a Household Member
            int newCaseId;

            // !! Check if the user has completed a LN Application !!
            //applicationXml = CheckUserLexisNexisApplication(applicationXml);

            //Convert string into XML document and error check
            var doc = ApplicationHelpers.checkAppXml(applicationXml);

            /*-------------------------------------------------------------------------------------------*/

            #region Normal App Logic

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

            //Parse through XDocument for Number in Household
            var numberInHousehold = doc.Descendants("householdNumberInHousehold").Select(n => new
            {
                number = Convert.ToInt32(n.Element("numberInHouseholdSpinner").Value)
            }).First();

            //Parse through XDocument for Contact
            var contact = doc.Descendants("householdContact").Select(c => new
            {
                homeCity = c.Element("contactHomeCity").Value,
                messagePhone = c.Element("contactMessagePhone").Value,
                mailingAddress = c.Element("contactMailingAddress").Value,
                homeZip = c.Element("contactHomeZip").Value,
                mailingAddress2 = c.Element("contactMailingAddress2").Value,
                mailingCity = c.Element("contactMailingCity").Value,
                mailingState = c.Element("contactMailingState").Value,
                mailingZip = c.Element("contactMailingZip").Value,
                homeState = c.Element("contactHomeState").Value,
                homeAddress2 = c.Element("contactHomeAddress2").Value,
                requestPhoneInterview = c.Element("contactRequestPhoneInterview").Value,
                cellPhone = c.Element("contactCellPhone").Value,
                workPhone = c.Element("contactWorkPhone").Value,
                homePhone = c.Element("contactHomePhone").Value,
                county = c.Element("contactCounty").Value,
                emailAddress = c.Element("contactEmailAddress").Value,
                sameAddress = c.Element("contactSameAddress").Value,
                homeAddress = c.Element("contactHomeAddress").Value
            }).First();

            var authorizedRep = doc.Descendants("householdAuthorizedRepresentatives").Select(a => new
            {
                certRepPhoneNumber = a.Element("authorizedRepresentativesCertRepPhoneNumber").Value,
                useCardSuffix = a.Element("authorizedRepresentativesUseCardSuffix").Value,
                certRepFirstName = a.Element("authorizedRepresentativesCertRepFirstName").Value,
                certRepSuffix = a.Element("authorizedRepresentativesCertRepSuffix").Value,
                certRepLastName = a.Element("authorizedRepresentativesCertRepLastName").Value,
                certRepMiddleName = a.Element("authorizedRepresentativesCertRepMiddleName").Value,
                useCardMiddleName = a.Element("authorizedRepresentativesUseCardMiddleName").Value,
                useCardFirstName = a.Element("authorizedRepresentativesUseCardFirstName").Value,
                useCardPhoneNumber = a.Element("authorizedRepresentativesUseCardPhoneNumber").Value,
                useCardLastName = a.Element("authorizedRepresentativesUseCardLastName").Value,
                authorizeUse = a.Element("authorizedRepresentativesAuthorizeToUseCard").Value,
                authorizedCert = a.Element("authorizedRepresentativesAuthorizeForCertification").Value
            }).First();

            //Parse through XDocument for householdApplicant
            var householdApplicant = doc.Descendants("householdApplicant").Select(h => new
            {
                phoneNumber = h.Element("applicantPhoneNumber").Value,
                zipCode = h.Element("applicantZip").Value,
                headOfHousehold = h.Element("applicantHeadOfHousehold").Value,
                city = h.Element("applicantCity").Value,
                state = h.Element("applicantState").Value,
                address = h.Element("applicantAddress").Value,
                address2 = h.Element("applicantAddress2").Value,
                suffix = h.Element("applicantSuffix").Value,
                firstName = h.Element("applicantFirstName").Value,
                member = h.Element("applicantHouseholdMember").Value,
                lastName = h.Element("applicantLastName").Value,
                middleName = h.Element("applicantMiddleName").Value,
                SSN = h.Element("applicantSocialSecurityNumber").Value,
                DOB = h.Element("applicantDOB").Value
            }).First();
            //Parse through XDocument for Household Member 
            var householdMembers = doc.Descendants("householdMember").Select(h => new
            {
                MemberInstanceKey = h.Element("householdMemberInstanceKey").Value,
                FirstName = h.Element("householdMemberFirstName").Value,
                Relationship = h.Element("householdMemberRelationship").Value,
                DOB = h.Element("householdMemberDOB").Value,
                SSN = h.Element("householdMemberSocialSecurityNumber").Value,
                Suffix = h.Element("householdMemberSuffix").Value,
                LastName = h.Element("householdMemberLastName").Value,
                MiddleName = h.Element("householdMemberMiddleName").Value,
                Sex = h.Element("householdMemberSex").Value,
                USCitizen = h.Element("householdMemberUSCitizen").Value,
                HispanicLatino = h.Element("householdMemberEthnicity").Value,
                IndianAlaskan = h.Element("householdMemberIndianOrAlaskan").Value,
                Asian = h.Element("householdMemberAsian").Value,
                Black = h.Element("householdMemberBlack").Value,
                White = h.Element("householdMemberWhite").Value,
                Hawaiian = h.Element("householdMemberHawaiin").Value,
                HigherLearning = h.Element("householdMemberHigherLearning").Value,
                School = h.Element("householdMemberHigherLearningSchool").Value,
                QuitJob = h.Element("householdMemberQuitJob").Value,
                QuitDate = h.Element("householdMemberQuitJobDateOfQuit").Value,
                QuitReason = h.Element("householdMemberQuitJobReasonOfQuit").Value,
                QuitEmployer = h.Element("householdMemberQuitJobEmployer").Value,
                QuitEmployerPhone = h.Element("householdMemberQuitJobEmployeePhoneNumber").Value,
                Striker = h.Element("householdMemberStriker").Value,
                StrikerEmployer = h.Element("householdMemberStrikerNameOfEmployer").Value,
                PaysRoomMeal = h.Element("householdMemberRoomMeal").Value,
                RoomMealName = h.Element("householdMemberRoomMealName").Value,
                ProgramViolation = h.Element("householdMemberProgramViolation").Value,
                GuiltySellingFoodAssistance = h.Element("householdMemberGuiltySellingFoodAssistance").Value,
                GuiltyBoughtGuns = h.Element("householdMemberBoughtGuns").Value,
                FraudIdentity = h.Element("householdMemberFraud").Value,
                DrugFelony = h.Element("householdMemberDrugFelony").Value,
                FledParole = h.Element("householdMemberFledParole").Value,
                SeasonMigrantWorker = h.Element("householdMemberSeasonMigrantWorker").Value,
                DuplicateBenefits = (string) h.Element("householdMemberDuplicateBenefits") ?? ""
            }).ToList();

            //Parse through XDocument for Household Info 
            var householdInfos = doc.Descendants("householdInfo").Select(householdInfo => new
            {
                fraudulentlyMisrepresented = householdInfo.Element("householdInfoFraudulentlyMisrepresented").Value,
                fleeingFelon = householdInfo.Element("householdInfoFleeingFelon").Value,
                duplicateBenefits = (string) householdInfo.Element("householdInfoDuplicateBenefits") ?? "",
                convictedOfFelony = householdInfo.Element("householdInfoConvictedOfFelony").Value,
                migrantWorker = householdInfo.Element("householdInfoSeasonalOrMigrantWorker").Value,
                guiltyForUsing = householdInfo.Element("householdInfoFoundGuiltyForUsing").Value,
                guiltyForSelling = householdInfo.Element("householdInfoFoundGuiltyForSelling").Value,
                payForRoomMeals = householdInfo.Element("householdInfoPayForRoomMeals").Value,
                householdDisqualified = householdInfo.Element("householdInfoHouseholdDisqualified").Value,
                voluntaryQuitJob = householdInfo.Element("householdInfoVoluntarilyQuitAJob").Value,
                striker = householdInfo.Element("householdInfoStriker").Value,
                higherLearning = householdInfo.Element("householdInfoEnrolledInHigherLearning").Value,
                lessThan100LiquidResources = householdInfo.Element("householdInfoLessthan100LiquidResources").Value,
                ipv = householdInfo.Element("ipv").Value
            }).ToList();
            //  Console.WriteLine(householdApplicant.member);

            //Parse through XDocument for Job Income 
            var jobIncomes = doc.Descendants("incomeJobIncome").Select(jobIncome => new
            {
                amount = jobIncome.Element("jobIncomeAmount").Value,
                employerPhoneNumber = jobIncome.Element("jobIncomeEmployerPhoneNumber").Value,
                employer = jobIncome.Element("jobIncomeEmployer").Value,
                name = jobIncome.Element("jobIncomeName").Value,
                frequency = jobIncome.Element("jobIncomeHowOften").Value,
                hoursWorked = (string) jobIncome.Element("jobIncomeHoursWorked") ?? "1"
            }).ToList();

            //Parse through XDocument for Self Employment Income
            var selfEmployIncomes = doc.Descendants("incomeSelfEmploymentIncome").Select(selfEmployIncome => new
            {
                businessType = selfEmployIncome.Element("selfEmploymentIncomeBusinessType").Value,
                businessName = selfEmployIncome.Element("selfEmploymentIncomeBusinessName").Value,
                name = selfEmployIncome.Element("selfEmploymentIncomeName").Value
            }).ToList();

            //Parse through XDocument for Room and Board Income
            var rabIncomes = doc.Descendants("incomeRoomAndBoardIncome").Select(rabIncome => new
            {
                amount = rabIncome.Element("roomAndBoardIncomeAmount").Value,
                name = rabIncome.Element("roomAndBoardIncomeName").Value,
                mealsPerDay = rabIncome.Element("roomAndBoardIncomeMealsPerDay").Value,
                frequency = rabIncome.Element("roomAndBoardIncomeHowOften").Value
            }).ToList();

            //Parse through XDocument for Unearned Income
            var unearnedIncomes = doc.Descendants("incomeUnearnedIncome").Select(unearnedIncome => new
            {
                Disability = unearnedIncome.Element("unearnedIncomeIsForDisability").Value,
                Amount = unearnedIncome.Element("unearnedIncomeAmount").Value,
                Frequency = unearnedIncome.Element("unearnedIncomeHowOften").Value,
                Name = unearnedIncome.Element("unearnedIncomeName").Value,
                Type = unearnedIncome.Element("unearnedIncomeType").Value
            }).ToList();

            //Parse through XDocument for Shelter 
            var shelter = doc.Descendants("expensesShelter").Select(s => new
            {
                homeInsuranceAmt = s.Element("shelterHomeInsuranceAmount").Value,
                lotRentFrequency = s.Element("shelterLotRentHowOften").Value,
                homeInsuranceFrequency = s.Element("shelterHomeInsuranceHowOften").Value,
                rentFrequency = s.Element("shelterRentOrMortgageHowOften").Value,
                describe = s.Element("shelterDescribe").Value,
                annualPropertyTaxAmt = s.Element("shelterAnnualPropertyTaxAmount").Value,
                lotRentAmt = s.Element("shelterLotRentAmount").Value,
                rentAmt = s.Element("shelterRentOrMortgageAmount").Value,
                ownOrRent = s.Element("shelterOwnOrRent").Value
            }).First();

            //Parse through XDocument for Medical Expenses
            var medicalExpenses = doc.Descendants("expensesMedical").Select(medicalExpense => new
            {
                incurredBy = medicalExpense.Element("medicalExpenseExpenseIncurredBy").Value,
                type = medicalExpense.Element("medicalExpenseTypeofMedicalExpense").Value
            }).ToList();

            //Parse through XDocument for Dependent Care
            var dependentCareExpenses = doc.Descendants("expensesDependentCare").Select(dependentCareExpense => new
            {
                name = dependentCareExpense.Element("dependentCareName").Value,
                amount = dependentCareExpense.Element("dependentCareAmount").Value,
                frequency = dependentCareExpense.Element("dependentCareHowOften").Value,
                provider = dependentCareExpense.Element("dependentCareProvider").Value,
                phoneNumber = dependentCareExpense.Element("dependentCarePhoneNumberOfPersonPaid").Value
            }).ToList();

            //Parse through XDocument for Child Support
            var childSupports = doc.Descendants("expensesChildSupport").Select(childSupport => new
            {
                NameOfChildren = childSupport.Element("childSupportNameOfChildren").Value,
                amount = childSupport.Element("childSupportAmount").Value,
                frequency = childSupport.Element("childSupportHowOften").Value,
                whoPays = childSupport.Element("childSupportWhoPays").Value,
                toWhomPaid = childSupport.Element("childSupportToWhom").Value
            }).ToList();

            //Parse through XDocument for Utilities Expense
            var utilityExpense = doc.Descendants("expensesUtilities").Select(u => new
            {
                AC = u.Element("utilitiesAC").Value,
                anyoneElsePay = u.Element("utilitiesAnyoneElsePay").Value,
                other = u.Element("utilitiesOther").Value,
                garbage = u.Element("utilitiesGarbage").Value,
                waterSewage = u.Element("utilitiesWaterSewage").Value,
                gas = u.Element("utilitiesGas").Value,
                electricity = u.Element("utilitiesElectricity").Value,
                oil = u.Element("utilitiesOil").Value,
                telephone = u.Element("utilitiesTelephone").Value,
                expectingLIHEAP = u.Element("utilitiesExpectingLIHEAP").Value,
                receivedLIHEAP = u.Element("utilitiesReceivedLIHEAP").Value,
                heat = u.Element("utilitiesHeat").Value,
                heatAc = (string) u.Element("utilitiesHeatAc") ?? "",
                liheap = (string) u.Element("utilitiesLiheap") ?? "",
                heatACMember = (string) u.Element("utilitiesHeatAcMember") ?? "",
                outsidePayName = (string) u.Element("utilitiesOutsidePayName") ?? ""
            }).First();

            //Parse through XDocument for Investments
            var investments = doc.Descendants("AvailableFundsInvestments").Select(investment => new
            {
                bank = investment.Element("InvestmentsBank").Value,
                name = investment.Element("InvestmentsName").Value,
                amount = investment.Element("InvestmentsAmount").Value
            }).ToList();

            //Parse through XDocument for Prepaid Burial Account
            var burialAccounts = doc.Descendants("availableFundsBurial").Select(burialAccount => new
            {
                name = burialAccount.Element("BurialAccountName").Value,
                amount = burialAccount.Element("BurialAccountAmount").Value,
                additionalInfo = burialAccount.Element("BurialAccountAdditionalInfo").Value
            }).ToList();

            //Parse through XDocument for Retirement Account
            var retirementAccounts =
                doc.Descendants("AvailableFundsRetirement").Select(retirementAccount => new
                {
                    name = retirementAccount.Element("RetirementName").Value,
                    amount = retirementAccount.Element("RetirementAmount").Value,
                    bank = retirementAccount.Element("RetirementBankName").Value
                }).ToList();

            //Parse through XDocument for Savings Account
            var savingsAccounts = doc.Descendants("AvailableFundsSavings").Select(savingsAccount => new
            {
                amount = savingsAccount.Element("SavingsAmount").Value,
                jointAccount = savingsAccount.Element("SavingsIsJointAccount").Value,
                jointAccountHolderName = savingsAccount.Element("SavingsJointAccountHolderName").Value,
                name = savingsAccount.Element("SavingsName").Value,
                accountNumber = savingsAccount.Element("SavingsNumber").Value,
                bankInformation = savingsAccount.Element("SavingsBankInfo").Value
            }).ToList();

            //Parse through XDocument for Checking Account
            var checkingAccounts = doc.Descendants("availableFundsChecking").Select(checkingAccount => new
            {
                bank = checkingAccount.Element("CheckingBankName").Value,
                name = checkingAccount.Element("CheckingName").Value,
                jointAccountHolderName = checkingAccount.Element("CheckingJointAccountHolderName").Value,
                amount = checkingAccount.Element("CheckingAmount").Value,
                accountNumber = checkingAccount.Element("CheckingAccountNumber").Value,
                isJointAccount = checkingAccount.Element("CheckingIsJointAccount").Value
            }).ToList();

            //Parse through XDocument for Cash on Hand 
            var cashOnHands = doc.Descendants("availableFundsCashOnHand").Select(cashOnHand => new
            {
                name = cashOnHand.Element("cashOnHandName").Value,
                amount = cashOnHand.Element("CashOnHandAmount").Value,
                addlInfo = cashOnHand.Element("CashOnHandAdditionalInfo").Value
            }).ToList();

            //Parse through XDocument for Trust Fund 
            var trustFunds = doc.Descendants("availableFundsTrustFunds").Select(trustFund => new
            {
                name = trustFund.Element("TrustFundsName").Value,
                amount = trustFund.Element("TrustFundsAmount").Value,
                addlInfo = trustFund.Element("TrustFundsAdditionalInformation").Value
            }).ToList();

            //Parse through XDocument for Other Liquid Resources
            var otherLiquidResources =
                doc.Descendants("availableFundsOther").Select(otherLiquidResource => new
                {
                    name = otherLiquidResource.Element("OtherName").Value,
                    amount = otherLiquidResource.Element("OtherAmount").Value,
                    addlInfo = otherLiquidResource.Element("OtherAdditionalInformation").Value
                }).ToList();

            //Parse through XDocument for Life Line 
            var lifeLines = from lifeLine in doc.Descendants("summaryLifeLine")
                select new
                {
                    doYouAccept = lifeLine.Element("lifeLineDoYouAccept").Value
                };

            //Parse through XDocument for Survey
            var locations = from location in doc.Descendants("summaryLocation")
                select new
                {
                    location = location.Element("locationLocation").Value
                };

            //Parse through XDocument for LexisNexis
            var lexisNexis = from ln in doc.Descendants("lexisNexis")
                select new
                {
                    potentiallyFraudulent = ln.Element("potentiallyFraudulent").Value,
                    OptOut = ln.Element("accountOptOut").Value //OptOut bit
                };

            /*-------------------------------------------------------------------------------------------*/

            // holds the number in household
            var number = householdMembers.Count;

            // holds the application id
            var id = instance.id;

            // holds the application start date
            var startDate = instance.StartDate;

            // This is an attempt to fix the incorrect application date issue
            // DHR was getting applications with the start date at the end of the next month
            // so we check to see if the xml date is weird, and if so, mitigate it.
            if (DateTime.Parse(startDate) > DateTime.Now) startDate = DateTime.Now.ToString();

            // holds the mydhr user id
            var userId = instance.UserID;

            // an array of member ids
            var Members = new Dictionary<string, int>();

            // an array of race codes
            var RaceCode = new Dictionary<string, int>();

            // get application type and case number
            string appType;
            var caseNumber = "";
            var dnaEntry = new REGISTER_DONOTAPPLY();

            try
            {
                var headOfHousehold = householdMembers.FirstOrDefault(hm => hm.MemberInstanceKey == "instance-0");
                dnaEntry =
                    ctx.REGISTER_DONOTAPPLY.FirstOrDefault(d => d.ID_SSN == headOfHousehold.SSN.Replace("-", ""));
            }
            catch (Exception e)
            {
                //var client = new HttpClient();
                //var apiUrl = ConfigurationManager.AppSettings["MyDhrApiBaseAddress"] + "/";
                //client.BaseAddress = new Uri(apiUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var smptClient = new SmtpClient();
                var mail = new MailMessage();

                var message = "Application with workflowId = " + instance.id +
                              " does not contain a household member" +
                              " with instanceKey-0 and cannot be processed. Please assign a household member" +
                              " instanceKey-0 and retry the submission.";

                mail.Body = message;
                mail.To.Add(ConfigurationManager.AppSettings["FailedAppsEmail"]);
                //mail.From = new MailAddress(ConfigurationManager.AppSettings["FromAddress"]);
                mail.Subject = "Failed Application Household Member Instance Key Error";

                smptClient.EnableSsl = false;
                smptClient.Port = 25;
                smptClient.UseDefaultCredentials = true;
                smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smptClient.Send(mail);

                //var email = new EmailDto()
                //{
                //    Subject = "Failed Application Information",
                //    Body = message,
                //    ToAddress = ConfigurationManager.AppSettings["FailedAppsEmail"]
                //};

                //var emailResponse = client.PostAsJsonAsync<EmailDto>("email/SendEmail", email).Result;
                //emailResponse.EnsureSuccessStatusCode();

                return;
            }

            //var headOfHousehold = householdMembers.FirstOrDefault(hm => hm.MemberInstanceKey == "instance-0");
            //var dnaEntry = ctx.REGISTER_DONOTAPPLY.FirstOrDefault(d => d.ID_SSN == headOfHousehold.SSN.Replace("-", ""));
            var userIdGuid = Guid.Parse(userId);
            var cas = ctx.CASE.FirstOrDefault(c => c.MYDHR_USERID == userIdGuid);
            if (dnaEntry != null)
            {
                //appType = "QUICK";
                appType = "NEW";
            }
            else if (cas != null)
            {
                caseNumber = cas.NO_CASE.ToString().Trim();
                if (caseNumber.Length != 9)
                    caseNumber = "0" + caseNumber;
                appType = "RECERT";
                //appType = "NEW";
            }
            else
            {
                appType = "NEW";
                caseNumber = null;
            }

            //}
            // Calculate expedited flag
            var expedite = "N";
            // Migrant worker flag
            string migrantWorker_flag = null;
            //  lessThan100LiquidResources flag

            string expense_flag = null;

            decimal totalIncome = 0;
            // Calculate total income
            foreach (var unearnedIncome in unearnedIncomes)
            {
                decimal f = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == unearnedIncome.Frequency).NO_FRQNCY_FACTOR.Value;

                totalIncome += decimal.Multiply(unearnedIncome.Amount.ToDecimal(), f);
            }

            foreach (var rabIncome in rabIncomes)
            {
                decimal f = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == rabIncome.frequency).NO_FRQNCY_FACTOR.Value;

                totalIncome += decimal.Multiply(rabIncome.amount.ToDecimal(), f);
            }

            foreach (var jobIncome in jobIncomes)
            {
                decimal f = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == jobIncome.frequency).NO_FRQNCY_FACTOR.Value;

                //if the person does not get paid by the hour
                if (jobIncome.frequency != "12")
                {
                    totalIncome += decimal.Multiply(jobIncome.amount.ToDecimal(), f);
                }

                //if the person is paid by the hour
                else
                {
                    var weeksInMonth = ctx.REF_FREQUENCY.ToList().First(fr => fr.ID_FRQNCY_REF.ToString() == "4")
                        .NO_FRQNCY_FACTOR.Value;
                    var hoursInMonth = decimal.Multiply(jobIncome.hoursWorked.ToDecimal(), weeksInMonth);
                    totalIncome += decimal.Multiply(jobIncome.amount.ToDecimal(), hoursInMonth);
                }
            }

            decimal totalExpenses = 0;
            // Calculate total expenses
            // Shelter expense
            if (shelter.homeInsuranceFrequency != "")
            {
                decimal insFreq = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == shelter.homeInsuranceFrequency).NO_FRQNCY_FACTOR
                    .Value;
                totalExpenses += decimal.Multiply(shelter.homeInsuranceAmt.ToDecimal(), insFreq);

                decimal lotRentFreq = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == shelter.lotRentFrequency).NO_FRQNCY_FACTOR.Value;
                totalExpenses += decimal.Multiply(shelter.lotRentAmt.ToDecimal(), lotRentFreq);

                decimal rentMortgageFreq = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF.ToString() == shelter.rentFrequency).NO_FRQNCY_FACTOR.Value;
                totalExpenses += decimal.Multiply(shelter.rentAmt.ToDecimal(), rentMortgageFreq);

                decimal propertyTaxFreq = ctx.REF_FREQUENCY.ToList()
                    .First(fr => fr.ID_FRQNCY_REF == 7).NO_FRQNCY_FACTOR.Value;
                totalExpenses += decimal.Multiply(shelter.annualPropertyTaxAmt.ToDecimal(), propertyTaxFreq);
            }
            // Utilities expense

            //this variable is used for utility expense import
            //"J" is the deduction code for no utility expense
            var deductionCode = "J";
            if (utilityExpense.heatAc == "true" || utilityExpense.liheap == "true")
            {
                deductionCode = "A";
                decimal SUA = ctx.REF_UTILITY_DEDUCTION
                    .Where(sua => sua.CD_UTLTY_DDCTN == deductionCode)
                    .Select(sua => sua.AM_UTLTY_DDCTN.Value).First();
                totalExpenses += SUA;
            }
            else
            {
                if (utilityExpense.telephone.ToLower() == "true"
                    && utilityExpense.electricity.ToLower() != "true"
                    && utilityExpense.gas.ToLower() != "true"
                    && utilityExpense.oil.ToLower() != "true"
                    && utilityExpense.waterSewage.ToLower() != "true"
                    && utilityExpense.garbage.ToLower() != "true"
                    && utilityExpense.other.ToLower() != "true")
                {
                    deductionCode = "D";
                    decimal phone = ctx.REF_UTILITY_DEDUCTION
                        .Where(sua => sua.CD_UTLTY_DDCTN == deductionCode)
                        .Select(sua => sua.AM_UTLTY_DDCTN.Value).First();
                    totalExpenses += phone;
                }

                var utilityFlagCount = 0;
                if (utilityExpense.telephone.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.electricity.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.gas.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.oil.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.waterSewage.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.garbage.ToLower() == "true")
                    utilityFlagCount++;
                if (utilityExpense.other.ToLower() == "true")
                    utilityFlagCount++;

                if (utilityFlagCount >= 2)
                {
                    deductionCode = "K";
                    decimal BUA = ctx.REF_UTILITY_DEDUCTION
                        .Where(sua => sua.CD_UTLTY_DDCTN == deductionCode)
                        .Select(sua => sua.AM_UTLTY_DDCTN.Value).First();
                    totalExpenses += BUA;
                }
            }

            // If migrant worker true, expedited = Y
            foreach (var householdInfo in householdInfos)
                if (householdInfo.migrantWorker == "true" && householdInfo.lessThan100LiquidResources == "true")
                {
                    expedite = "Y";
                    migrantWorker_flag = "Y";
                }
            //    if (householdInfo.migrantWorker == "true")
            //       migrantWorker_flag = "Y";

            // If resources<100 flag true AND income < 150, expedited = Y
            foreach (var householdInfo in householdInfos)
                if (householdInfo.lessThan100LiquidResources == "true" && totalIncome < 150)
                    expedite = "Y";
            //  if (householdInfo.lessThan100LiquidResources == "true")
            //     expense_flag = "Y";

            // If expenses > income, expedited = Y
            if (totalExpenses > totalIncome) expedite = "Y";

            // If no expenses or income, expedited = Y
            if (totalExpenses == 0 && totalIncome == 0) expedite = "Y";

            // If case is RECERT, expedited = N                
            if (appType == "RECERT") expedite = "N";

            // populates the RaceCodes array
            foreach (var householdMember in householdMembers)
            {
                var m = householdMember.MemberInstanceKey;

                // Create Race Flags
                var HispanicLatino = 0;
                var IndianAlaskan = 0;
                var Asian = 0;
                var Black = 0;
                var White = 0;
                var Hawaiian = 0;

                // Set Race Flags
                if (householdMember.HispanicLatino == "true")
                    HispanicLatino = 1;
                if (householdMember.IndianAlaskan.ToLower() == "true")
                    IndianAlaskan = 1;
                if (householdMember.Asian.ToLower() == "true")
                    Asian = 1;
                if (householdMember.Black.ToLower() == "true")
                    Black = 1;
                if (householdMember.White.ToLower() == "true")
                    White = 1;
                if (householdMember.Hawaiian.ToLower() == "true")
                    Hawaiian = 1;

                // Determine Race Code
                if (HispanicLatino == 0) // NOT Hispanic/Latino (1-32)
                {
                    if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 1;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 2;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 3;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 4;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 5;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 6;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 7;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 8;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 9;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 10;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 11;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 12;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 13;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 14;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 15;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 16;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 17;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 18;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 19;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 20;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 21;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 22;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 23;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 24;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 25;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 26;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 27;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 28;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 29;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 30;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 31;
                    else
                        RaceCode[m] = 32;
                }
                else if (HispanicLatino == 1) // Hispanic/Latino (33-64)
                {
                    if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 33;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 34;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 35;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 36;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 37;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 38;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 39;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 40;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 41;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 42;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 43;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 44;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 45;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 46;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 47;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 0)
                        RaceCode[m] = 48;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 49;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 50;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 51;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 52;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 53;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 54;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 55;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 56;
                    else if (IndianAlaskan == 0 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 57;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 58;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 0 && White == 1)
                        RaceCode[m] = 59;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 0 && Hawaiian == 1 && White == 0)
                        RaceCode[m] = 60;
                    else if (IndianAlaskan == 1 && Asian == 0 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 61;
                    else if (IndianAlaskan == 0 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 62;
                    else if (IndianAlaskan == 1 && Asian == 1 && Black == 1 && Hawaiian == 1 && White == 1)
                        RaceCode[m] = 63;
                    else
                        RaceCode[m] = 64;
                }
                else
                {
                    RaceCode[m] = 0;
                }
            }

            //IMPORT_ApplicationEntry
            // Home Phone
            var homePhone = ApplicationHelpers.FormatAsPhoneNumber(contact.homePhone);

            // Work Phone
            var workPhone = ApplicationHelpers.FormatAsPhoneNumber(contact.workPhone);

            // Message Phone
            var messagePhone = ApplicationHelpers.FormatAsPhoneNumber(contact.messagePhone);

            // Cell Phone
            var cellPhone = ApplicationHelpers.FormatAsPhoneNumber(contact.cellPhone);

            // AUREP_PHONE
            var crPhone = ApplicationHelpers.FormatAsPhoneNumber(authorizedRep.certRepPhoneNumber);

            // EBTREP_PHONE
            var ucPhone = ApplicationHelpers.FormatAsPhoneNumber(authorizedRep.useCardPhoneNumber);

            // Phone Interview
            var phoneInterview = "";
            if (contact.requestPhoneInterview == "true")
                phoneInterview = "Y";
            else if (contact.requestPhoneInterview == "false")
                phoneInterview = "N";

            var strMailAddress = contact.mailingAddress.Trim() + " " + contact.mailingAddress2.Trim();

            string mailAdd1;
            string mailAdd2 = null;
            if (strMailAddress.Length > 25)
            {
                mailAdd1 = strMailAddress.Substring(0, 25);

                if (!string.IsNullOrEmpty(strMailAddress.Substring(25)))
                {
                    if (strMailAddress.Substring(25).Length > 24)
                        mailAdd2 = strMailAddress.Substring(25, 25);
                    else
                        mailAdd2 = strMailAddress.Substring(25);
                }
            }
            else
            {
                mailAdd1 = strMailAddress;
            }

            var strResAddress = contact.homeAddress.Trim() + " " + contact.homeAddress2.Trim();

            string resAdd1;
            string resAdd2 = null;
            if (strResAddress.Length > 25)
            {
                resAdd1 = strResAddress.Substring(0, 25);

                if (!string.IsNullOrEmpty(strResAddress.Substring(25)))
                {
                    if (strResAddress.Substring(25).Length > 24)
                        resAdd2 = strResAddress.Substring(25, 25);
                    else
                        resAdd2 = strResAddress.Substring(25);
                }
            }
            else
            {
                resAdd1 = strResAddress;
            }

            var submitCountyOfficeId = contact.county;
            var resideCountyOfficeId = contact.county; //new
            //Handle Jefferson and Mobile county
            if (submitCountyOfficeId == "")
            {
                var zipCode = contact.homeZip;

                var county = ApplicationHelpers.HandleJeffersonCounty(zipCode);

                resideCountyOfficeId = county; //new
                submitCountyOfficeId = county;
            }
            else if (submitCountyOfficeId == "49 02")
            {
                var zipCode = contact.homeZip;

                var county = ApplicationHelpers.HandleMobileCounty(zipCode);

                resideCountyOfficeId = county; //new
                submitCountyOfficeId = county;
            }

            //Determine AESAP
            var householdCount = householdMembers.Count(m =>
                (DateTime.Now - (m.DOB == "" ? DateTime.Today : Convert.ToDateTime(m.DOB))).Days / 365 < 60);
            var incomeCount = jobIncomes.Count + selfEmployIncomes.Count + rabIncomes.Count;
            if (householdCount.Equals(0) && incomeCount.Equals(0))
                submitCountyOfficeId = "70 02";
            
            //new here
            var countyCodesSub = submitCountyOfficeId.Split(new[] {' '});
            var countyCodes = resideCountyOfficeId.Split(new[] {' '});

            var applicantName = householdApplicant.firstName;
            if (householdApplicant.middleName.Trim() != "")
                applicantName += " " + householdApplicant.middleName;
            applicantName += " " + householdApplicant.lastName;
            if (householdApplicant.suffix.Trim() != "")
                applicantName += " " + householdApplicant.suffix;
            
            var applicantPhone = Regex.Replace(householdApplicant.phoneNumber, "[^0-9.]", "");

            var applicantAddress = "";
            if (householdApplicant.member != "true")
            {
                applicantAddress += householdApplicant.address;
                if (householdApplicant.address2.Trim() != "")
                    applicantAddress += ", " + householdApplicant.address2;
                applicantAddress += ", " + householdApplicant.city + ", " + householdApplicant.state + " " +
                                    householdApplicant.zipCode;
            }

            string certRepFirstName = null;
            string certRepMiddleName = null;
            var certRepLastName = "NONE";

            if (authorizedRep.authorizedCert.ToLower() == "true")
            {
                certRepFirstName = authorizedRep.certRepFirstName.Trim();
                certRepMiddleName = authorizedRep.certRepMiddleName.Trim();
                certRepLastName = authorizedRep.certRepLastName.Trim();
            }

            string useCardFirstName = null;
            string useCardMiddleName = null;
            var useCardLastName = "NONE";

            //check values of LexisNexis for Nulls
            bool? _lnpotentiallyFraudulent = string.IsNullOrEmpty(lexisNexis.FirstOrDefault().potentiallyFraudulent)
                ? null
                : Convert.ToBoolean(lexisNexis.FirstOrDefault().potentiallyFraudulent);
            bool? _lnoptout = string.IsNullOrEmpty(lexisNexis.FirstOrDefault().OptOut)
                ? null
                : Convert.ToBoolean(lexisNexis.FirstOrDefault().OptOut);

            var lnpotentiallyFraudulent = true;
            if (_lnpotentiallyFraudulent.HasValue) lnpotentiallyFraudulent = _lnpotentiallyFraudulent.Value;

            var lnoptout = true;
            if (_lnoptout.HasValue) lnoptout = _lnoptout.Value;


            if (authorizedRep.authorizeUse.ToLower() == "true")
            {
                useCardFirstName = authorizedRep.useCardFirstName.Trim();
                useCardMiddleName = authorizedRep.useCardMiddleName.Trim();
                useCardLastName = authorizedRep.useCardLastName.Trim();
            }

            var ret = ctx.IMPORT_ApplicationEntry(
                Convert.ToDateTime(startDate).ToString("MMddyyyy"),
                DateTime.Now.ToString("MMddyyyy"),
                id,
                userId,
                appType,
                caseNumber,
                countyCodesSub[0],
                countyCodesSub[1],
                mailAdd1,
                mailAdd2,
                contact.mailingCity,
                contact.mailingState,
                contact.mailingZip,
                countyCodes[0],
                resAdd1,
                resAdd2,
                contact.homeCity,
                contact.homeState,
                contact.homeZip,
                countyCodes[0],
                countyCodes[1],
                homePhone,
                workPhone,
                messagePhone,
                cellPhone,
                contact.emailAddress,
                certRepFirstName,
                certRepMiddleName,
                certRepLastName,
                crPhone,
                useCardFirstName,
                useCardMiddleName,
                useCardLastName,
                ucPhone,
                0,
                0,
                expedite,
                contact.mailingAddress.Trim() + '|' + contact.mailingAddress2.Trim(),
                phoneInterview,
                applicantName,
                applicantAddress,
                applicantPhone,
                householdApplicant.member == "true" ? "N" : "Y",
                migrantWorker_flag,
                expense_flag,
                lnpotentiallyFraudulent,
                lnoptout
            ).Single();

            newCaseId = Convert.ToInt32(ret);

            //If anything here fails, we need to delete existing partial data
            try
            {
                ctx.INS_REGISTER_APPLICATION_STATUS(id);

                ctx.IMPORT_UpdateAppStatusTbl(id, true);

                //IMPORT_MemberEntry
                var memberCount = 2;
                householdMembers = householdMembers.OrderBy(h => Convert.ToInt32(h.MemberInstanceKey.Substring(9)))
                    .ToList();
                foreach (var householdMember in householdMembers)
                {
                    var i = householdMember.MemberInstanceKey;

                    int dependentNo;
                    if (i == "instance-0")
                    {
                        dependentNo = 1;
                    }
                    else
                    {
                        dependentNo = memberCount;
                        memberCount++;
                    }

                    var hoh = "N";
                    if (i == "instance-0") hoh = "Y";

                    string citizen;
                    if (householdMember.USCitizen == "true")
                        citizen = "Y";
                    else if (householdMember.USCitizen == "false")
                        citizen = "N";
                    else
                        citizen = "U";

                    //changed to better handle date time parsing
                    string dob;
                    DateTime dobDate;
                    if (DateTime.TryParse(householdMember.DOB, out dobDate))
                        dob = dobDate.ToString("MMddyyyy");
                    else
                        dob = null; //DateTime.Today.ToString("MMddyyyy");

                    var sex = householdMember.Sex;
                    if (sex == "") sex = "U";

                    string ethnicity;
                    if (householdMember.HispanicLatino == "true")
                        ethnicity = "2";
                    else if (householdMember.HispanicLatino == "false")
                        ethnicity = "1";
                    else
                        ethnicity = "N";

                    Members[i] = Convert.ToInt32(
                        ctx.IMPORT_MemberEntry(
                            newCaseId,
                            dependentNo,
                            hoh,
                            id,
                            null,
                            householdMember.Relationship,
                            ethnicity,
                            citizen,
                            householdMember.SSN.Replace("-", ""),
                            householdMember.FirstName,
                            householdMember.MiddleName,
                            householdMember.LastName,
                            householdMember.Suffix,
                            dob,
                            sex,
                            Convert.ToString(RaceCode[i])).Single()
                    );

                    /* FLAGS **********************************************************************/

                    // QuitEmployerPhone
                    var quitEmployerPhone = ApplicationHelpers.FormatAsPhoneNumber(householdMember.QuitEmployerPhone);

                    if (householdMember.QuitJob == "true")
                    {
                        string quitDate;
                        DateTime quitDateDate;
                        if (DateTime.TryParse(householdMember.QuitDate, out quitDateDate))
                            quitDate = quitDateDate.ToString("MMddyyyy");
                        else
                            quitDate = DateTime.Today.ToString("MMddyyyy");

                        ctx.IMPORT_VoluntaryQuitAsync(
                            Members[i],
                            householdMember.QuitEmployer,
                            quitEmployerPhone,
                            householdMember.QuitReason,
                            quitDate
                        );
                    }

                    if (householdMember.DrugFelony == "true") ctx.IMPORT_FelonAsync(Members[i]);

                    if (householdMember.SeasonMigrantWorker == "true") ctx.IMPORT_MigrantWorker(Members[i]);

                    if (householdMember.FledParole == "true") ctx.IMPORT_Parole(Members[i]);

                    if (householdMember.ProgramViolation == "true") ctx.IMPORT_StampFraud(Members[i]);

                    if (householdMember.GuiltyBoughtGuns == "true") ctx.IMPORT_StampsForWeapons(Members[i]);

                    if (householdMember.GuiltySellingFoodAssistance == "true") ctx.IMPORT_StampTrafficking(Members[i]);

                    if (householdMember.Striker == "true")
                        ctx.IMPORT_Striker(
                            Members[i],
                            householdMember.StrikerEmployer
                        );

                    if (householdMember.HigherLearning == "true")
                        ctx.IMPORT_Student(
                            Members[i],
                            householdMember.School
                        );

                    if (householdMember.DuplicateBenefits == "true") ctx.IMPORT_DuplicateBenefits(Members[i]);
                }

                /* FLAGS **********************************************************************/

                //IMPORT_Employment
                foreach (var jobIncome in jobIncomes)
                {
                    // jobEmployerPhone
                    var jobEmployerPhone = ApplicationHelpers.FormatAsPhoneNumber(jobIncome.employerPhoneNumber);
                    //if (jobEmployerPhone.Trim().Length >= 10)
                    //{
                    //    //jobEmployerPhone = jobEmployerPhone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
                    //    jobEmployerPhone = Regex.Replace(jobEmployerPhone, "[^0-9.]", "");
                    //    jobEmployerPhone = Convert.ToInt64(jobEmployerPhone).ToString("###-###-####");
                    //}

                    ctx.IMPORT_Employment(
                        Members[jobIncome.name],
                        jobIncome.employer,
                        jobEmployerPhone,
                        jobIncome.amount.ToDecimal(),
                        jobIncome.frequency,
                        (int) jobIncome.hoursWorked.ToDecimal()
                    );
                }

                //IMPORT_SelfEmployment
                foreach (var selfEmployIncome in selfEmployIncomes)
                    ctx.IMPORT_SelfEmployment(
                        Members[selfEmployIncome.name],
                        selfEmployIncome.businessName,
                        selfEmployIncome.businessType
                    );

                //IMPORT_RoomerBoarder
                foreach (var rabIncome in rabIncomes)
                {
                    short mpd;
                    if (rabIncome.mealsPerDay == "")
                        mpd = 0;
                    else
                        mpd = Convert.ToInt16(rabIncome.mealsPerDay);

                    ctx.IMPORT_RoomerBoarder(
                        Members[rabIncome.name],
                        rabIncome.amount.ToDecimal(),
                        rabIncome.frequency,
                        mpd
                    );
                }

                //IMPORT_UnearnedIncome
                foreach (var unearnedIncome in unearnedIncomes)
                    ctx.IMPORT_UnearnedIncome(
                        Members[unearnedIncome.Name],
                        unearnedIncome.Type,
                        unearnedIncome.Amount.ToDecimal(),
                        unearnedIncome.Frequency
                    );

                //IMPORT_ShelterExpense
                ctx.IMPORT_ShelterExpense(
                    Members["instance-0"], // Head of Household
                    shelter.ownOrRent,
                    shelter.describe,
                    shelter.rentAmt.ToDecimal(),
                    shelter.lotRentAmt.ToDecimal(),
                    shelter.lotRentFrequency,
                    shelter.annualPropertyTaxAmt.ToDecimal(),
                    shelter.homeInsuranceAmt.ToDecimal(),
                    shelter.homeInsuranceFrequency
                );

                //IMPORT_MedicalExpense 
                foreach (var medicalExpense in medicalExpenses)
                    ctx.IMPORT_MedicalExpense(
                        Members[medicalExpense.incurredBy],
                        medicalExpense.type,
                        0); //Convert.ToDecimal(medicalExpense.amount)

                //IMPORT_DependentCare
                foreach (var dependentCareExpense in dependentCareExpenses)
                {
                    // Dependent Care Phone
                    var dependentCarePhone = ApplicationHelpers.FormatAsPhoneNumber(dependentCareExpense.phoneNumber, false);
                    //if (dependentCarePhone.Trim().Length >= 10)
                    //{
                    //    //dependentCarePhone = dependentCarePhone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
                    //    dependentCarePhone = Regex.Replace(dependentCarePhone, "[^0-9.]", "");
                    //    dependentCarePhone = Convert.ToInt64(dependentCarePhone).ToString("###-###-####");
                    //}

                    ctx.IMPORT_DependentCare(
                        Members[dependentCareExpense.name],
                        dependentCareExpense.amount.ToDecimal(),
                        dependentCareExpense.frequency,
                        dependentCareExpense.provider,
                        dependentCarePhone
                    );
                }

                //IMPORT_ChildSupport
                foreach (var childSupport in childSupports)
                    ctx.IMPORT_ChildSupport(
                        Members[childSupport.whoPays],
                        childSupport.toWhomPaid,
                        childSupport.amount.ToDecimal(),
                        childSupport.frequency,
                        childSupport.NameOfChildren
                    );

                //IMPORT_UtilityExpense
                var heatAc = "N";
                int? hacMem = null;
                if (utilityExpense.heatAc.ToLower() == "true")
                {
                    heatAc = "Y";
                    hacMem = Members[utilityExpense.heatACMember];
                }

                var liheap = "N";
                if (utilityExpense.liheap.ToLower() == "true")
                    liheap = "Y";

                var phone = "N";
                if (utilityExpense.telephone.ToLower() == "true")
                    phone = "Y";

                var electricity = "N";
                if (utilityExpense.electricity.ToLower() == "true")
                    electricity = "Y";

                var gas = "N";
                if (utilityExpense.gas.ToLower() == "true")
                    gas = "Y";

                var oil = "N";
                if (utilityExpense.oil.ToLower() == "true")
                    oil = "Y";

                var water = "N";
                if (utilityExpense.waterSewage.ToLower() == "true")
                    water = "Y";

                var garbage = "N";
                if (utilityExpense.garbage.ToLower() == "true")
                    garbage = "Y";

                var other = "N";
                if (utilityExpense.other.ToLower() == "true")
                    other = "Y";

                var outsidePay = "N";
                string outsidePayName = null;
                if (utilityExpense.anyoneElsePay.ToLower() == "true")
                {
                    outsidePay = "Y";
                    outsidePayName = utilityExpense.outsidePayName;
                }

                ctx.IMP_Util_Exp(
                    newCaseId,
                    deductionCode,
                    hacMem,
                    heatAc,
                    liheap,
                    phone,
                    electricity,
                    gas,
                    oil,
                    water,
                    garbage,
                    other,
                    outsidePay,
                    outsidePayName
                );

                //IMPORT_LiquidResource
                foreach (var investment in investments)
                    ctx.IMPORT_LiquidResource(
                        Members[investment.name],
                        "5",
                        investment.amount.ToDecimal(),
                        investment.bank,
                        null,
                        null,
                        null
                    );

                foreach (var burialAccount in burialAccounts)
                    ctx.IMPORT_LiquidResource(
                        Members[burialAccount.name],
                        "8",
                        burialAccount.amount.ToDecimal(),
                        burialAccount.additionalInfo,
                        null,
                        null,
                        null
                    );

                foreach (var retirementAccount in retirementAccounts)
                    ctx.IMPORT_LiquidResource(
                        Members[retirementAccount.name],
                        "6",
                        retirementAccount.amount.ToDecimal(),
                        retirementAccount.bank,
                        null,
                        null,
                        null
                    );

                foreach (var savingsAccount in savingsAccounts)
                {
                    var isJointAccount = "";
                    if (savingsAccount.jointAccount == "true")
                        isJointAccount = "Y";
                    else if (savingsAccount.jointAccount == "false")
                        isJointAccount = "N";

                    ctx.IMPORT_LiquidResource(
                        Members[savingsAccount.name],
                        "3",
                        savingsAccount.amount.ToDecimal(),
                        null,
                        null,
                        isJointAccount,
                        savingsAccount.jointAccountHolderName
                    );
                }

                foreach (var checkingAccount in checkingAccounts)
                {
                    var isJointAccount = "";
                    if (checkingAccount.isJointAccount == "true")
                        isJointAccount = "Y";
                    else if (checkingAccount.isJointAccount == "false")
                        isJointAccount = "N";

                    ctx.IMPORT_LiquidResource(
                        Members[checkingAccount.name],
                        "4",
                        checkingAccount.amount.ToDecimal(),
                        checkingAccount.bank,
                        null,
                        isJointAccount,
                        checkingAccount.jointAccountHolderName
                    );
                }

                foreach (var cashOnHand in cashOnHands)
                    ctx.IMPORT_LiquidResource(
                        Members[cashOnHand.name],
                        "2",
                        cashOnHand.amount.ToDecimal(),
                        cashOnHand.addlInfo,
                        null,
                        null,
                        null
                    );

                foreach (var trustFund in trustFunds)
                    ctx.IMPORT_LiquidResource(
                        Members[trustFund.name],
                        "7",
                        trustFund.amount.ToDecimal(),
                        trustFund.addlInfo,
                        null,
                        null,
                        null
                    );

                foreach (var otherLiquidResource in otherLiquidResources)
                    ctx.IMPORT_LiquidResource(
                        Members[otherLiquidResource.name],
                        "9",
                        otherLiquidResource.amount.ToDecimal(),
                        otherLiquidResource.addlInfo,
                        null,
                        null,
                        null
                    );

                //IMPORT_UpdateApplication
                // inserts the HOHID into the Application Entry
                ctx.IMPORT_UpdateApplication(newCaseId);

                ctx.IMPORT_UpdateAppStatusTbl(id, false);

                //IMPORT_CheckHoliday               *****************************
                //IMPORT_DeleteApplication          *****************************
                //IMPORT_UpdateAppStatusTbl         *****************************
                //IMPORT_GetAppStatus               *****************************
            }
            catch (Exception ex)
            {
                ex.Data.Add("WorkflowId", instance.id);
                var message = ex.Message + string.Format(" WorkflowId = {0} ", instance.id);
                var newEx = new Exception(message, ex);
                ctx.IMPORT_DeleteApplication(newCaseId);
                ExceptionDispatchInfo.Capture(newEx).Throw();
            }

            #endregion
        }
    }

        
        //checkDate should be in MM/dd/yyyy format
        //public string CheckHoliday(string checkDate)
        //{
        //    var response = MakeAPICall<string>("CheckHoliday", "?checkDate=" + checkDate, "Application");

        //    return response;
        //}

        //private string GetCounty(string workflowXml)
        //{
        //    try
        //    {
        //        XDocument doc = XDocument.Parse(workflowXml);
        //        string countyCode = doc.Descendants("householdContact")
        //                            .Descendants("contactCounty").FirstOrDefault().Value.ToString().Split().First().Trim();
        //        var county = MakeAPICall<string>("GetCountyFromCountyCode", "?countyCode=" + countyCode).Trim();
        //        return county;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //private T MakeAPICall<T>(string actionName, string queryString, string controllerName = "oacis") where T : class
        //{
        //    string path = APIUrl + "/" + controllerName + "/" + actionName + queryString;
        //    HttpClientHandler handler = new HttpClientHandler();
        //    handler.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        //    HttpClient client = new HttpClient(handler);
        //    client.BaseAddress = new Uri(APIUrl);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var response = client.GetAsync(path).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var o = response.Content.ReadAsAsync<T>().Result;
        //        return o;
        //    }
        //    return null;
        //}

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
            return result;
        }

        private string SerializeLnDataToApplication(string appXml, LnDbEntry entry)
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
            var CONN_STRING = "";
            //var CONN_STRING = ConfigurationManager.AppSettings["MyDhrDatabase"];
            //int pos = _CONN_STRING.IndexOf("connection string=");
            //var CONN_STRING = _CONN_STRING.Substring(pos + 19);
            //CONN_STRING = CONN_STRING.Remove(CONN_STRING.Length - 1);
            //LogMsg(CONN_STRING, $"connection_string_{DateTime.Now.Ticks}");

            using (var conn = new SqlConnection(CONN_STRING))
            {
                var cmd = new SqlCommand($"SELECT * FROM [dbo].[LexisNexis] WHERE [UserId] = '{Id}'", conn) { CommandType = CommandType.Text };
                conn.Open();
                var rdr = cmd.ExecuteReader(CommandBehavior.KeyInfo);

                while (rdr.Read())
                {
                    entry.UserId = rdr.IsDBNull(0) ? new Guid() : rdr.GetGuid(0);
                    entry.AppId = rdr.IsDBNull(1) ? "" : rdr.GetString(1).Trim();
                    entry.RequestData = rdr.IsDBNull(2) ? "" : rdr.GetString(2).Trim();
                    entry.ResponseData = rdr.IsDBNull(3) ? "" : rdr.GetString(3).Trim();
                    entry.CVI = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4);
                    entry.RiskData = rdr.IsDBNull(5) ? "" : rdr.GetString(5).Trim();
                    entry.PotentiallyFraudulent = rdr.IsDBNull(6) ? true : rdr.GetBoolean(6);
                    entry.AppStartDate = rdr.IsDBNull(7) ? DateTime.MinValue : rdr.GetDateTime(7);

                    entry.AccountEmail = rdr.IsDBNull(8) ? "" : rdr.GetString(8).Trim();
                    entry.AccountTelephone = rdr.IsDBNull(9) ? "" : rdr.GetString(9).Trim();
                    entry.AccountAddressStreet1 = rdr.IsDBNull(10) ? "" : rdr.GetString(10).Trim();
                    entry.AccountAddressStreet2 = rdr.IsDBNull(11) ? "" : rdr.GetString(11).Trim();
                    entry.AccountAddressCity = rdr.IsDBNull(12) ? "" : rdr.GetString(12).Trim();
                    entry.AccountAddressZip = rdr.IsDBNull(13) ? "" : rdr.GetString(13).Trim();
                    entry.AccountAddressState = rdr.IsDBNull(14) ? "" : rdr.GetString(14).Trim();
                    entry.AccountAddressCountry = rdr.IsDBNull(15) ? "" : rdr.GetString(15).Trim();
                    entry.AccountDateOfBirth = rdr.IsDBNull(16) ? DateTime.MinValue : rdr.GetDateTime(16);
                    entry.AccountFirstName = rdr.IsDBNull(17) ? "" : rdr.GetString(17).Trim();
                    entry.AccountLastName = rdr.IsDBNull(18) ? "" : rdr.GetString(18).Trim();
                    entry.SsnRaw = rdr.IsDBNull(19) ? "" : rdr.GetString(19).Trim();
                    entry.Submitted = rdr.IsDBNull(20) ? false : rdr.GetBoolean(20);
                    entry.Approved = rdr.IsDBNull(21) ? false : rdr.GetBoolean(21);
                }

                conn.Close();
            }
            return entry;
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
