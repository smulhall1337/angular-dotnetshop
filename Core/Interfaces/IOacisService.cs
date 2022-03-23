using Core.Entities.Dhr;
using Core.Entities.Oacis;

namespace Core.Interfaces
{
    public interface IOacisService
    {
        string LinkCase(OacisLinkCase dto);

        string UnlinkCase(OacisLinkCase dto);

        string RepresentativeCanViewCase(string caseNumber, string ssn, string firstName, string lastName);

        string GetCaseFoodAssistanceWorkerEmail(string caseNumber);

        MailingAddress GetCountyOffice(string caseNumber);

        string GetCountyOfficePhone(string caseNumber);

        MailingAddress GetCaseAddresses(string type, string caseNumber);

        string GetCaseStatus(string caseNumber);

        DateTime GetNextRecertificationDate(string caseNumber);

        DateTime GetCertificationStartDate(string caseNumber);

        DateTime GetCertificationEndDate(string caseNumber);

        string GetHeadOfHouseholdName(string caseNumber);

        string GetCaseCounty(string caseNumber);

        VerificationItem[] GetVerificationItems(string caseNumber);

        AuthRep[] GetAuthorizedReps(string caseNumber);

        Allotment[] GetAllotmentHistory(string caseNumber);

        decimal GetIncomeLimit(string caseNumber);

        decimal GetTotalChildSupportPayments(string caseNumber);

        IncomeExpenseEntry[] GetIncomesAndChildSupport(string caseNumber);

        DateTime GetNextAppointment(string caseNumber);

        string GetOfficePhone(string caseNumber);

        DateTime GetSemiAnnualReviewDate(string caseNumber);

        DateTime GetStatusDate(string caseNumber);

        DateTime GetAnnualReviewDate(string caseNumber);

        string[] GetHouseholdNames(string caseNumber);

        string GetReviewCode(string caseNumber);

        RegisterApplication GetCompletedApplicationByCaseNumber(string caseNumber);

        string GetClosureCode(string caseNumber);

        bool UserCanAccessVerifications(string caseNumber);

        Allotment GetSNAPVerification(string caseNumber);

        bool UserCanAccessComplaint(string caseNumber);

        bool UserIsOnDNAList(string SSN);

        Guid GetUserIdByCaseNumber(string caseNumber);

    }
}
