// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_MEDICAL_NOTE_TYPE
    {
        public REF_MEDICAL_NOTE_TYPE()
        {
            MEDICAL_NOTES = new HashSet<MEDICAL_NOTES>();
        }

        public short ID_REF_MDCL_NOTE_TYPE { get; set; }
        public string CD_REF_MDCL_NOTE_TYPE { get; set; }
        public string TX_REF_MDCL_NOTE_TYPE_DSCR { get; set; }

        public virtual ICollection<MEDICAL_NOTES> MEDICAL_NOTES { get; set; }
    }
}