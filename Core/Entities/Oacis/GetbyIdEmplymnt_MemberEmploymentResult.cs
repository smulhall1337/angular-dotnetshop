// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetbyIdEmplymnt_MemberEmploymentResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string NM_MMBR_MDL { get; set; }
        public string NM_MMBR_SFX { get; set; }
        public string MEMBER_NAME { get; set; }
        public int ID_EMPLYMNT { get; set; }
        //public int ID_MMBR { get; set; }
        public string NM_EMPLYR { get; set; }
        public decimal? AM_YRLY_SLRY { get; set; }
        public decimal? AM_INCM_MNTHLY { get; set; }
        public decimal? AM_INCM_PER_FREQ { get; set; }
        public decimal? AM_TTL_INCM_NXT_MNTH { get; set; }
        public decimal? AM_TTL_INCM_PRVS_MNTH { get; set; }
        public decimal? AM_AVG_MNTHLY { get; set; }
        public decimal? ID_FRQNCY_EMPLYMNT { get; set; }
    }
}
