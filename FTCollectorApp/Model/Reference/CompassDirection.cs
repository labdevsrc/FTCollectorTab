using System;
using System.Collections.Generic;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class CompassDirection
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CompasKey { get; set; }
        public string CompassDirDesc { get; set; }
        public string ITSFM { get; set; }
    }
}
