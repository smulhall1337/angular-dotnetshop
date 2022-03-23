﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class EMPLOYMENT
    {
        public EMPLOYMENT()
        {
            ADDRESS = new HashSet<ADDRESS>();
            AU_EMPLOYMENT = new HashSet<AU_EMPLOYMENT>();
            MEAL = new HashSet<MEAL>();
            ROOM = new HashSet<ROOM>();
            STRIKE = new HashSet<STRIKE>();
        }

        public int ID_EMPLYMNT { get; set; }
        public int ID_MMBR { get; set; }
        public decimal ID_INCM_TYPE { get; set; }
        public decimal? AM_AVG_MNTHLY { get; set; }
        public decimal? AM_CNTRCT_INCM_MNTHLY { get; set; }
        public decimal? AM_ERND_SELF_EMPLYMNT_INCM { get; set; }
        public decimal? AM_ERND_SELF_EMPLYMNT_INCM2 { get; set; }
        public decimal? AM_INCM_PER_FREQ { get; set; }
        public decimal? AM_INCM_MNTHLY { get; set; }
        public decimal? AM_TTL_INCM { get; set; }
        public decimal? AM_TTL_INCM_NXT_MNTH { get; set; }
        public decimal? AM_TTL_INCM_PRVS_MNTH { get; set; }
        public decimal? AM_UNERND_SELF_EMPLYMNT_INCM { get; set; }
        public decimal? AM_UNERND_SELF_EMPLYMNT_INCM2 { get; set; }
        public decimal? AM_YRLY_SLRY { get; set; }
        public DateTime? DT_EMPLYMNT_END { get; set; }
        public DateTime? DT_EMPLYMNT_STRT { get; set; }
        public string FL_GOOD_RSN_QUIT { get; set; }
        public string FL_MNG_PRPRTY { get; set; }
        public string FL_QLFY_STMPS { get; set; }
        public string FL_SELF_EMPLYD { get; set; }
        public string FL_TTL_RPRSNTTV_PAY { get; set; }
        public string FL_VLNTRY_QUIT { get; set; }
        public decimal? ID_FRQNCY_EMPLYMNT { get; set; }
        public int? ID_VRFCTN_EMPLYMNT { get; set; }
        public string NM_EMPLYR { get; set; }
        public string NM_INSRNC_WCOMP { get; set; }
        public string NM_PRPRTY { get; set; }
        public decimal? NO_HOURS_MNG_PRPRTY { get; set; }
        public decimal? NO_HOURS_MNG_PRPRTY_MTH2 { get; set; }
        public decimal? NO_HOURS_WRK_PRWK_MTH1 { get; set; }
        public decimal? NO_HOURS_WRK_PRWK_MTH2 { get; set; }
        public string NO_EMPLYR_PHONE { get; set; }
        public decimal? NO_MNTHS { get; set; }
        public string TX_BSNSS_TYPE { get; set; }
        public string TX_PAY_DAY { get; set; }
        public string TX_RSN_QUIT { get; set; }
        public string TX_SLCTD_FLDS { get; set; }
        public int? ID_EMPLOYER { get; set; }
        public string TX_VRFCTN_EMPLYMNT { get; set; }
        public int? ID_WRKR_RMNDR { get; set; }
        public DateTime? DT_VQ_Exemption { get; set; }
        public decimal? CD_CNTY { get; set; }
        public string AD_STREET1 { get; set; }
        public string AD_CITY { get; set; }
        public string AD_STATE { get; set; }
        public string AD_ZIP { get; set; }

        public virtual REF_EARNED_INCOME_TYPE ID_INCM_TYPENavigation { get; set; }
        public virtual MEMBER ID_MMBRNavigation { get; set; }
        public virtual ICollection<ADDRESS> ADDRESS { get; set; }
        public virtual ICollection<AU_EMPLOYMENT> AU_EMPLOYMENT { get; set; }
        public virtual ICollection<MEAL> MEAL { get; set; }
        public virtual ICollection<ROOM> ROOM { get; set; }
        public virtual ICollection<STRIKE> STRIKE { get; set; }
    }
}