using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class EquipmentType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string EquipCodeKey { get; set; }
        public string EquipCodeDesc { get; set; }
    }
}
