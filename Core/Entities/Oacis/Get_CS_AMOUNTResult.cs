﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_CS_AMOUNTResult
    {
        public int ID_CS_AMNT { get; set; }
        public int ID_CS_PAYER_RCPNT { get; set; }
        public int? ID_MMBR { get; set; }
        public decimal? AM_CS_CRT_ORDRD_MNTHLY { get; set; }
        public decimal? AM_CS_MNTHLY { get; set; }
        public decimal? AM_NON_RCRRNG_RSRC { get; set; }
        public string DT_DAY_OF_MNTH { get; set; }
        public DateTime? DT_PYMNT_RCVD { get; set; }
        public string FL_ARRRS { get; set; }
        public decimal? ID_FRQNCY_PAY_CS_AMNT { get; set; }
        public short? ID_VRFCTN_CS_AMNT { get; set; }
    }
}