namespace Core.Entities.Dhr
{
    public class Session
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public UserLogin User { get; set; }
        public Guid AccountRepId { get; set; }
        public string CaseNumber { get; set; }
        public byte[] IPAddress { get; set; }
    }
}
