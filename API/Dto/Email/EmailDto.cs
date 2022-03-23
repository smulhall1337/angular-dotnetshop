namespace API.Dto.Email
{
    public class EmailDto
    {
        public Guid Id { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public List<AttachmentFileDto> AttachmentFiles { get; set; }
    }
}
