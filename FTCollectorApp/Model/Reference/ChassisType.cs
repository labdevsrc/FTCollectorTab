using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class ChassisType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CTKey { get; set; }
        public string CTDesc { get; set; }

    }
}
