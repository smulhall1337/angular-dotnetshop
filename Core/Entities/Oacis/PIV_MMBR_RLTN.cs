// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class PIV_MMBR_RLTN
    {
        public int ID_PIV_MMBR_RLTN { get; set; }
        public short ID_RLTN { get; set; }
        public int ID_MMBR1 { get; set; }
        public int ID_MMBR2 { get; set; }
        public string FL_NRST_RLTN { get; set; }

        public virtual MEMBER ID_MMBR1Navigation { get; set; }
        public virtual MEMBER ID_MMBR2Navigation { get; set; }
        public virtual REF_RELATION ID_RLTNNavigation { get; set; }
    }
}