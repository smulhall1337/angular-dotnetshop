namespace API.Dto
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public UserLoginDto User { get; set; }
        public Guid AccountRepId { get; set; }
        public string CaseNumber { get; set; }
        public byte[] IPAddress { get; set; }
    }
}
