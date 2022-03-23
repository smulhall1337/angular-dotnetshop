namespace Core.Entities.Dhr
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
