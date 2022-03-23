namespace API.Dto.Application
{
    public class LnDbEntryDto
    {
        public Guid UserId { get; set; }
        public string AppId { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public int CVI { get; set; }
        public string RiskData { get; set; }
        public bool PotentiallyFraudulent { get; set; }
        public DateTime? AppStartDate { get; set; }
        public string AccountEmail { get; set; } // Customer Email
        public string AccountTelephone { get; set; } // "Cutomer telephone number Include country and city/area codes.Highly recommended to strip all punctuation and pass as digits only."
        public string AccountAddressStreet1 { get; set; } // "Registration - Customer Address Street Line 
        public string AccountAddressStreet2 { get; set; } // "Registration - Customer Address Street Line 
        public string AccountAddressCity { get; set; } // "Registration - Customer Address City
        public string AccountAddressZip { get; set; } // "Registration - Customer Address Zip
        public string AccountAddressState { get; set; } // "Registration - Customer Address State
        public string AccountAddressCountry { get; set; } // "Registration - Customer Address Country
        public DateTime? AccountDateOfBirth { get; set; } // Customer Date of Birth -  Format required is YYYYMMDD.
        public string AccountFirstName { get; set; } // "Registration - Customer First Name; Login - Customer First Name
        public string AccountLastName { get; set; } // "Registration - Customer Last Name; Login - Customer Last Name
        public string SsnRaw { get; set; }
        public bool Submitted { get; set; }
        public bool Approved { get; set; }
    }
}
