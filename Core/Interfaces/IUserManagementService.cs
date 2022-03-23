

using Core.Entities.Dhr;

namespace Core.Interfaces
{
    public interface IUserManagementService
    {
        Session Authenticate(UserLogin loginDto);

        bool CheckPassword(UserLogin dto);

        void ChangePassword(UserLogin dto);

        Session UpdateCaseNumber(Session newSession);
 
        bool MakeUserAdmin(Guid userId);

        bool MakeUserNotAdmin(Guid userId);

        Session ValidateSession(Session sessionId);

        UserLogin CreateUser(UserLogin user);

        User UpdateUser(User user);

        User UpdateMyAlUserToMyDhr(User user);

        User FetchUser(Guid id);

        String FetchUserSex(Guid id);

        PrefillInfo PrefillUserInfo(Guid id);

        User FetchUserByUsername(string username);

        bool UsernameAlreadyExists(string username);

        User LinkUser(User link);

        User FetchUserForgotPassword(string email);

        bool ResetPassword(ResetPassword dto);

        bool ValidateResetToken(Guid resetToken);

        string GetReviewStatus(ReviewStatus dto);

        List<UserSearch> SearchUsers(UserSearchParameters dto);

        List<CompletedApplicationSummary> FetchCompletedApplicationsByUserId(Guid userId);

        bool RemoveCompletedApplicationByWorkflowId(string id);

        RegisterApplication FetchCompletedApplicationByCaseNumber(string caseNumber);

        List<IPApplicationInfo> FetchIPsWithMultipleApplications();

        List<CompletedApplicationSummary> FetchSubmittedAppsByIPAddress(byte[] ipAddress);
    }
}
