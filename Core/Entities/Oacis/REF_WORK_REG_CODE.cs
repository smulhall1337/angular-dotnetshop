﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_WORK_REG_CODE
    {
        public REF_WORK_REG_CODE()
        {
            MEMBERID_WORK_CODENavigation = new HashSet<MEMBER>();
            MEMBERID_WORK_CODE_MNTH2Navigation = new HashSet<MEMBER>();
        }

        public short ID_WORK_CODE { get; set; }
        public string CD_WORK { get; set; }
        public string TX_WORK_DSCR { get; set; }

        public virtual ICollection<MEMBER> MEMBERID_WORK_CODENavigation { get; set; }
        public virtual ICollection<MEMBER> MEMBERID_WORK_CODE_MNTH2Navigation { get; set; }
    }
}