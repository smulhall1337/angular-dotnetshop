// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_UNEARNED_PAYER_TYPE
    {
        public REF_UNEARNED_PAYER_TYPE()
        {
            UNEARNED_PAYER = new HashSet<UNEARNED_PAYER>();
        }

        public short ID_REF_UNERND_PAYER_TYPE { get; set; }
        public string CD_REF_UNERND_PAYER { get; set; }
        public string CD_REF_UNERND_PAYER_TYPE { get; set; }

        public virtual ICollection<UNEARNED_PAYER> UNEARNED_PAYER { get; set; }
    }
}