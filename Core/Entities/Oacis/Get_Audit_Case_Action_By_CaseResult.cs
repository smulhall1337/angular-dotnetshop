// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_Audit_Case_Action_By_CaseResult
    {
        public int ID_AUDIT_CASEACTION { get; set; }
        public DateTime ID_DATE_CREATED { get; set; }
        public int ID_CASE_NUMBER { get; set; }
        public decimal NO_CASE { get; set; }
        public string ID_WRKR_SSN { get; set; }
        public string ID_USER { get; set; }
        public string NO_WRKR_NMBR { get; set; }
        public string CD_RJCTN_CLSR { get; set; }
        public string CASE_ACTION { get; set; }
        public DateTime? DT_CASE_STTS { get; set; }
        public string ACTION_STATUS { get; set; }
        public string UPDATE_NOTICE_TYPE { get; set; }
        public string SCI_MSG { get; set; }
        public string CD_CASE_STTS_TYPE { get; set; }
        public int? ID_NMBR_CASELOG { get; set; }
        public decimal? CD_PRCSSN_STNDRD { get; set; }
        public decimal? NO_CASE_CHCKDGT { get; set; }
        public string CD_SPCL_REOPEN_RSN { get; set; }
        public string CD_PAST_DUE_RSN { get; set; }
        public decimal? AM_ALLTMNT { get; set; }
        public decimal? AM_AWARD_ALLTMNT { get; set; }
        public decimal? AM_PEND_ALLTMNT { get; set; }
        public string CD_ADVRS_GUARD { get; set; }
        public string CD_ALLTMNT_TYPE { get; set; }
        public decimal? AM_RSTRTN { get; set; }
        public decimal? AM_RCPMNT { get; set; }
        public string CD_RETRO_RESTORE_RSN { get; set; }
        public DateTime? DT_CERT_END { get; set; }
        public DateTime? DT_CERT_STRT { get; set; }
        public DateTime? DT_STRT_ISSNC { get; set; }
        public DateTime? DT_ADVRS_ACTN { get; set; }
        public DateTime? DT_APPLD { get; set; }
        public DateTime? DT_APPNTMNT { get; set; }
        public DateTime? DT_APPRVL { get; set; }
    }
}
