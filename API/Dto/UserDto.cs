namespace API.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public Guid MyAlabamaUserId { get; set; }
        public string MyAlabamaUserName { get; set; }
        public DateTime? MyAlabamaLinkDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string SSN { get; set; }
        public string SSNHash { get; set; }
        public string Sex { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string SecurityQuestion3 { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityAnswer2 { get; set; }
        public string SecurityAnswer3 { get; set; }
        public DateTime? PasswordLastReset { get; set; }
        public int PasswordAttempts { get; set; }

        private List<string> _roles = new List<string>();
        public List<string> Roles { get { return _roles; } }
    }
}
