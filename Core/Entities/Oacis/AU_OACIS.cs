// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class AU_OACIS
    {
        public int ID_AU_OACIS { get; set; }
        public string TX_TABLE_NAME { get; set; }
        public short? TX_TABLE_KEY { get; set; }
        public short ID_USER { get; set; }
        public int ID_MMBR { get; set; }
        public short ID_AUDIT_CTGRY { get; set; }
        public DateTime? DT_AUDIT_NOTE { get; set; }
        public DateTime? DT_CRTD { get; set; }
        public string TX_CASE_NOTE_DSCR { get; set; }
    }
}