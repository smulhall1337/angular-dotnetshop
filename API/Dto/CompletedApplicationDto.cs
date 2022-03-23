namespace API.Dto
{
    public class CompletedApplicationDto
    {
        public Guid WorkflowId { get; set; }
        public Guid UserId { get; set; }
        public string WorkflowXml { get; set; }
        public bool Submitted { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int Attempts { get; set; }
        public string CaseNumber { get; set; }
        public bool FailedNotificationSent { get; set; }
    }
}
