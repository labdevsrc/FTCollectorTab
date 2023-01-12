using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Chassis
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ChassisNum { get; set; }
        public string OWNER_CD { get; set; }
        public string ChassisKey { get; set; }
        public string ModelKey { get; set; }
        public string TagNumber { get; set; }
        public string rack_number { get; set; }
        public string Model { get; set; }
        public int temp { get; set; }
    }
}
