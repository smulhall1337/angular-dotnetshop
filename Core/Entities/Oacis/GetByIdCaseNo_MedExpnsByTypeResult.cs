// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdCaseNo_MedExpnsByTypeResult
    {
        public string MEMBER_NAME { get; set; }
        public string TX_PRVDR_DSCR { get; set; }
        public string NM_PRVDR { get; set; }
        public int ID_MDCL_EXPNS { get; set; }
        public string NO_ACCT_MDCL { get; set; }
        public decimal? AM_TTL_OWED { get; set; }
        public decimal? AM_RECALCLTD_MNTHLY { get; set; }
        public decimal? AM_UNERND_INCM { get; set; }
        public string CD_MNTH { get; set; }
        public string CD_VRFCTN_DETAIL_DSCR { get; set; }
        public string milesVerification { get; set; }
        public string TX_MDCTN_APPLNC_DSCR { get; set; }
    }
}
