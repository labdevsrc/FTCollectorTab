using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class DevType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string DevTypeDesc { get; set; }
        public string DevTypeKey { get; set; }
        public string DevTag { get; set; }
    }
}
