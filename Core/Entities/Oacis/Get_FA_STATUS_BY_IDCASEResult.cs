﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_FA_STATUS_BY_IDCASEResult
    {
        public int ID_FA_STTS { get; set; }
        public int? ID_MMBR { get; set; }
        public short ID_CNTY_STATE { get; set; }
        public DateTime? DT_CASE_CLSD_WILLORHAS { get; set; }
        public string FL_FA_CASE_CLSD { get; set; }
        public string FL_LAST_MNTH_FA { get; set; }
        public string FL_RECERT { get; set; }
        public string FL_THIS_CNTY { get; set; }
        public short? ID_VRFCTN_FA_STTS { get; set; }
        public string TX_CNTCT_INFO { get; set; }
        public string TX_CNTY_OTHR { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string MEMBER_NAME { get; set; }
    }
}