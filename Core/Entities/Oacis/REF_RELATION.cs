// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_RELATION
    {
        public REF_RELATION()
        {
            PIV_MMBR_RLTN = new HashSet<PIV_MMBR_RLTN>();
        }

        public short ID_RLTN { get; set; }
        public string CD_RLTN { get; set; }
        public string TX_RLTN_DSCR { get; set; }

        public virtual ICollection<PIV_MMBR_RLTN> PIV_MMBR_RLTN { get; set; }
    }
}