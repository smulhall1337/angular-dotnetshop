﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
namespace Core.Entities.Dhr
{
    public partial class CompletedApplication
    {
        public Guid WorkflowId { get; set; }
        public Guid UserId { get; set; }
        public string WorkflowXml { get; set; }
        public bool Submitted { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int Attempts { get; set; }
        public string CaseNumber { get; set; }
        public bool? FailedNotificationSent { get; set; }

        public virtual MyDhrUser MyDhrUser { get; set; }
    }
}