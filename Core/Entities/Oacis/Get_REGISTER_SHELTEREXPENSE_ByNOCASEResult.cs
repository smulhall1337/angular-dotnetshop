﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class Get_REGISTER_SHELTEREXPENSE_ByNOCASEResult
    {
        public int ID_NEWMBR { get; set; }
        public string DHR_MEMBER_FIRST_NAME { get; set; }
        public string DHR_MEMBER_MIDDLE_NAME { get; set; }
        public string DHR_MEMBER_LAST_NAME { get; set; }
        public string DHR_MEMBER_SUFFIX_NAME { get; set; }
        public string SHELTER_PAY_TYPE { get; set; }
        public string SHELTER_TYPE { get; set; }
        public decimal? RENT_MORTGAGE_AMOUNT { get; set; }
        public decimal? LOT_RENT { get; set; }
        public string LOT_PAY_FREQUENCY { get; set; }
        public decimal? AMT_PROP_TAX { get; set; }
        public decimal? AMT_INSRNC { get; set; }
        public string INSRNC_PAY_FREQUENCY { get; set; }
    }
}
