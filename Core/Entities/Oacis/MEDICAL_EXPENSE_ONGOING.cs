﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class MEDICAL_EXPENSE_ONGOING
    {
        public int ID_ME_ONGOING { get; set; }
        public int ID_MDCL_EXPNS { get; set; }
        public decimal? AM_MNTHLY_ONGOING { get; set; }
        public DateTime? DT_TRNSCTN { get; set; }
        public decimal? NO_OF_MNTHS { get; set; }

        public virtual MEDICAL_EXPENSE ID_MDCL_EXPNSNavigation { get; set; }
    }
}