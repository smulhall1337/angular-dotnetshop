﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_FEDERAL_MILEAGE_RATE
    {
        public REF_FEDERAL_MILEAGE_RATE()
        {
            TRIP = new HashSet<TRIP>();
        }

        public short ID_REF_FDRL_MLG_RATE { get; set; }
        public decimal? AM_FDRL_MLG_RATE { get; set; }
        public DateTime? DT_EFFCTV { get; set; }

        public virtual ICollection<TRIP> TRIP { get; set; }
    }
}