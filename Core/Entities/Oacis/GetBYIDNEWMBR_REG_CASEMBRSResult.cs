﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetBYIDNEWMBR_REG_CASEMBRSResult
    {
        public int ID_NEWMBR { get; set; }
        public int ID_NEWCASE { get; set; }
        public string DEPNUM { get; set; }
        public string SSN { get; set; }
        public string VERIND { get; set; }
        public string DEP_NM_MMBR_LST { get; set; }
        public string DEP_NM_MMBR_FRST { get; set; }
        public string DEP_NM_MMBR_MDL { get; set; }
        public string DEP_NM_MMBR_SFX { get; set; }
        public string SEX { get; set; }
        public string CD_RACE { get; set; }
        public string DOB { get; set; }
        public string PACASE { get; set; }
        public string ELIG { get; set; }
        public string FL_INVALID_MEMBER { get; set; }
        public string CD_INVALID_MEMBER { get; set; }
        public string FL_HOH { get; set; }
        public string ApplicationID { get; set; }
        public int? PersonID { get; set; }
    }
}
