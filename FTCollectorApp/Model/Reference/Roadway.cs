using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Roadway
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string RoadwayKey { get; set; }
        public string RoadwayName { get; set; }

        public string RoadOwnerKey { get; set; }


    }
}
