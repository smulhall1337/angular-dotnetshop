namespace API.Dto.Email
{
    public class AttachmentFileDto
    {
        public Guid Id { get; set; }
        public Guid EmailId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public DateTime? DateSubmitted { get; set; }
    }
}
