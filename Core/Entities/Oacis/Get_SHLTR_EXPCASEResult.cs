﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_SHLTR_EXPCASEResult
    {
        public int ID_SHLTR_EXPNS { get; set; }
        public int ID_MMBR1 { get; set; }
        public int? ID_MMBR2 { get; set; }
        public short ID_OWNRSHP { get; set; }
        public short ID_SHLTR_DWELLNG_TYPE { get; set; }
        public short ID_SHLTR_EXPNS_TYPE { get; set; }
        public decimal? AM_ACTL_SHLTR_EXPNS { get; set; }
        public decimal? AM_SHLTR_EXPNS_MNTHLY { get; set; }
        public string FL_ASSSTD_BY_OTHR { get; set; }
        public string FL_SHLTR_PASSTHRU { get; set; }
        public decimal? ID_FRQNCY_SHLTR_EXPNS { get; set; }
        public short? ID_VRFCTN_SHLTR_EXPNS { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public short ID_REF_CERT_ELIG { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string MEMBER_NAME { get; set; }
    }
}
