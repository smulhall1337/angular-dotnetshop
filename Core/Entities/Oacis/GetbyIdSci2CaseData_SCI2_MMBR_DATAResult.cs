﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetbyIdSci2CaseData_SCI2_MMBR_DATAResult
    {
        public int ID_SCI2_MMBR_DATA { get; set; }
        public int ID_NMBR_CASELOG { get; set; }
        public int ID_SCI2_CASE_DATA { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public int? ID_MMBR { get; set; }
        public string CD_REC_TYPE { get; set; }
        public int? NO_DEPENDENT_NBR { get; set; }
        public string NO_SSN { get; set; }
        public string NM_DEPENDENT { get; set; }
        public string CD_SEX { get; set; }
        public string CD_RACE { get; set; }
        public string DT_DOB { get; set; }
        public string CD_WORK_REG { get; set; }
        public string CD_CERT_ELIG { get; set; }
        public string AM_EARNED_INC { get; set; }
        public string AM_UNERND_INC { get; set; }
        public string AM_PA { get; set; }
        public string AM_SSI_VA { get; set; }
        public string CD_EARNED_INC_TYPE { get; set; }
        public string TX_EARNED_INC_TYPE { get; set; }
        public string CD_UNERND_INC_TYPE { get; set; }
        public string TX_UNEARNED_TYPE { get; set; }
        public string CD_PA_TYPE { get; set; }
        public string TX_PA_TYPE { get; set; }
        public string CD_SSI_TYPE { get; set; }
        public string TX_SSIVA_TYPE { get; set; }
        public string AM_SSA { get; set; }
        public string AM_MEDICARE { get; set; }
        public string AM_CS_RCVD { get; set; }
        public string AM_CS_PAID { get; set; }
        public string CD_MEDICARE_TYPE { get; set; }
        public string CD_CS_RCVD_TYPE { get; set; }
        public string TX_CSRCVD_TYPE { get; set; }
        public string CD_CS_PAID_TYPE { get; set; }
        public string TX_CSPAID_TYPE { get; set; }
        public string PA_FILE_NBR { get; set; }
        public string PA_CASE_NBR { get; set; }
        public string CHILD_SUPPORT_NBR { get; set; }
        public string ENUM { get; set; }
    }
}
