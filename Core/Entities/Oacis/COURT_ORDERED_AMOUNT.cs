﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class COURT_ORDERED_AMOUNT
    {
        public COURT_ORDERED_AMOUNT()
        {
            ARREARS_CS = new HashSet<ARREARS_CS>();
            AU_COURT_ORDERED_AMOUNT = new HashSet<AU_COURT_ORDERED_AMOUNT>();
            ID_DPNDNT = new HashSet<OUTSIDE_DEPENDENT>();
        }

        public int ID_CRT_ORDRD_AMNT { get; set; }
        public decimal ID_CRT_ORDRD { get; set; }
        public int ID_CS_PAYER_RCPNT { get; set; }
        public decimal? AM_CLCLTD_CRRNT_MNTHLY { get; set; }
        public decimal? AM_CRT_ORDR_MNTHLY { get; set; }
        public decimal? AM_PAY_CRRNT_CS { get; set; }
        public decimal? AM_PAY_HLTH_INS_MNTHLY { get; set; }
        public decimal? AM_PAY_LNDLRD_MNTHLY { get; set; }
        public decimal? AM_PAY_MDBLLS_MNTHLY { get; set; }
        public decimal? AM_PAY_MRTGG_MNTHLY { get; set; }
        public decimal? AM_PAY_UTLTY_MNTHLY { get; set; }
        public decimal? AM_TTL_ALLWBL_CS { get; set; }
        public string CD_MONTH { get; set; }
        public string FL_HLTH_INS_MNTHLY { get; set; }
        public string FL_LNDLRD_MNTHLY { get; set; }
        public string FL_MDBLLS_MNTHLY { get; set; }
        public string FL_MRTGG_MNTHLY { get; set; }
        public string FL_UTLTY_MNTHLY { get; set; }
        public decimal? ID_FRQNCY_CRRNT { get; set; }
        public decimal? ID_FRQNCY_CRT_ORDR { get; set; }
        public short? ID_VRFCTN_CRT_ORDRD_AMT { get; set; }
        public string NO_CRT_ORDR { get; set; }
        public string TX_VRFCTN_CRT_ORDRD_AMT { get; set; }

        public virtual REF_COURT_ORDERED_TYPE ID_CRT_ORDRDNavigation { get; set; }
        public virtual CS_PAYER_RECIPIENT ID_CS_PAYER_RCPNTNavigation { get; set; }
        public virtual ICollection<ARREARS_CS> ARREARS_CS { get; set; }
        public virtual ICollection<AU_COURT_ORDERED_AMOUNT> AU_COURT_ORDERED_AMOUNT { get; set; }

        public virtual ICollection<OUTSIDE_DEPENDENT> ID_DPNDNT { get; set; }
    }
}