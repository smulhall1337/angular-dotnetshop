﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIDCaseNmbr_MEMBERS_DisasterResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string CD_SEX { get; set; }
        public DateTime? DT_BRTH { get; set; }
        public string FL_HOH { get; set; }
        public string NO_SSN { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string NM_MMBR_MDL { get; set; }
        public string NM_MMBR_SFX { get; set; }
        public string MEMBER_NAME { get; set; }
        public short ID_RACE_CODE { get; set; }
        public decimal? CD_RACE { get; set; }
        public string TX_RACE_DSCR { get; set; }
        public short? ID_WORK_CODE { get; set; }
        public string CD_WORK { get; set; }
        public short? ID_REF_CERT_ELIG { get; set; }
        public string CD_CERT_ELIG { get; set; }
    }
}