﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetTransferResourceSummaryResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string FL_CTGRCLLY_ELGBL { get; set; }
        public string MEMBER_NAME { get; set; }
        public int ID_MMBR_RSRC { get; set; }
        public decimal? TOTAL_TRANS_RESOURCES { get; set; }
    }
}