// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Oacis
{
    public partial class GetByIdMmbr_ROOMResult
    {
        public int ID_ROOM { get; set; }
        public int? ID_EMPLYMNT { get; set; }
        public int ID_MMBR { get; set; }
        public short? ID_RSRC { get; set; }
        public decimal? AM_ROOM_PAY { get; set; }
        public decimal? ID_FRQNCY_ROOM_PAY { get; set; }
        public short? ID_VRFCTN_ROOM { get; set; }
        public string CD_MONTH { get; set; }
        public string CD_MONTH_TRANSLATED { get; set; }
    }
}
