// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetBySearchADDRESSResult
    {
        public int ID_ADDRSS { get; set; }
        public int? ID_MMBR { get; set; }
        public decimal ID_REF_ADDRSS_TYPE { get; set; }
        public string AD_STREET1 { get; set; }
        public string AD_CITY { get; set; }
        public string AD_STATE { get; set; }
        public string AD_ZIP { get; set; }
        public string AD_ZIP_4 { get; set; }
    }
}
