// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByPrimaryKey_RBMEALResult
    {
        public int ID_MEAL { get; set; }
        public int? ID_EMPLYMNT { get; set; }
        public int? ID_MMBR { get; set; }
        public short? ID_MEAL_OTHER { get; set; }
        public short? ID_RSRC { get; set; }
        public decimal? AM_MEAL_COST_HOH { get; set; }
        public decimal? AM_MEAL_MMBR_PAY { get; set; }
        public decimal? ID_FRQNCY_MEAL_COST { get; set; }
        public decimal? ID_FRQNCY_MEAL_PAY { get; set; }
        public short? ID_VRFCTN_MEAL { get; set; }
        public decimal? NO_MEALS { get; set; }
        public string CD_MONTH { get; set; }
        public string TXDTL { get; set; }
        public string CD_MONTH_TRANSLATED { get; set; }
        //public int ID_MMBR { get; set; }
        public int? ID_CASE_NMBR { get; set; }
        public string FL_PAYS_ROOM { get; set; }
        public string FL_PAYS_MEAL { get; set; }
        public string MEMBER_NAME { get; set; }
    }
}
