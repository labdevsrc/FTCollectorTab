using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class EquipmentCO
    {
        [AutoIncrement,PrimaryKey]
        public int id { get; set; }
        public string TypeKey { get; set; }
        public string EqKey { get; set; }
        public string EquipmentTypeDesc { get; set; }
        public string EqDesc { get; set; }
    }
}
