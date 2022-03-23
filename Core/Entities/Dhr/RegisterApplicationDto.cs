namespace Core.Entities.Dhr
{
    public class RegisterApplication
    { 
        public Guid UserId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
