﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByPrimaryKey_UTILITY_EXPENSEResult
    {
        public int ID_UTLTY_EXPNS { get; set; }
        public int ID_UTLTY_TYPE { get; set; }
        public int ID_MMBR1 { get; set; }
        public int? ID_MMBR2 { get; set; }
        public decimal? AM_ACTL_UTLTY_AVG { get; set; }
        public decimal? AM_UTLTY_EXPNS { get; set; }
        public decimal? AM_UTLTY_EXPNS_MNTHLY { get; set; }
        public decimal? AM_UTLTY_EXPNS_TTL_ALLWBL { get; set; }
        public DateTime? DT_LIHEAP_RCVD { get; set; }
        public string FL_AC_BLLD { get; set; }
        public string FL_ASSSTD_BY_OTHR { get; set; }
        public string FL_BUA { get; set; }
        public string FL_HTNG_BLLD { get; set; }
        public string FL_LIHEAP { get; set; }
        public string FL_PHONE_STNDRD { get; set; }
        public string FL_SUA_ELGBL { get; set; }
        public string FL_UTLTY_PASSTHRU { get; set; }
        public decimal? ID_FRQNCY_UTLTY_EXPNS { get; set; }
        public short? ID_VRFCTN_UTLTY_EXPNS { get; set; }
    }
}
