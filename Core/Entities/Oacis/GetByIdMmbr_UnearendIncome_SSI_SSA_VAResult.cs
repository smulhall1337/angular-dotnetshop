// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdMmbr_UnearendIncome_SSI_SSA_VAResult
    {
        public int ID_UNERND_INCM { get; set; }
        public int ID_MMBR { get; set; }
        public int? ID_CS_AMNT { get; set; }
        public int ID_REF_UNERND_SUBTYPE { get; set; }
        public decimal? AM_UNERND_MNTHLY { get; set; }
        public decimal? AM_UNERND_VALUE1 { get; set; }
        public decimal? AM_UNERND_VALUE2 { get; set; }
        public short? ID_VRFCTN_UNERND_INCM { get; set; }
        public string CD_MONTH { get; set; }
        public int? NO_PYMNT_MNTHS { get; set; }
        //public int? ID_REF_UNERND_SUBTYPE { get; set; }
        public string TX_UNERND_SUBTYPE { get; set; }
    }
}
