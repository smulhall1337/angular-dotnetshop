// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_SANCTION_END_NXTMTH_FAWResult
    {
        public int? ID_SNCTN { get; set; }
        public decimal? ID_REF_SNCTN_RSN { get; set; }
        public string SnctnRsn { get; set; }
        public int? ID_MMBR { get; set; }
        public DateTime? DT_SNCTN_END { get; set; }
        public decimal NO_CASE { get; set; }
        public string NO_WRKR_NMBR { get; set; }
        public string CD_CNTY_DSCR { get; set; }
        //public int? ID_MMBR { get; set; }
        public string FL_HOH { get; set; }
        public string FL_SNCTN { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string NM_MMBR_MDL { get; set; }
        public string NM_MMBR_SFX { get; set; }
        public string MEMBER_NAME { get; set; }
        public string HOHnmn { get; set; }
    }
}
