// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetbyIDMMBR_SANCTION_ResourcesResult
    {
        public int ID_SNCTN { get; set; }
        public decimal ID_REF_SNCTN_RSN { get; set; }
        public int ID_MMBR { get; set; }
        public DateTime? DT_SNCTN_END { get; set; }
        public DateTime? DT_SNCTN_STRT { get; set; }
        public decimal? NO_SNCTN { get; set; }
        //public decimal ID_REF_SNCTN_RSN { get; set; }
        public short ID_REF_SNCTN_TYPE { get; set; }
        public string TX_SNCTN_RSN { get; set; }
        //public short ID_REF_SNCTN_TYPE { get; set; }
        public string CD_SNCTN_TYPE { get; set; }
    }
}
