﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class REGISTER_EMPLOYMENT
    {
        public int ID_EMPLOYMENT { get; set; }
        public int? ID_NEWMBR { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_PHONE_NBR { get; set; }
        public decimal? PAY_AMOUNT { get; set; }
        public string PAY_FREQUENCY { get; set; }
        public int? HOURS_WORKED { get; set; }

        public virtual REGISTER_MEMBERS ID_NEWMBRNavigation { get; set; }
    }
}