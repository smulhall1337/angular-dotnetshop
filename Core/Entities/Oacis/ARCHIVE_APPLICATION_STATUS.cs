﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Core.Entities.Oacis
{
    public partial class ARCHIVE_APPLICATION_STATUS
    {
        public string APPLICATION_ID { get; set; }
        public DateTime? DT_DOWNLOADED { get; set; }
        public DateTime? DT_PROCESSING_START { get; set; }
        public DateTime? DT_PROCESSING_END { get; set; }
        public DateTime? DT_STATUS_SUBMITTED { get; set; }
        public DateTime? DT_APPROVED { get; set; }
        public DateTime? DT_DENIED { get; set; }
        public DateTime? DT_REGISTERED { get; set; }
    }
}