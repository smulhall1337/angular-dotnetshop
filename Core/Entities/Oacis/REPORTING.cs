﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REPORTING
    {
        public int ID_RPRTNG { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string CD_RPRTNG_RQRMNT { get; set; }
        public DateTime? DT_RPRTNG { get; set; }
        public string TX_RPRTNG_RQRMNT_DSCR { get; set; }

        public virtual CASE ID_CASE_NMBRNavigation { get; set; }
    }
}