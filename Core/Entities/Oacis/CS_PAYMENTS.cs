﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class CS_PAYMENTS
    {
        public int ID_CS_PYMNT { get; set; }
        public int ID_CS_AMNT { get; set; }
        public decimal? AM_CS_PYMNT { get; set; }
        public DateTime? DT_CS_PYMNT { get; set; }

        public virtual CS_AMOUNT ID_CS_AMNTNavigation { get; set; }
    }
}