// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdCaseNmbr_ResourceTotalsResult
    {
        public int ID_MMBR { get; set; }
        public int ID_CASE_NMBR { get; set; }
        public string MEMBER_NAME { get; set; }
        public string CD_CASE_CRTD { get; set; }
        public string CD_MONTH { get; set; }
        public decimal? TOTAL_RESOURCES { get; set; }
        public string RESOURCE_TYPE { get; set; }
        public string TRANSFERRED_RESOURCE { get; set; }
        public string CD_VRFCTN_DETAIL_DSCR { get; set; }
    }
}
