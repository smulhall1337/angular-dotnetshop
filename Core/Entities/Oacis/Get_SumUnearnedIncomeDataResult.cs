﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_SumUnearnedIncomeDataResult
    {
        public int ID_MMBR { get; set; }
        public decimal? TOTAL_UNEARNED_INC_AMNT { get; set; }
        public string CD_UNERND_SUBTYPE { get; set; }
        public string TX_UNERND_SUBTYPE { get; set; }
    }
}
