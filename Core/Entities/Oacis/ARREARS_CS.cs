﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class ARREARS_CS
    {
        public ARREARS_CS()
        {
            AU_ARREARS_CS = new HashSet<AU_ARREARS_CS>();
        }

        public int ID_ARRRS_CS { get; set; }
        public int ID_CRT_ORDRD_AMNT { get; set; }
        public decimal? AM_OWED_ARRRS { get; set; }
        public decimal? AM_PAID_ARRRS { get; set; }
        public decimal? ID_FRQNCY_ARRRS { get; set; }

        public virtual COURT_ORDERED_AMOUNT ID_CRT_ORDRD_AMNTNavigation { get; set; }
        public virtual ICollection<AU_ARREARS_CS> AU_ARREARS_CS { get; set; }
    }
}