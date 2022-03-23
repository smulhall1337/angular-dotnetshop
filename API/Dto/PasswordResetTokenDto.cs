namespace API.Dto
{
    public class PasswordResetTokenDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
