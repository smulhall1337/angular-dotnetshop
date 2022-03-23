﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class RESOURCE
    {
        public RESOURCE()
        {
            AU_RESOURCE = new HashSet<AU_RESOURCE>();
            MEAL = new HashSet<MEAL>();
            ROOM = new HashSet<ROOM>();
        }

        public short ID_RSRC { get; set; }
        public int ID_MMBR { get; set; }
        public short ID_RSRC_CODE { get; set; }
        public decimal? AM_RSRC_INCM { get; set; }
        public decimal? AM_RSRC_VALUE { get; set; }
        public decimal? AM_TRST_INTRST { get; set; }
        public decimal? AM_WTHDRWL_PNLTY { get; set; }
        public string CD_MONTH { get; set; }
        public string FL_CHLD_TRUST_FUND { get; set; }
        public string FL_CRT_ORDRD { get; set; }
        public string FL_EXCLD_RSRC { get; set; }
        public string FL_JNT_ACCT { get; set; }
        public string FL_RCVNG_TRUST_INTRST { get; set; }
        public string FL_TRUST_ACCSSBL { get; set; }
        public string FL_WTHDRWL_PNLTY { get; set; }
        public decimal? ID_FRQNCY_INTRST { get; set; }
        public short? ID_VRFCTN_RSRC { get; set; }
        public string NM_BANK_INSTTTN { get; set; }
        public string NM_JNT_MMBR_LST { get; set; }
        public string NM_JNT_MMBR_FRST { get; set; }
        public string NM_JNT_MMBR_MDL { get; set; }
        public string NM_JNT_MMBR_SFX { get; set; }
        public string NM_SRC_LCTN { get; set; }
        public string NO_ACCT { get; set; }
        public int? ID_UNERND_INCM { get; set; }
        public string TX_VRFCTN_RSRC { get; set; }

        public virtual MEMBER ID_MMBRNavigation { get; set; }
        public virtual REF_RESOURCE_TYPE ID_RSRC_CODENavigation { get; set; }
        public virtual ICollection<AU_RESOURCE> AU_RESOURCE { get; set; }
        public virtual ICollection<MEAL> MEAL { get; set; }
        public virtual ICollection<ROOM> ROOM { get; set; }
    }
}