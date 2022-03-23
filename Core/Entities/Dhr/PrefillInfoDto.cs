namespace Core.Entities.Dhr
{
    public class PrefillInfo
    {
        public MailingAddress HomeAddress { get; set; }
        public MailingAddress MailingAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Dob { get; set; }
    }
}
