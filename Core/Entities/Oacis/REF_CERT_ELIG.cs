// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_CERT_ELIG
    {
        public REF_CERT_ELIG()
        {
            MEMBERID_REF_CERT_ELIGNavigation = new HashSet<MEMBER>();
            MEMBERID_REF_CERT_ELIG_MNTH2Navigation = new HashSet<MEMBER>();
        }

        public short ID_REF_CERT_ELIG { get; set; }
        public string CD_CERT_ELIG { get; set; }
        public string TX_CERT_ELIG_DSCR { get; set; }

        public virtual ICollection<MEMBER> MEMBERID_REF_CERT_ELIGNavigation { get; set; }
        public virtual ICollection<MEMBER> MEMBERID_REF_CERT_ELIG_MNTH2Navigation { get; set; }
    }
}