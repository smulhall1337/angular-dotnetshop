// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class TANF_MMBR_INCLUDED
    {
        public int ID_TANF_MMBR_INCLD { get; set; }
        public int ID_UNERND_INCM { get; set; }
        public int ID_MMBR { get; set; }

        public virtual MEMBER ID_MMBRNavigation { get; set; }
        public virtual UNEARNED_INCOME ID_UNERND_INCMNavigation { get; set; }
    }
}