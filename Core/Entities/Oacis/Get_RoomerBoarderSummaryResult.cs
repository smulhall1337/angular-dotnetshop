// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_RoomerBoarderSummaryResult
    {
        public int ID_MMBR { get; set; }
        public int? ID_CASE_NMBR { get; set; }
        public string FL_PAYS_ROOM { get; set; }
        public string FL_PAYS_MEAL { get; set; }
        public string MEMBER_NAME { get; set; }
        public int ID_MEAL { get; set; }
        public int ID_ROOM { get; set; }
        public decimal? AMT_PAID_MNTHLY { get; set; }
        public string TYPE { get; set; }
        public string CD_MONTH_TRANSLATED { get; set; }
    }
}
