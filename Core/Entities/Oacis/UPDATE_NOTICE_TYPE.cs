﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class UPDATE_NOTICE_TYPE
    {
        public int ID_UPDT_NTC { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public short? ID_REF_NTC_TYPE { get; set; }
        public string CD_LANGUAGE_INDCTR { get; set; }
        public DateTime? DT_UPDT_NTC { get; set; }

        public virtual CASE ID_CASE_NMBRNavigation { get; set; }
        public virtual REF_NOTICE_TYPE ID_REF_NTC_TYPENavigation { get; set; }
    }
}