﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REGISTER_DUPLICATEBENEFITS
    {
        public int ID_DUP_BENE { get; set; }
        public int ID_NEWMBR { get; set; }

        public virtual REGISTER_MEMBERS ID_NEWMBRNavigation { get; set; }
    }
}