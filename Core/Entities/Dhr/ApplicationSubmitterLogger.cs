﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
namespace Core.Entities.Dhr
{
    public partial class ApplicationSubmitterLogger
    {
        public int Id { get; set; }
        public string AppSource { get; set; }
        public string EventLog { get; set; }
        public string EventEntry { get; set; }
        public bool? HasError { get; set; }
        public DateTime? EntryDate { get; set; }
    }
}