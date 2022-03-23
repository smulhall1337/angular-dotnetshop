
namespace Core.Entities.Dhr
{
    public class CompletedApplicationSummary
    {
        public Guid WorkflowId { get; set; }
        public Guid UserId { get; set; }
        public bool Submitted { get; set; }
        public string? User { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int Attempts { get; set; }
        public string? CaseNumber { get; set; }
        public string? IPAddress { get; set; }
        public bool PotentiallyFraudulent { get; set; }
        public bool OptedOut { get; set; }

        public virtual MyDhrUser MyDhrUser { get; set; }
    }
}
