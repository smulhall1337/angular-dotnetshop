﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIDMmbr_INSURANCE_EXPResult
    {
        public int ID_INSRNC { get; set; }
        public decimal ID_INSRNC_PAYER { get; set; }
        public int ID_PRVDR_NAME { get; set; }
        public decimal? AM_PREMIUM { get; set; }
        public decimal? AM_PREMIUM_MNTHLY { get; set; }
        public decimal? AM_TTL_PREMIUM_PAID { get; set; }
        public string CD_MONTH { get; set; }
        public DateTime? DT_PREMIUM_PAID { get; set; }
        public decimal? ID_FRQNCY_PREMIUM { get; set; }
        public short? ID_VRFCTN_INSRNC { get; set; }
        public decimal? NO_HH_MMBR { get; set; }
        public string TX_VRFCTN_INSRNC { get; set; }
        public string TX_VRFCTN_INSRNC_OTHER { get; set; }
        public int? ID_MMBR { get; set; }
    }
}
