namespace API.Dto;

public class RegisterDto
{
#pragma warning disable CS8618
    public string DisplayName { get; set; }
#pragma warning restore CS8618
    public string Email { get; set; }
    public string Password { get; set; }
}