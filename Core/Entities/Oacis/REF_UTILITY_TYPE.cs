﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_UTILITY_TYPE
    {
        public REF_UTILITY_TYPE()
        {
            UTILITY_EXPENSE = new HashSet<UTILITY_EXPENSE>();
        }

        public int ID_UTLTY_TYPE { get; set; }
        public string CD_UTLTY_TYPE { get; set; }

        public virtual ICollection<UTILITY_EXPENSE> UTILITY_EXPENSE { get; set; }
    }
}