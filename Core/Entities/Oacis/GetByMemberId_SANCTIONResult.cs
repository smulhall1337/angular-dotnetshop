﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByMemberId_SANCTIONResult
    {
        public int ID_SNCTN { get; set; }
        public decimal ID_REF_SNCTN_RSN { get; set; }
        public int ID_MMBR { get; set; }
        public DateTime? DT_SNCTN_END { get; set; }
        public DateTime? DT_SNCTN_STRT { get; set; }
        public decimal? NO_SNCTN { get; set; }
        public int? ID_WRKR_RMNDR { get; set; }
        public DateTime? DT_COMPLYOREXEMPT { get; set; }
    }
}
