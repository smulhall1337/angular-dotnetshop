﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_WAIVE_ADVERSE_ACTION
    {
        public REF_WAIVE_ADVERSE_ACTION()
        {
            WAIVE_ADVERSE_ACTION = new HashSet<WAIVE_ADVERSE_ACTION>();
        }

        public short ID_REF_WAIVE_ADVRS_ACTN { get; set; }
        public string CD_WAIVE_ADVRS_ACTN_RSN { get; set; }
        public string TX_ADVRS_ACTN_DSCR { get; set; }

        public virtual ICollection<WAIVE_ADVERSE_ACTION> WAIVE_ADVERSE_ACTION { get; set; }
    }
}