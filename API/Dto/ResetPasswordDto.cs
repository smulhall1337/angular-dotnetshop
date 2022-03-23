namespace API.Dto
{
    public class ResetPasswordDto
    {
        public Guid ResetToken { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
