﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class AU_TRIPS
    {
        public int ID_TRIPS_AUDIT { get; set; }
        public int ID_TRIPS { get; set; }
        public string CD_ACTN_TYPE { get; set; }
        public DateTime? DT_ACTN { get; set; }
        public string NM_ACTN_BY { get; set; }
        public string NM_CLMN { get; set; }
        public string TX_NEW_VALUE { get; set; }
        public string TX_OLD_VALUE { get; set; }

        public virtual TRIP ID_TRIPSNavigation { get; set; }
    }
}