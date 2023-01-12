using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    
    public class AFiberCable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string AFRKey { get; set; }
        public string CableIdDesc { get; set; }
        public string CableType { get; set; }
        public string CableTypeDesc { get; set; }
        public string JobKey { get; set; }
        public string JobNumber { get; set; }
        public string OwnerKey { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SMCount { get; set; }
        public string MMCount { get; set; }
        public string MM_diameter { get; set; }
        public string SheathType { get; set; }
        public string buffer_count { get; set; }
        public string label { get; set; }
        public string FiberSegmentIdx { get; set; } // for Tracer Page


    }
}
