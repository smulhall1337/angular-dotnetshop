namespace API.Dto
{
    public class RegisterApplicationDto
    { 
        public Guid UserId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
