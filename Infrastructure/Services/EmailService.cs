using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailService: IEmailService
    {
        static IConfiguration conf = (JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()), "appsettings.json").Build());

        //public void WriteLogEntry(EmailDto dto)
        //{
        //    FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
        //    string caseNumber = getLinkedCase(dto.UserId);

        //    Email e = new Email();
        //    e.Id = dto.Id;
        //    e.Subject = dto.Subject;
        //    e.Body = dto.Body;
        //    e.FromAddress = dto.FromAddress;
        //    e.ToAddress = dto.ToAddress;
        //    e.UserId = dto.UserId;
        //    e.Date = dto.Date;

        //    if (dto.AttachmentFiles != null)
        //    {
        //        foreach (var file in dto.AttachmentFiles)
        //        {
        //            AttachmentFile af = new AttachmentFile();
        //            af.Id = file.Id;
        //            af.EmailId = file.EmailId;
        //            af.FileContent = file.FileContent;
        //            af.FileName = file.FileName;

        //            db.AttachmentFile.Add(af);
        //        }
        //    }

        //    db.Email.Add(e);
        //    db.SaveChanges();
        //}

        private static string APIUrl { get; } = conf["OacisServicesAPIUrl"];
        //string APIUrl = System.Configuration.ConfigurationManager.AppSettings["OacisServicesAPIUrl"] + "/";

        //public string getLinkedCase(Guid userId)
        //{
        //    string queryString = "?userId=" + userId.ToString();
        //    string actionName = "GetLinkedCase";

        //    var response = MakeAPICall<string>(actionName, queryString);
        //    return response;
        //}

        
        public List<AttachmentFileDto> FetchEmailAttachmentsByUserId(Guid userId)
        {
            FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            IQueryable<Email> dbEmail = db.Email.Where(a => a.UserId == userId);
            List<AttachmentFile> dbAttachments = new List<AttachmentFile>();
            List<AttachmentFileDto> returnList;
            foreach (Email email in dbEmail)
            {
                var temp = db.AttachmentFile.Where(a => a.EmailId == email.Id);
                foreach (AttachmentFile attach in temp)
                {
                    attach.FileContent = null;
                    dbAttachments.Add(attach);
                }
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<AttachmentFile[], IEnumerable<AttachmentFileDto>>());
            Mapper mapper = new Mapper(config);
            return mapper.Map <List<AttachmentFileDto>> (dbAttachments.ToArray());
            //return mapper.Map<AttachmentFile[], IEnumerable<AttachmentFileDto>>(dbAttachments.ToArray()).ToList();
        }

        
        public AttachmentFileDto FetchEmailAttachmentByAttachmentId(Guid attachmentId)
        {
            FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var dbAttachment = db.AttachmentFile.Where(a => a.Id == attachmentId).FirstOrDefault();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<AttachmentFile, AttachmentFileDto>());
            Mapper mapper = new Mapper(config);
            return mapper.Map<AttachmentFileDto>(dbAttachment);
        }

        
        public EmailDto FetchEmailById(Guid emailId)
        {
            var db = new FoodAssistanceDatabaseContext();
            var email = db.Email.Where(a => a.Id == emailId).Select(b => new EmailDto()
            {
                Id = b.Id,
                Date = (DateTime)b.Date,
                UserId = (Guid)b.UserId
            });
            return email.Single();
        }

        
        public IEnumerable<EmailDto> GetAnnualReportsFromDate(DateTime date)
        {
            DateTime next = date.AddDays(1);
            FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var apps = db.Email.
                Where(a => a.Date > date && a.Date < next && a.Subject.StartsWith("Annual Report")).
                Select(b => new EmailDto()
                {
                    Id = b.Id,
                    Date = (DateTime)b.Date,
                    UserId = (Guid)b.UserId
                });
            return apps;
        }
        
        public IEnumerable<EmailDto> GetSemiAnnualReportsFromDate(DateTime date)
        {
            DateTime next = date.AddDays(1);
            FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            var apps = db.Email.
                Where(a => a.Date > date && a.Date < next && a.Subject.StartsWith("Semi-Annual Report")).
                Select(b => new EmailDto()
                {
                    Id = b.Id,
                    Date = (DateTime)b.Date,
                    UserId = (Guid)b.UserId
                });
            return apps;
        }

        
        public bool RemoveEmailAttachmentByAttachmentId(Guid attachmentId)
        {
            if (attachmentId == null)
                return false;

            if (attachmentId == null || attachmentId == Guid.Empty)
                return false;

            FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            AttachmentFile file = db.AttachmentFile.FirstOrDefault(a => a.Id == attachmentId);

            if (file == null)
                return false;

            db.AttachmentFile.Remove(file);
            db.SaveChanges();

            return true;

        }

        
        public bool sendEmail(EmailDto dto)
        {
            var mail = new MailMessage();
            using (var smtpServer = new SmtpClient("REPLACE ME"))
                //ConfigurationManager.AppSettings["SmtpServer"]))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                var fromAddress = new MailAddress("");
                //new MailAddress(ConfigurationManager.AppSettings["FromAddress"]);
                mail.From = fromAddress;
                mail.To.Add(dto.ToAddress);
                mail.Subject = dto.Subject;
                mail.Body = dto.Body;
                mail.IsBodyHtml = true;
                try
                {
                    smtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    mail.Dispose();
                    return false;
                }
                mail.Dispose();
                return true;
            }
        }

        public string getLinkedCase(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void WriteLogEntry(EmailDto dto)
        {
            throw new NotImplementedException();
        }

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
    }
}
