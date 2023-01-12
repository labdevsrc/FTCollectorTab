using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class SlotBladeTray
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string key { get; set; }
        public string owner_key { get; set; }
        public string rack_key { get; set; }
        public string chassis_key { get; set; }
        public string OWNER_CD { get; set; }
        public string site { get; set; }
        public string slot_or_blade_number { get; set; }
        public string front_back { get; set; }
        public string orientation { get; set; }
        public string manufacturer_key { get; set; }
        public string model_key { get; set; }
        public int temp { get; set; }
    }
}
