namespace API.Dto
{
    public record UserLoginDto
    {
        public UserLoginDto(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
            RememberMe = false;
        }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string IPAddress { get; set; }
        public bool RememberMe { get; set; }
        public string Role { get; set; }
    }
}
