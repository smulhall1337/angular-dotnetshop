﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdCaseNmbr_SumChildSupportExpenseResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string MEMBER_NAME { get; set; }
        public string EXPENSE_TYPE { get; set; }
        public decimal? TOTAL_EXPENSE { get; set; }
    }
}