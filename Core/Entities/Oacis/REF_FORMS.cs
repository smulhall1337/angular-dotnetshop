﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REF_FORMS
    {
        public REF_FORMS()
        {
            FORMS_OACIS = new HashSet<FORMS_OACIS>();
        }

        public int ID_FORM { get; set; }
        public string CD_FORM { get; set; }
        public string TXT_FORM { get; set; }

        public virtual ICollection<FORMS_OACIS> FORMS_OACIS { get; set; }
    }
}