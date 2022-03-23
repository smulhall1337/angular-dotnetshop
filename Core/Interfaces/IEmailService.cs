using Core.Entities.Dhr;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        void WriteLogEntry(Email dto);
        string getLinkedCase(Guid userId);
        List<AttachmentFile> FetchEmailAttachmentsByUserId(Guid userId);
        AttachmentFile FetchEmailAttachmentByAttachmentId(Guid attachmentId);
        Email FetchEmailById(Guid emailId);
        IEnumerable<Email> GetAnnualReportsFromDate(DateTime date);
        IEnumerable<Email> GetSemiAnnualReportsFromDate(DateTime date);
        bool RemoveEmailAttachmentByAttachmentId(Guid attachmentId);
        bool sendEmail(Email dto);

    }
}
