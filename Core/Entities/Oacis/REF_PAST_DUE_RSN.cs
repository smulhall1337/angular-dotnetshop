﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_PAST_DUE_RSN
    {
        public REF_PAST_DUE_RSN()
        {
            CASE_STATUS = new HashSet<CASE_STATUS>();
        }

        public string CD_PAST_DUE_RSN { get; set; }
        public string TX_PAST_DUE_RSN { get; set; }

        public virtual ICollection<CASE_STATUS> CASE_STATUS { get; set; }
    }
}