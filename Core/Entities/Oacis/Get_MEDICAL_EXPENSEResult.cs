﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_MEDICAL_EXPENSEResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string MEMBER_NAME { get; set; }
        public int ID_MDCL_EXPNS { get; set; }
        //public int ID_MMBR { get; set; }
        public int? ID_TRIPS { get; set; }
        public int ID_PRVDR_NAME { get; set; }
        public string CD_MONTH { get; set; }
        public decimal? AM_AGREED_MNTHLY { get; set; }
        public decimal? AM_CALCLTD_MNTHLY { get; set; }
        public decimal? AM_UNERND_INCM { get; set; }
        public decimal? AM_PAY_MNTHLY { get; set; }
        public decimal? AM_RECALCLTD_MNTHLY { get; set; }
        //public decimal? AM_RECALCLTD_MNTHLY { get; set; }
        public decimal? AM_RMNNG_OWED { get; set; }
        public decimal? AM_ME_TTL_ALLWBL { get; set; }
        public DateTime? DT_SRVC { get; set; }
        public string FL_ASSSTD_BY_OTHR { get; set; }
        public string FL_INSRNC_PAID_ALL { get; set; }
        public string FL_ONETIME { get; set; }
        public string FL_PAY_BILL_MNTHLY { get; set; }
        public string FL_RPYMNT_AGRMNT { get; set; }
        public decimal? ID_FRQNCY_ME_DETAIL { get; set; }
        public short? ID_VRFCTN_MDCL_EXPNS { get; set; }
        public short? ID_VRFCTN_RMNNG { get; set; }
        public string NO_ACCT_MDCL { get; set; }
        public decimal? NO_MEALS_MNTHLY { get; set; }
        public decimal? NO_MNTHS_OWED { get; set; }
        public decimal? NO_MNTHS_RMNNG { get; set; }
        public short ID_ME_OTHR_PAYER { get; set; }
        //public int? ID_MMBR { get; set; }
        //public int ID_MDCL_EXPNS { get; set; }
        public decimal? AM_ME_OTHR_PAY { get; set; }
        public DateTime? DT_ME_OTHR_PAYER { get; set; }
        public string FL_ME_PAY_MMBR { get; set; }
        public short? ID_VRFCTN_ME_OTHR_PAYER { get; set; }
        //public int ID_PRVDR_NAME { get; set; }
        public short ID_PRVDR_TYPE { get; set; }
        public string NM_PRVDR { get; set; }
        public int ID_ADDRSS { get; set; }
        public short ID_CNTY_STATE { get; set; }
        public decimal ID_REF_ADDRSS_TYPE { get; set; }
        public string AD_STREET1 { get; set; }
        public string AD_STREET2 { get; set; }
        public string AD_CITY { get; set; }
        public string AD_STATE { get; set; }
        public string AD_ZIP { get; set; }
        public string AD_ZIP_4 { get; set; }
    }
}
