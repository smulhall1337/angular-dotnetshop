// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdCaseNmbr_RESOURCE_M2Result
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string FL_CTGRCLLY_ELGBL { get; set; }
        public string NM_MMBR_LST { get; set; }
        public string NM_MMBR_FRST { get; set; }
        public string NM_MMBR_MDL { get; set; }
        public string NM_MMBR_SFX { get; set; }
        public string MEMBER_NAME { get; set; }
        public int RESOURCE_ID { get; set; }
        //public int ID_MMBR { get; set; }
        public short ID_RSRC_CODE { get; set; }
        public decimal? AM_RSRC_INCM { get; set; }
        public decimal? AM_RSRC_VALUE { get; set; }
        public string FL_EXCLD_RSRC { get; set; }
        public string CD_MONTH { get; set; }
        public string CD_MONTH_TRANSLATED { get; set; }
        public int? ID_UNERND_INCM { get; set; }
        //public short ID_RSRC_CODE { get; set; }
        public string CD_RSRC_CODE { get; set; }
        public string TX_RSRC_DSCR { get; set; }
        public string TRANSFERRED_RESOURCE { get; set; }
    }
}
