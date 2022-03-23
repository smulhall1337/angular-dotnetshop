using System.ComponentModel.DataAnnotations;

namespace API.Dto.Store;

public class RegisterDto
{
#pragma warning disable CS8618
    [Required]
    public string DisplayName { get; set; }
#pragma warning restore CS8618
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
        ErrorMessage = "Password must be at least 6 characters, contain 1 uppercase, 1 lowercase and 1 special character")] // regex helps us ensure that PWs meet our requirements 
    public string Password { get; set; }
}