// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class MEDICAL_NOTES
    {
        public int ID_MDCL_NOTES { get; set; }
        public int ID_MDCL_EXPNS { get; set; }
        public short ID_REF_MDCL_NOTE_TYPE { get; set; }
        public string TX_MDCL_NOTES_DSCR { get; set; }

        public virtual MEDICAL_EXPENSE ID_MDCL_EXPNSNavigation { get; set; }
        public virtual REF_MEDICAL_NOTE_TYPE ID_REF_MDCL_NOTE_TYPENavigation { get; set; }
    }
}