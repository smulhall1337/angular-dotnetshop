﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_VERIFICATION_DETAIL
    {
        public short ID_VRFCTN_DETAIL { get; set; }
        public decimal ID_REF_VRFCTN_SUBTYPE { get; set; }
        public string CD_VRFCTN_DETAIL_DSCR { get; set; }
        public string FL_OTHR_VRFCTN_RCVD { get; set; }
        public int? ID_VRFCTN_SORT_ORDR { get; set; }
        public string TX_OTHR_VRFCTN { get; set; }

        public virtual REF_VERIFICATION_SUBTYPE ID_REF_VRFCTN_SUBTYPENavigation { get; set; }
    }
}