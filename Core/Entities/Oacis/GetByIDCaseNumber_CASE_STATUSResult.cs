﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIDCaseNumber_CASE_STATUSResult
    {
        public int ID_CASE_NMBR { get; set; }
        public decimal ID_REF_CASE_STATUS_TYPE { get; set; }
        public decimal? AM_ALLTMNT { get; set; }
        public DateTime? DT_CASE_STTS { get; set; }
        public DateTime? DT_CERT_END { get; set; }
        public DateTime? DT_CERT_STRT { get; set; }
        public DateTime? DT_STRT_ISSNC { get; set; }
        //public decimal ID_REF_CASE_STATUS_TYPE { get; set; }
        public string CD_CASE_STTS_TYPE { get; set; }
        public string TX_CASE_STTS_DSCR { get; set; }
    }
}
