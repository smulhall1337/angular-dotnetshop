﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_SHELTER_EXPENSE_OTHER_PAYERResult
    {
        public short ID_SHLTR_EXPNS_OTHR_PAYER { get; set; }
        public int ID_MMBR { get; set; }
        public int ID_SHLTR_EXPNS { get; set; }
        public decimal? AM_SHLTR_EXPNS_OTHR_PAYER { get; set; }
        public DateTime? DT_SHLTR_EXPNS_OTHR_PAYER { get; set; }
        public string FL_SHLTR_EXPNS_PAY_MMBR { get; set; }
        public decimal? ID_FRQNCY_SHLTR_EXPNS_OP { get; set; }
        public short? ID_VRFCTN_SHLTR_EXPNS_OP { get; set; }
    }
}
