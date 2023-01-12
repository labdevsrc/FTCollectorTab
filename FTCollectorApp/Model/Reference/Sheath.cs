using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model.Reference
{
    public class Sheath
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string SheathKey { get; set; }
        public string SheathType { get; set; }
    }
}
