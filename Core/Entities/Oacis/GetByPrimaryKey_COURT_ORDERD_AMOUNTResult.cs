﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByPrimaryKey_COURT_ORDERD_AMOUNTResult
    {
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
        public string FL_HLTH_INS_MNTHLY { get; set; }
        public string FL_LNDLRD_MNTHLY { get; set; }
        public string FL_MDBLLS_MNTHLY { get; set; }
        public string FL_MRTGG_MNTHLY { get; set; }
        public string FL_UTLTY_MNTHLY { get; set; }
        public decimal? ID_FRQNCY_CRRNT { get; set; }
        public decimal? ID_FRQNCY_CRT_ORDR { get; set; }
        public short? ID_VRFCTN_CRT_ORDRD_AMT { get; set; }
        public string NO_CRT_ORDR { get; set; }
        public decimal? AM_TTL_ALLWBL_CS { get; set; }
        public string CD_MONTH { get; set; }
        public string CD_MONTH_TRANSLATED { get; set; }
    }
}
