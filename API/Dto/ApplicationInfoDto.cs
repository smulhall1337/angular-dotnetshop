namespace API.Dto
{
    public class ApplicationInfoDto
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowXml { get; set; }
        public Guid UserId { get; set; }
        public bool Submitted { get; set; }
        public UserDto User { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string County { get; set; }
        public int Attempts { get; set; }
        public string CaseNumber { get; set; }
        public string IPAddress { get; set; }
    }
}
