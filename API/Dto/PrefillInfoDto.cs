﻿namespace API.Dto
{
    public class PrefillInfoDto
    {
        public MailingAddressDto HomeAddress { get; set; }
        public MailingAddressDto MailingAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Dob { get; set; }
    }
}
