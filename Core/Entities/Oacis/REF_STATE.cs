﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_STATE
    {
        public REF_STATE()
        {
            REF_COUNTY_STATE = new HashSet<REF_COUNTY_STATE>();
        }

        public string ID_REF_STATE { get; set; }
        public string CD_STATE_DSCR { get; set; }

        public virtual ICollection<REF_COUNTY_STATE> REF_COUNTY_STATE { get; set; }
    }
}