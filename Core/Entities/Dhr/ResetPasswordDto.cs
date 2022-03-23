namespace Core.Entities.Dhr
{
    public class ResetPassword
    {
        public Guid ResetToken { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
