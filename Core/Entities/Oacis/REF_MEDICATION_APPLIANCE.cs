﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_MEDICATION_APPLIANCE
    {
        public REF_MEDICATION_APPLIANCE()
        {
            MEDICAL_EXPENSE = new HashSet<MEDICAL_EXPENSE>();
        }

        public int ID_MDCTN_APPLNC { get; set; }
        public int ID_PRVDR_NAME { get; set; }
        public string TX_MDCTN_APPLNC_DSCR { get; set; }

        public virtual REF_PROVIDER_NAME ID_PRVDR_NAMENavigation { get; set; }
        public virtual ICollection<MEDICAL_EXPENSE> MEDICAL_EXPENSE { get; set; }
    }
}